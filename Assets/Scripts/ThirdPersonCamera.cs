using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Камера от третьего лица
    /// </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        /// <summary>
        /// Цель слежения камеры
        /// </summary>
        [SerializeField] private Transform target;

        /// <summary>
        /// Смещение камеры
        /// </summary>
        [SerializeField] private Vector3 offset;
        /// <summary>
        /// Интенсивность изменения смещения
        /// </summary>
        [SerializeField] private float changeOffsetRate;
        /// <summary>
        /// Интенсивность интерполяции вращения
        /// </summary>
        [SerializeField] private float rotateTargetLerpRate;

        [Header("Distance")]
        /// <summary>
        /// Расстояние до цели
        /// </summary>
        [SerializeField] private float distance;
        /// <summary>
        /// Минимальное расстояние до цели
        /// </summary>
        [SerializeField] private float minDistance;
        /// <summary>
        /// Интенсивность интерполяции дистанции
        /// </summary>
        [SerializeField] private float distanceLerpRate;
        /// <summary>
        /// Смещение расстояния от коллизии объекта с камерой
        /// </summary>
        [SerializeField] private float distanceOffsetFromCollisionHit;

        /// <summary>
        /// Чувствительность вращения
        /// </summary>
        [SerializeField] private float sensitive;

        [Header("Rotation Limit")]
        /// <summary>
        /// Максимальный предел наклона по оси Y
        /// </summary>
        [SerializeField] private float maxLimitY;
        /// <summary>
        /// Минимальный предел наклона по оси Y
        /// </summary>
        [SerializeField] private float minLimitY;

        /// <summary>
        /// Вращать ли камеру
        /// </summary>
        [HideInInspector] public bool IsRotateTarget;
        /// <summary>
        /// Вектор внешнего управления вращением
        /// </summary>
        [HideInInspector] public Vector2 RotationControl;

        /// <summary>
        /// На сколько передвинута мышь по оси X
        /// </summary>
        private float deltaRotationX;
        /// <summary>
        /// На сколько передвинута мышь по оси Y
        /// </summary>
        private float deltaRotationY;

        /// <summary>
        /// Текущее расстояние до цели
        /// </summary>
        private float currentDistance;

        /// <summary>
        /// Смещение от цели
        /// </summary>
        private Vector3 targetOffset;
        /// <summary>
        /// Смещение от цели по умолчанию
        /// </summary>
        private Vector3 defaultOffset;


        #region Unity Events

        private void Start()
        {
            targetOffset = offset;
            defaultOffset = offset;
        }

        private void Update()
        {
            // Расчёт вращения и позиции
            deltaRotationX += RotationControl.x * sensitive;
            deltaRotationY += RotationControl.y * sensitive;
            deltaRotationY = ClampAngle(deltaRotationY, minLimitY, maxLimitY);

            offset = Vector3.MoveTowards(offset, targetOffset, Time.deltaTime * changeOffsetRate);

            Quaternion finalRotation = Quaternion.Euler(deltaRotationY, deltaRotationX, 0);
            Vector3 finalPosition = target.position - (finalRotation * Vector3.forward * distance);
            finalPosition = AddLocalOffset(finalPosition);

            // Расчёт дистанции
            float targetDistance = distance;

            RaycastHit hit;
            if (Physics.Linecast(target.position + new Vector3(0, offset.y, 0), finalPosition, out hit))
            {
                float distanceToHit = Vector3.Distance(target.position + new Vector3(0, offset.y, 0), hit.point);

                if (hit.transform != target)
                {
                    if (distanceToHit < distance)
                    {
                        targetDistance = distanceToHit - distanceOffsetFromCollisionHit;
                    }
                }
            }

            currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, Time.deltaTime * distanceLerpRate);
            currentDistance = Mathf.Clamp(currentDistance, minDistance, distance);

            // Корректировка позиции камеры
            finalPosition = target.position - (finalRotation * Vector3.forward * currentDistance);

            //Применение трансформы
            transform.rotation = finalRotation;
            transform.position = finalPosition;
            transform.position = AddLocalOffset(transform.position);

            // Вращаем цель
            if (IsRotateTarget)
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
                target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, Time.deltaTime * rotateTargetLerpRate);
            }
        }

        #endregion


        #region Public API

        /// <summary>
        /// Задать смещение до цели
        /// </summary>
        /// <param name="offset">Значение смещения</param>
        public void SetTargetOffset(Vector3 offset)
        {
            targetOffset = offset;
        }

        /// <summary>
        /// Задать смещение до цели по умолчанию
        /// </summary>
        public void SetDefaultOffset()
        {
            targetOffset = defaultOffset;
        }

        /// <summary>
        /// Задать цель
        /// </summary>
        /// <param name="target">Цель</param>
        public void SetTarget(Transform targetTransform)
        {
            target = targetTransform;
        }

        #endregion


        /// <summary>
        /// Ограничение угла поворота камеры
        /// </summary>
        /// <param name="angle">Угол</param>
        /// <param name="min">Минимум</param>
        /// <param name="max">Максимум</param>
        /// <returns>Итоговый угол поворота камеры</returns>
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

        /// <summary>
        /// Добавление локального смещения
        /// </summary>
        /// <param name="position">Позиция смещения</param>
        /// <returns>Итоговая позиция смещения</returns>
        private Vector3 AddLocalOffset(Vector3 position)
        {
            Vector3 result = position;
            result.y += offset.y;
            result += transform.right * offset.x;
            result += transform.forward * offset.z;
            return result;
        }
    }
}