using UnityEngine;

namespace Shooter3D
{
    public class CharacterAnimationState : MonoBehaviour
    {
        private const float INPUT_CONTROL_LERP_RATE = 10.0f;

        [SerializeField] private CharacterController targetCharacterController;
        [SerializeField] private CharacterMovement targetCharacterMovement;
        [SerializeField] private Animator targetAnimator;

        private Vector3 inputControl;


        private void Update()
        {
            Vector3 movementSpeed = transform.InverseTransformDirection(targetCharacterController.velocity);

            // Чтобы не было дёрганья модели при движении камеры по горизонтали
            inputControl = Vector3.MoveTowards(inputControl, targetCharacterMovement.TargetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

            targetAnimator.SetFloat("Normalize Movement X", inputControl.x);
            targetAnimator.SetFloat("Normalize Movement Z", inputControl.z);

            targetAnimator.SetBool("Is Ground", targetCharacterMovement.IsGrounded);
            targetAnimator.SetBool("Is Croaching", targetCharacterMovement.IsCrouch);
            targetAnimator.SetBool("Is Aiming", targetCharacterMovement.IsAiming);
            targetAnimator.SetBool("Is Sprint", targetCharacterMovement.IsSprint);

            if (targetCharacterMovement.IsGrounded == false)
            {
                targetAnimator.SetFloat("Jump", movementSpeed.y);
            }

            targetAnimator.SetFloat("Distance To Ground", targetCharacterMovement.DistanceToGround);

            Vector3 groundSpeed = movementSpeed;
            groundSpeed.y = 0;
            targetAnimator.SetFloat("Ground Speed", groundSpeed.magnitude);
        }
    }
}