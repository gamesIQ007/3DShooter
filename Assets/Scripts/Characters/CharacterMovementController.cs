﻿using UnityEngine;

namespace Shooter3D
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement targetCharacterMovement;
        [SerializeField] private ThirdPersonCamera targetCamera;

        [SerializeField] private Vector3 aimingOffset;


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            targetCharacterMovement.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            targetCamera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

            if (targetCharacterMovement.TargetDirectionControl != Vector3.zero || targetCharacterMovement.IsAiming)
            {
                targetCamera.IsRotateTarget = true;
            }
            else
            {
                targetCamera.IsRotateTarget = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                targetCharacterMovement.Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                targetCharacterMovement.Crouch();
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                targetCharacterMovement.UnCrouch();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                targetCharacterMovement.Sprint();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                targetCharacterMovement.UnSprint();
            }

            if (Input.GetMouseButtonDown(1))
            {
                targetCharacterMovement.Aiming();
                targetCamera.SetTargetOffset(aimingOffset);
            }
            if (Input.GetMouseButtonUp(1))
            {
                targetCharacterMovement.UnAiming();
                targetCamera.SetDefaultOffset();
            }
        }
    }
}