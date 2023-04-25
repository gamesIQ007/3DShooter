using UnityEngine;

namespace Shooter3D
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;

        [Header("Movement")]
        [SerializeField] private float RifleRunSpeed;
        [SerializeField] private float RifleSprintSpeed;
        [SerializeField] private float AimingWalkSpeed;
        [SerializeField] private float AimingRunSpeed;
        [SerializeField] private float CrouchSpeed;
        [SerializeField] private float JumpSpeed;

        [Header("State")]
        [SerializeField] private float crouhHeight;

        private bool isAiming;
        private bool isJump;
        private bool isCrouch;
        private bool isSprint;

        private float BaseCharacterHeight;
        private float BaseCharacterHeightOffset;

        public Vector3 DirectionControl;
        private Vector3 MovementDirection;


        private void Start()
        {
            // Запоминаем базовые значения капсулы
            BaseCharacterHeight = characterController.height;
            BaseCharacterHeightOffset = characterController.center.y;
        }

        private void Update()
        {
            DirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump"))
                Jump();

            // Вызов метода Crouch/UnCrouch в зависимости от ввода
            if (Input.GetButtonDown("Crouch"))
            {
                isCrouch = !isCrouch;
            }

            // Вызов метода Sprint/UnSprint в зависимости от ввода
            if (Input.GetButtonDown("Sprint"))
            {
                isSprint = !isSprint;
            }

            // Вызов метода Aiming/UnAiming в зависимости от ввода
            if (Input.GetButtonDown("Aiming"))
            {
                isAiming = !isAiming;
            }

            Move();
        }


        public void Jump()
        {
            if (characterController.isGrounded == false) return;

            isJump = true;
        }

        public void Crouch()
        {
            if (characterController.isGrounded == false) return;
            isCrouch = true;
            characterController.height = crouhHeight;
            characterController.center = new Vector3(0, characterController.center.y / 2, 0);
        }

        public void UnCrouch()
        {
            isCrouch = false;
            // установить капсулу в дефолтное состояние
            characterController.height = BaseCharacterHeight;
            characterController.center = new Vector3(0, BaseCharacterHeightOffset, 0);
        }

        // Методы для всех состояний

        public void Sprint()
        {
            if (characterController.isGrounded == false) return;
            if (isCrouch)
            {
                UnCrouch();
            }
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

        private float GetCurrentSpeedByState()
        {
            // Получение скоростей в зависимости от состояния
            if (isCrouch) return CrouchSpeed;
            if (isAiming == false && isSprint) return RifleSprintSpeed;
            if (isAiming && isSprint == false) return AimingWalkSpeed;
            if (isAiming && isSprint) return AimingRunSpeed;

            return RifleRunSpeed;
        }

        private void Move()
        {
            if (characterController.isGrounded)
            {
                MovementDirection = DirectionControl * GetCurrentSpeedByState();

                if (isJump == true)
                {
                    MovementDirection.y = JumpSpeed;
                    isJump = false;
                }
            }

            MovementDirection += Physics.gravity * Time.deltaTime;

            characterController.Move(MovementDirection * Time.deltaTime);
        }
    }
}