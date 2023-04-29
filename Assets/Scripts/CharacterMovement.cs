using UnityEngine;

namespace Shooter3D
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;

        [Header("Movement")]
        [SerializeField] private float rifleRunSpeed;
        [SerializeField] private float rifleSprintSpeed;
        [SerializeField] private float aimingWalkSpeed;
        [SerializeField] private float aimingRunSpeed;
        [SerializeField] private float accelerationRate;
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float jumpSpeed;

        [Header("State")]
        [SerializeField] private float crouchHeight;
        private bool isAiming;
        private bool isJump;
        private bool isCrouch;
        private bool isSprint;
        private float distanceToGround;

        // Public
        [HideInInspector] public Vector3 TargetDirectionControl;
        public float JumpSpeed => jumpSpeed;
        public float CrouchHeight => crouchHeight;
        public bool IsCrouch => isCrouch;
        public bool IsSprint => isSprint;
        public bool IsAiming => isAiming;
        public float DistanceToGround => distanceToGround;

        // Private
        private float BaseCharacterHeight;
        private float BaseCharacterHeightOffset;

        private Vector3 directionControl;
        private Vector3 movementDirection;


        private void Start()
        {
            BaseCharacterHeight = characterController.height;
            BaseCharacterHeightOffset = characterController.center.y;
        }

        private void Update()
        {
            Move();
            UpdateDistanceToGround();
        }


        public void Jump()
        {
            //if (characterController.isGrounded == false) return;
            if (distanceToGround > 0.1f) return;
            if (isCrouch) return;

            isJump = true;
        }

        public void Crouch()
        {
            if (characterController.isGrounded == false) return;
            if (isSprint) return;
            isCrouch = true;
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0, characterController.center.y / 2, 0);
        }
        public void UnCrouch()
        {
            isCrouch = false;
            characterController.height = BaseCharacterHeight;
            characterController.center = new Vector3(0, BaseCharacterHeightOffset, 0);
        }

        public void Sprint()
        {
            if (characterController.isGrounded == false) return;
            if (isCrouch) return;

            isSprint = true;
        }
        public void UnSprint()
        {
            isSprint = false;
        }

        public void Aiming()
        {
            isAiming = true;
        }
        public void UnAiming()
        {
            isAiming = false;
        }

        public float GetCurrentSpeedByState()
        {
            if (isCrouch) return crouchSpeed;

            if (isAiming)
            {
                if (isSprint) return aimingRunSpeed;
                else return aimingWalkSpeed;
            }

            if (isAiming == false)
            {
                if (isSprint) return rifleSprintSpeed;
                else return rifleRunSpeed;
            }

            return rifleRunSpeed;
        }


        private void Move()
        {
            directionControl = Vector3.MoveTowards(directionControl, TargetDirectionControl, Time.deltaTime * accelerationRate);

            if (characterController.isGrounded)
            {
                movementDirection = directionControl * GetCurrentSpeedByState();

                if (isJump == true)
                {
                    movementDirection.y = JumpSpeed;
                    isJump = false;
                }
            }

            movementDirection += Physics.gravity * Time.deltaTime;

            characterController.Move(movementDirection * Time.deltaTime);
        }

        private void UpdateDistanceToGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000))
            {
                distanceToGround = hit.distance;
            }
        }
    }
}