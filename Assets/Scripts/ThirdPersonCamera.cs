using UnityEngine;

namespace Shooter3D
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private Vector3 offset;

        [SerializeField] private float distance;

        [SerializeField] private float sensitive;

        [Header("Rotation Limit")]
        [SerializeField] private float maxLimitY;
        [SerializeField] private float minLimitY;

        /// <summary>
        /// Вращать ли камеру
        /// </summary>
        [HideInInspector] public bool IsRotateTarget;
        [HideInInspector] public Vector2 RotationControl;

        /// <summary>
        /// На сколько передвинута мышь по осям
        /// </summary>
        private float deltaRotationX;
        private float deltaRotationY;

        private Vector3 targetOffset;
        private Vector3 defaultOffset;


        private void Start()
        {
            targetOffset = offset;
            defaultOffset = offset;
        }

        private void Update()
        {
            deltaRotationX += RotationControl.x * sensitive;
            deltaRotationY += RotationControl.y * sensitive;
            deltaRotationY = ClampAngle(deltaRotationY, minLimitY, maxLimitY);

            Quaternion finalRotation = Quaternion.Euler(deltaRotationY, deltaRotationX, 0);
            Vector3 finalPosition = target.position - (finalRotation * Vector3.forward * distance);

            finalPosition = AddLocalOffset(finalPosition);
            
            transform.position = finalPosition;
            transform.rotation = finalRotation;

            if (IsRotateTarget)
            {
                target.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }


        /// <summary>
        /// Ограничение угла поворота
        /// </summary>
        /// <param name="angle">Угол</param>
        /// <param name="min">Минимум</param>
        /// <param name="max">Максимум</param>
        /// <returns></returns>
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
            return Mathf.Clamp(angle, min, max);
        }

        private Vector3 AddLocalOffset(Vector3 position)
        {
            Vector3 result = position;
            result.y += offset.y;
            result += transform.right * offset.x;
            result += transform.forward * offset.z;
            return result;
        }


        public void SetTargetOffset(Vector3 offset)
        {
            targetOffset = offset;
            this.offset = offset;
        }
        public void SetDefaultOffset()
        {
            targetOffset = defaultOffset;
            offset = defaultOffset;
        }
    }
}