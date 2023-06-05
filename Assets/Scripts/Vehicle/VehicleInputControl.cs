using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Управление транспортом
    /// </summary>
    public class VehicleInputControl : MonoBehaviour
    {
        /// <summary>
        /// Транспорт
        /// </summary>
        [SerializeField] private Vehicle vehicle;

        /// <summary>
        /// Камера
        /// </summary>
        [SerializeField] private ThirdPersonCamera thirdPersonCamera;

        /// <summary>
        /// Оффсет камеры
        /// </summary>
        [SerializeField] private Vector3 cameraOffset;


        #region Unity Events

        protected virtual void Start()
        {
            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.IsRotateTarget = false;
                thirdPersonCamera.SetTargetOffset(cameraOffset);
            }
        }

        protected virtual void Update()
        {
            vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")));
            thirdPersonCamera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        #endregion


        /// <summary>
        /// Назначить камеру
        /// </summary>
        /// <param name="camera">Камера</param>
        public void AssignCamera(ThirdPersonCamera camera)
        {
            thirdPersonCamera = camera;
            thirdPersonCamera.IsRotateTarget = false;
            thirdPersonCamera.SetTargetOffset(cameraOffset);
            thirdPersonCamera.SetTarget(vehicle.transform);
        }
    }
}