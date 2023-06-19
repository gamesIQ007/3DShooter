using System.Collections.Generic;
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
        /// Смотрящий на коллайдеры
        /// </summary>
        [SerializeField] private ColliderViewer colliderViewer;

        /// <summary>
        /// Контролируемый дрон
        /// </summary>
        private Drone drone;

        /// <summary>
        /// Область перемещения
        /// </summary>
        private CubeArea movementArea;

        /// <summary>
        /// Позиция, в которую дрон перемещается
        /// </summary>
        private Vector3 movementPosition;
        /// <summary>
        /// Цель стрельбы
        /// </summary>
        private Transform shootTarget;


        #region Unity Events

        private void Start()
        {
            drone = GetComponent<Drone>();
            drone.EventOnDeath.AddListener(OnDroneDeath);

            FindMovementArea();

            drone.OnGetDamage += OnGetDamage;
        }

        private void Update()
        {
            UpdateAI();
        }

        private void OnDestroy()
        {
            drone.EventOnDeath.RemoveListener(OnDroneDeath);
            drone.OnGetDamage -= OnGetDamage;
        }

        #endregion


        #region Handlers

        /// <summary>
        /// Действие при получении урона
        /// </summary>
        /// <param name="other">Кто наносит урон</param>
        private void OnGetDamage(Destructible other)
        {
            ActionAssignTargetAllTeamMembers(other.transform);
        }

        #endregion


        /// <summary>
        /// Обновление ИИ
        /// </summary>
        private void UpdateAI()
        {
            ActionFindShootingTarget();

            ActionMove();

            ActionFire();
        }


        #region Actions

        /// <summary>
        /// Действие передвижения
        /// </summary>
        private void ActionMove()
        {
            if (transform.position == movementPosition)
            {
                movementPosition = movementArea.GetRandomInsideZone();
            }

            if (Physics.Linecast(transform.position, movementPosition))
            {
                movementPosition = movementArea.GetRandomInsideZone();
            }

            drone.MoveTo(movementPosition);

            if (shootTarget != null)
            {
                drone.LookAt(shootTarget.position);
            }
            else
            {
                drone.LookAt(movementPosition);
            }
        }

        /// <summary>
        /// Действие нахождения цели для стрельбы
        /// </summary>
        private void ActionFindShootingTarget()
        {
            Transform potencionalTarget = FindShootTarget();

            if (potencionalTarget != null)
            {
                ActionAssignTargetAllTeamMembers(potencionalTarget);
            }
        }

        /// <summary>
        /// Действие стрельбы
        /// </summary>
        private void ActionFire()
        {
            if (shootTarget != null)
            {
                drone.Fire(shootTarget.position);
            }
        }

        /// <summary>
        /// Задать цель для всех членов команды
        /// </summary>
        /// <param name="other">Цель</param>
        private void ActionAssignTargetAllTeamMembers(Transform other)
        {
            List<Destructible> team = Destructible.GetAllTeamMembers(drone.TeamId);

            foreach (Destructible dest in team)
            {
                AIDrone ai = dest.transform.root.GetComponent<AIDrone>();

                if (ai != null && ai.enabled == true)
                {
                    ai.SetShootTarget(other);
                }
            }
        }

        #endregion


        /// <summary>
        /// Событие при смерти дрона
        /// </summary>
        private void OnDroneDeath()
        {
            enabled = false;
        }
        
        /// <summary>
        /// Задать цель стрельбы
        /// </summary>
        /// <param name="target">Цель</param>
        public void SetShootTarget(Transform target)
        {
            shootTarget = target;
        }

        /// <summary>
        /// Найти цель стрельбы
        /// </summary>
        /// <returns>Цель стрельбы</returns>
        private Transform FindShootTarget()
        {
            List<Destructible> targets = Destructible.GetAllNonTeamMembers(drone.TeamId);

            for (int i = 0; i < targets.Count; i++)
            {
                if (colliderViewer.IsObjectVisible(targets[i].gameObject))
                {
                    return targets[i].transform;
                }
            }

            return null;
        }

        /// <summary>
        /// Найти область перемещения
        /// </summary>
        private void FindMovementArea()
        {
            if (movementArea == null)
            {
                CubeArea[] cubeAreas = FindObjectsOfType<CubeArea>();

                float minDistance = float.MaxValue;

                for (int i = 0; i < cubeAreas.Length; i++)
                {
                    if (Vector3.Distance(transform.position, cubeAreas[i].transform.position) < minDistance)
                    {
                        minDistance = Vector3.Distance(transform.position, cubeAreas[i].transform.position);
                        movementArea = cubeAreas[i];
                    }
                }
            }
        }
    }
}