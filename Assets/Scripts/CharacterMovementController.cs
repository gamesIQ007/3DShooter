using UnityEngine;

namespace Shooter3D
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] CharacterMovement targetCharacterController;


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            targetCharacterController.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump"))
            {
                targetCharacterController.Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                targetCharacterController.Crouch();
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                targetCharacterController.UnCrouch();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                targetCharacterController.Sprint();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                targetCharacterController.UnSprint();
            }

            if (Input.GetMouseButtonDown(1))
            {
                targetCharacterController.Aiming();
            }
            if (Input.GetMouseButtonUp(1))
            {
                targetCharacterController.UnAiming();
            }
        }
    }
}