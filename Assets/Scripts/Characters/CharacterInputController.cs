using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Управление вводом для персонажа
    /// </summary>
    public class CharacterInputController : MonoBehaviour
    {
        /// <summary>
        /// Класс перемещения персонажа
        /// </summary>
        [SerializeField] private CharacterMovement targetCharacterMovement;
        /// <summary>
        /// Камера
        /// </summary>
        [SerializeField] private ThirdPersonCamera targetCamera;
        /// <summary>
        /// Стрелок
        /// </summary>
        [SerializeField] private PlayerShooter targetShooter;

        /// <summary>
        /// Риг разброса при стрельбе
        /// </summary>
        [SerializeField] private SpreadShootRig spreadShootRig;


        /// <summary>
        /// Смещение прицеливания
        /// </summary>
        [SerializeField] private Vector3 aimingOffset;

        /// <summary>
        /// Базовое смещение камеры
        /// </summary>
        [SerializeField] private Vector3 defaultOffset;


        #region Unity Events

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

            if (Input.GetMouseButton(0))
            {
                if (targetCharacterMovement.IsAiming)
                {
                    targetShooter.Shoot();
                    spreadShootRig.Spread();
                }
            }
        }

        #endregion


        /// <summary>
        /// Назначить камеру
        /// </summary>
        /// <param name="camera">Камера</param>
        public void AssignCamera(ThirdPersonCamera camera)
        {
            targetCamera = camera;
            targetCamera.IsRotateTarget = false;
            targetCamera.SetTargetOffset(defaultOffset);
            targetCamera.SetTarget(targetCharacterMovement.transform);
        }
    }
}