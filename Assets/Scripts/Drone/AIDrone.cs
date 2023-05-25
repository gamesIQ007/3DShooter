using UnityEngine;

namespace Shooter3D
{
    [RequireComponent(typeof(Drone))]

    /// <summary>
    /// ИИ дрона
    /// </summary>
    public class AIDrone : MonoBehaviour
    {
        /// <summary>
        /// Область перемещения
        /// </summary>
        [SerializeField] private CubeArea movementArea;

        /// <summary>
        /// Расстояние обнаружения противника
        /// </summary>
        [SerializeField] private float angryDistance;

        /// <summary>
        /// Контролируемый дрон
        /// </summary>
        private Drone drone;

        /// <summary>
        /// Позиция, в которую дрон перемещается
        /// </summary>
        private Vector3 movementPosition;
        /// <summary>
        /// Цель стрельбы
        /// </summary>
        private Transform shootTarget;

        /// <summary>
        /// Игрок
        /// </summary>
        private Transform player;


        #region Unity Events

        private void Start()
        {
            drone = GetComponent<Drone>();
            drone.EventOnDeath.AddListener(OnDroneDeath);

            player = GameObject.FindGameObjectWithTag("Player").transform;

            if (movementArea == null)
            {
                movementArea = FindObjectOfType<CubeArea>();
            }
        }

        private void Update()
        {
            UpdateAI();
        }

        private void OnDestroy()
        {
            drone.EventOnDeath.RemoveListener(OnDroneDeath);
        }

        #endregion


        private void OnDroneDeath()
        {
            enabled = false;
        }


        /// <summary>
        /// Обновление ИИ
        /// </summary>
        private void UpdateAI()
        {
            
            // Обновление позиции, в которую дрон перемещается
            if (transform.position == movementPosition)
            {
                movementPosition = movementArea.GetRandomInsideZone();
            }

            if (Physics.Linecast(transform.position, movementPosition))
            {
                movementPosition = movementArea.GetRandomInsideZone();
            }

            // Поиск цели
            if (Vector3.Distance(transform.position, player.position) <= angryDistance)
            {
                shootTarget = player;
            }

            // Перемещение
            drone.MoveTo(movementPosition);

            if (shootTarget != null)
            {
                drone.LookAt(shootTarget.position);
            }
            else
            {
                drone.LookAt(movementPosition);
            }

            // Стрельба
            if (shootTarget != null)
            {
                drone.Fire(shootTarget.position);
            }
        }


#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, angryDistance);
        }

#endif
    }
}