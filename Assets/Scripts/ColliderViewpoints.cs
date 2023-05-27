using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Точки, на которые смотрят, когда хотят определить, виден ли объект
    /// </summary>
    public class ColliderViewpoints : MonoBehaviour
    {
        /// <summary>
        /// Перечисление типов коллайдеров
        /// </summary>
        private enum ColliderType
        {
            /// <summary>
            /// Персонаж
            /// </summary>
            Character
        }

        /// <summary>
        /// Тип коллайдеров
        /// </summary>
        [SerializeField] private ColliderType colliderType;

        /// <summary>
        /// Коллайдер
        /// </summary>
        [SerializeField] private new Collider collider;

        /// <summary>
        /// Массив точек
        /// </summary>
        private Vector3[] points;


        #region Unity Events

        private void Start()
        {
            if (colliderType == ColliderType.Character)
            {
                UpdatePointsForCharacterController();
            }
        }

        private void Update()
        {
            if (colliderType == ColliderType.Character)
            {
                CalcPointsForCharacterController(collider as CharacterController);
            }
        }

        #endregion


        /// <summary>
        /// Видно ли из точки
        /// </summary>
        /// <param name="point">Точка обзора</param>
        /// <param name="eyeDir">Направление взгляда</param>
        /// <param name="viewAngle">Угол обзора</param>
        /// <param name="viewDistance">Дистанция обзора</param>
        /// <returns>Видно ли из точки</returns>
        public bool IsVisibleFromPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
        {
            for (int i = 0; i < points.Length; i++)
            {
                float angle = Vector3.Angle(points[i] - point, eyeDir);
                float dist = Vector3.Distance(points[i], point);

                if (angle < viewAngle * 0.5f && dist < viewDistance)
                {
                    RaycastHit hit;

                    Debug.DrawLine(point, points[i], Color.blue);
                    if (Physics.Raycast(point, (points[i] - point).normalized, out hit, viewDistance * 2))
                    {
                        if (hit.collider == collider)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Обновить точки
        /// </summary>
        [ContextMenu("Update View Points")]
        private void UpdateViewPoints()
        {
            if (collider == null) return;

            points = null;

            if (colliderType == ColliderType.Character)
            {
                UpdatePointsForCharacterController();
            }
        }

        /// <summary>
        /// Обновить точки для контроллера персонажа
        /// </summary>
        private void UpdatePointsForCharacterController()
        {
            if (points == null)
            {
                points = new Vector3[4];
            }

            CharacterController col = collider as CharacterController;

            CalcPointsForCharacterController(col);
        }

        /// <summary>
        /// Пересчитать точки для контроллера персонажа
        /// </summary>
        /// <param name="col">Коллайдер персонажа</param>
        private void CalcPointsForCharacterController(CharacterController col)
        {
            points[0] = col.transform.position + col.center + col.transform.up * col.height * 0.3f;
            points[1] = col.transform.position + col.center - col.transform.up * col.height * 0.3f;
            points[2] = col.transform.position + col.center + col.transform.right * col.radius * 0.4f;
            points[3] = col.transform.position + col.center - col.transform.right * col.radius * 0.4f;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (points == null) return;

            Gizmos.color = Color.blue;
            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawSphere(points[i], 0.1f);
            }
        }
#endif
    }
}