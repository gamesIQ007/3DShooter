using UnityEngine;

namespace Shooter3D
{
    public class CharacterAnimationState : MonoBehaviour
    {
        [SerializeField] private CharacterController targetCharacterController;
        [SerializeField] private CharacterMovement targetCharacterMovement;
        [SerializeField] private Animator targetAnimator;


        private void Update()
        {
            Vector3 movementSpeed = targetCharacterController.velocity;

            targetAnimator.SetFloat("Normalize Movement X", movementSpeed.x / targetCharacterMovement.GetCurrentSpeedByState());
            targetAnimator.SetFloat("Normalize Movement Z", movementSpeed.z / targetCharacterMovement.GetCurrentSpeedByState());

            targetAnimator.SetBool("Is Ground", targetCharacterController.isGrounded);
            targetAnimator.SetBool("Is Croaching", targetCharacterMovement.IsCrouch);
            targetAnimator.SetBool("Is Aiming", targetCharacterMovement.IsAiming);
            targetAnimator.SetBool("Is Sprint", targetCharacterMovement.IsSprint);

            if (targetCharacterController.isGrounded == false)
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