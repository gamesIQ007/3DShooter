using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Конвертёр вращения камеры
    /// </summary>
    public class ConverterCameraRotation : MonoBehaviour
    {
        /// <summary>
        /// Камера
        /// </summary>
        [SerializeField] private new Transform camera;
        /// <summary>
        /// Позиция, куда направлена камера
        /// </summary>
        [SerializeField] private Transform cameraLook;
        /// <summary>
        /// Смещение
        /// </summary>
        [SerializeField] private Vector3 lookOffset;

        /// <summary>
        /// Верхний ограничитель угла обзора
        /// </summary>
        [SerializeField] private float topAngleLimit;
        /// <summary>
        /// Нижний ограничитель угла обзора
        /// </summary>
        [SerializeField] private float bottomAngleLimit;


        private void Update()
        {
            Vector3 angles = new Vector3(0, 0, 0);

            angles.z = camera.eulerAngles.x;

            if (angles.z >= topAngleLimit || angles.z <= bottomAngleLimit)
            {
                transform.LookAt(cameraLook.position + lookOffset);

                angles.x = transform.eulerAngles.x;
                angles.y = transform.eulerAngles.y;

                transform.eulerAngles = angles;
            }
        }
    }
}