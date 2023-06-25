using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter3D
{
    /// <summary>
    /// ИИ солдата пришельцев
    /// </summary>
    public class AIAlienSoldier : MonoBehaviour
    {
        /// <summary>
        /// Перечисление типов поведения
        /// </summary>
        public enum AIBehaviour
        {
            /// <summary>
            /// Ничего
            /// </summary>
            Null,
            /// <summary>
            /// Ожидание
            /// </summary>
            Idle,
            /// <summary>
            /// Патрулирование по случайным точкам
            /// </summary>
            PatrolRandom,
            /// <summary>
            /// Патрулирование по маршруту
            /// </summary>
            CirclePatrol,
            /// <summary>
            /// Преследование цели
            /// </summary>
            PursuitTarget,
            /// <summary>
            /// Поиск цели
            /// </summary>
            SeekTarget,
            /// <summary>
            /// Поиск цели
            /// </summary>
            SeekTargetInArea
        }

        /// <summary>
        /// Поведение
        /// </summary>
        [SerializeField] private AIBehaviour aIBehaviour;
        public AIBehaviour AI_Behaviour => aIBehaviour;
        public AIBehaviour Behaviour { get { return aIBehaviour; } set { aIBehaviour = value; } }

        /// <summary>
        /// Солдат пришельцев
        /// </summary>
        [SerializeField] private AlienSoldier alienSoldier;

        /// <summary>
        /// Перемещение персонажа
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;

        /// <summary>
        /// Расстояние для атаки
        /// </summary>
        [SerializeField] private float aimingDistance;

        /// <summary>
        /// Расстояние обнаружения
        /// </summary>
        [SerializeField] private float detectedDistance;

        /// <summary>
        /// Агент перемещения
        /// </summary>
        [SerializeField] private NavMeshAgent agent;

        /// <summary>
        /// Индекс точки маршрута патрулирования
        /// </summary>
        [SerializeField] private int patrolPathNodeIndex = 0;

        /// <summary>
        /// Смотрящий на коллайдеры
        /// </summary>
        [SerializeField] private ColliderViewer colliderViewer;
		
		/// <summary>
        /// Смотрящий на коллайдеры, боковое зрение
        /// </summary>
        [SerializeField] private ColliderViewer colliderViewerSideView;
		
		/// <summary>
        /// Время нахождения в боковом зрении, прежде чем заметят
        /// </summary>
        [SerializeField] private float sideViewTime;
		
		/// <summary>
		/// Радиус, в котором будут вестись поиски цели
		/// </summary>
		[SerializeField] private float seekTargetRadius;
		
		/// <summary>
		/// Время поиска цели
		/// </summary>
		[SerializeField] private float seekTargetTime;

        /// <summary>
        /// Путь перемещения
        /// </summary>
        private NavMeshPath navMeshPath;
        /// <summary>
        /// Текущая точка перемещения
        /// </summary>
        private PatrolPathNode currentPathNode;

        /// <summary>
        /// Маршрут патрулирования
        /// </summary>
        private PatrolPath patrolPath;

        /// <summary>
        /// Потенциальная цель
        /// </summary>
        private GameObject potencialTarget;
        /// <summary>
        /// Цель преследования
        /// </summary>
        private Transform pursuitTarget;
        /// <summary>
        /// Цель поиска
        /// </summary>
        private Vector3 seekTarget;
        /// <summary>
        /// Цель поиска в области
        /// </summary>
        private Vector3 seekTargetArea;
		/// <summary>
        /// Таймер поиска цели
        /// </summary>
		private float seekTargetTimer;
		/// <summary>
        /// Идёт поиск цели
        /// </summary>
		private bool seekingTarget = false;
		/// <summary>
        /// Таймер бокового зрения
        /// </summary>
		private float sideViewTimer;
		/// <summary>
        /// Цель в боковом зрении
        /// </summary>
		private bool targetInSideView = false;
        public bool TargetInSideView => targetInSideView;
        /// <summary>
        /// Отправлено уведомление об обнаружении
        /// </summary>
        private bool isPlayerDetected;


        #region Unity Events

        private void Start()
        {
            FindMovementArea();

            characterMovement.UpdatePosition = false;
            navMeshPath = new NavMeshPath();
            StartBehaviour(aIBehaviour);

            alienSoldier.EventOnDeath.AddListener(OnDeath);

            alienSoldier.OnGetDamage += OnGetDamage;
        }

        private void Update()
        {
            SyncAgentAndCharacterMovement();
            UpdateAI();
			seekTargetTimer -= Time.deltaTime;
			sideViewTimer -= Time.deltaTime;
        }

        private void OnDestroy()
        {
            alienSoldier.EventOnDeath.RemoveListener(OnDeath);
            alienSoldier.OnGetDamage -= OnGetDamage;
        }

        #endregion


        #region Handlers

        /// <summary>
        /// Действие при получении урона
        /// </summary>
        /// <param name="other">Кто наносит урон</param>
        private void OnGetDamage(Destructible other)
        {
            if (other.TeamId != alienSoldier.TeamId)
            {
                ActionAssignTargetAllTeamMembers(other.transform);
            }
        }

        /// <summary>
        /// При смерти
        /// </summary>
        private void OnDeath()
        {
            SendPlayerStopPursuit();
        }

        /// <summary>
        /// Когда слышит
        /// </summary>
        public void OnHeard()
        {
            //StartBehaviour(AIBehaviour.PursuitTarget);

            if (alienSoldier.HitPoints <= 0) return;

            SendPlayerStartPursuit();

            pursuitTarget = potencialTarget.transform;
            ActionAssignTargetAllTeamMembers(pursuitTarget);
        }

        #endregion


        #region AI

        /// <summary>
        /// Обновление ИИ
        /// </summary>
        private void UpdateAI()
        {
            ActionUpdateTarget();

            if (aIBehaviour == AIBehaviour.Idle)
            {
                return;
            }

            if (aIBehaviour == AIBehaviour.PursuitTarget)
            {
                agent.CalculatePath(pursuitTarget.position, navMeshPath);
                agent.SetPath(navMeshPath);

                if (Vector3.Distance(transform.position, pursuitTarget.position) <= aimingDistance)
                {
                    characterMovement.Aiming();

                    if (Vector3.Angle(pursuitTarget.position - transform.position, transform.forward) < 10)
                    {
                        agent.isStopped = true;
                    }
                    alienSoldier.Fire(pursuitTarget.position + new Vector3(0, 1, 0));
                }
                else
                {
                    characterMovement.UnAiming();
                }
            }

            if (aIBehaviour == AIBehaviour.SeekTarget)
            {
                SendPlayerStartPursuit();

                agent.CalculatePath(seekTarget, navMeshPath);
                agent.SetPath(navMeshPath);

                if (AgentReachedDestination())
                {
					StartBehaviour(AIBehaviour.SeekTargetInArea);
                }
            }

            if (aIBehaviour == AIBehaviour.SeekTargetInArea)
            {
				if (seekingTarget == false)
				{
					seekTargetTimer = seekTargetTime;
					seekingTarget = true;
					seekTargetArea = seekTarget + Random.insideUnitSphere * seekTargetRadius;
				}
				
				agent.CalculatePath(seekTargetArea, navMeshPath);
                agent.SetPath(navMeshPath);
				
				if (AgentReachedDestination() && seekingTarget)
                {
					if (seekTargetTimer > 0)
					{
						seekTargetArea = seekTarget + Random.insideUnitSphere * seekTargetRadius;
						agent.CalculatePath(seekTargetArea, navMeshPath);
						agent.SetPath(navMeshPath);
					}

					if (seekTargetTimer <= 0)
					{
						seekingTarget = false;
						StartBehaviour(AIBehaviour.PatrolRandom);
					}
                }
            }

            if (aIBehaviour == AIBehaviour.PatrolRandom)
            {
                SendPlayerStopPursuit();

                if (AgentReachedDestination())
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }

            if (aIBehaviour == AIBehaviour.CirclePatrol)
            {
                SendPlayerStopPursuit();

                if (AgentReachedDestination())
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }
        }

        #endregion


        #region Actions

        /// <summary>
        /// Найти потенциальную цель
        /// </summary>
        private void FindPotencialTarget()
        {
            potencialTarget = Player.Instance.gameObject;
        }


        /// <summary>
        /// Действие обновления цели
        /// </summary>
        private void ActionUpdateTarget()
        {
            if (potencialTarget == null) 
            {
                FindPotencialTarget();

                if (potencialTarget == null) return;
            }

            if (colliderViewer.IsObjectVisible(potencialTarget) || Vector3.Distance(transform.position, potencialTarget.transform.position) <= detectedDistance)
            {
                SendPlayerStartPursuit();

                pursuitTarget = potencialTarget.transform;
                ActionAssignTargetAllTeamMembers(pursuitTarget);
            }
            else
            {
                if (pursuitTarget != null)
                {
                    seekTarget = pursuitTarget.position;
                    pursuitTarget = null;
                    StartBehaviour(AIBehaviour.SeekTarget);
                }
            }
            
			if (colliderViewerSideView.IsObjectVisible(potencialTarget) && colliderViewer.IsObjectVisible(potencialTarget) == false)
			{
				if (targetInSideView == false)
				{
					sideViewTimer = sideViewTime;
					targetInSideView = true;
				}
				if (targetInSideView && sideViewTimer <= 0)
				{
					targetInSideView = false;
					pursuitTarget = potencialTarget.transform;
					ActionAssignTargetAllTeamMembers(pursuitTarget);
				}
			}
            else
            {
                targetInSideView = false;
            }
        }

        /// <summary>
        /// Задать цель для всех членов команды
        /// </summary>
        /// <param name="other">Цель</param>
        private void ActionAssignTargetAllTeamMembers(Transform other)
        {
            List<Destructible> team = Destructible.GetAllTeamMembers(alienSoldier.TeamId);

            foreach (Destructible dest in team)
            {
                AIAlienSoldier ai = dest.transform.root.GetComponent<AIAlienSoldier>();

                if (ai != null && ai.enabled == true)
                {
                    ai.SetPursuitTarget(other);
                    ai.StartBehaviour(AIBehaviour.PursuitTarget);
                }
            }
        }

        #endregion


        #region Behaviour

        /// <summary>
        /// Начать поведение
        /// </summary>
        /// <param name="behaviour">Поведение</param>
        private void StartBehaviour(AIBehaviour state)
        {
            if (alienSoldier.IsDead) return;

            if (state == AIBehaviour.Idle)
            {
                agent.isStopped = true;
                characterMovement.UnAiming();
            }

            if (state == AIBehaviour.PatrolRandom)
            {
                agent.isStopped = false;
                SetDestinationByPathNode(patrolPath.GetRandomPathNode());
                characterMovement.UnAiming();
            }

            if (state == AIBehaviour.CirclePatrol)
            {
                agent.isStopped = false;
                SetDestinationByPathNode(patrolPath.GetNextNode(ref patrolPathNodeIndex));
                characterMovement.UnAiming();
            }

            if (state == AIBehaviour.PursuitTarget)
            {
                agent.isStopped = false;
            }

            if (state == AIBehaviour.SeekTarget)
            {
                agent.isStopped = false;
                characterMovement.UnAiming();
            }

            if (state == AIBehaviour.SeekTargetInArea)
            {
                agent.isStopped = false;
                characterMovement.UnAiming();
            }

            aIBehaviour = state;
        }

        /// <summary>
        /// Временно сменить поведение
        /// </summary>
        /// <param name="state">Поведение</param>
        /// <param name="second">Время</param>
        /// <returns></returns>
        IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
        {
            AIBehaviour previous = aIBehaviour;
            aIBehaviour = state;
            StartBehaviour(aIBehaviour);

            yield return new WaitForSeconds(second);

            StartBehaviour(previous);
        }

        #endregion


        /// <summary>
        /// Задать позицию
        /// </summary>
        /// <param name="pos">Позиция</param>
        public void SetPosition(Vector3 pos)
        {
            agent.Warp(pos);
        }

        /// <summary>
        /// Задать цель преследования
        /// </summary>
        /// <param name="target">Цель</param>
        public void SetPursuitTarget(Transform target)
        {
            pursuitTarget = target;
        }


        /// <summary>
        /// Задать точку назначения из точки маршрута
        /// </summary>
        /// <param name="node">Точка маршрута</param>
        private void SetDestinationByPathNode(PatrolPathNode node)
        {
            currentPathNode = node;
            agent.CalculatePath(currentPathNode.transform.position, navMeshPath);
            agent.SetPath(navMeshPath);
        }

        /// <summary>
        /// Синхронизация агента и движения персонажа
        /// </summary>
        private void SyncAgentAndCharacterMovement()
        {
            agent.speed = characterMovement.CurrentSpeed;

            float factor = agent.velocity.magnitude / agent.speed;
            characterMovement.TargetDirectionControl = transform.InverseTransformDirection(agent.velocity.normalized) * factor;
        }

        /// <summary>
        /// Агент достиг точки назначения
        /// </summary>
        /// <returns>Агент достиг точки назначения</returns>
        private bool AgentReachedDestination()
        {
            if (agent.pathPending == false)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (agent.hasPath == false || agent.velocity.sqrMagnitude == 0.0f)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Отправить уведомление о начале преследования
        /// </summary>
        private void SendPlayerStartPursuit()
        {
            if (isPlayerDetected == false)
            {
                Player.Instance.StartPursuit();
                isPlayerDetected = true;
            }
        }

        /// <summary>
        /// Отправить уведомление об окончании преследования
        /// </summary>
        private void SendPlayerStopPursuit()
        {
            if (isPlayerDetected)
            {
                Player.Instance.StopPursuit();
                isPlayerDetected = false;
            }
        }

        /// <summary>
        /// Найти маршрут перемещения
        /// </summary>
        private void FindMovementArea()
        {
            if (patrolPath == null)
            {
                PatrolPath[] patrolPaths = FindObjectsOfType<PatrolPath>();

                float minDistance = float.MaxValue;

                for (int i = 0; i < patrolPaths.Length; i++)
                {
                    if (Vector3.Distance(transform.position, patrolPaths[i].transform.position) < minDistance)
                    {
                        minDistance = Vector3.Distance(transform.position, patrolPaths[i].transform.position);
                        patrolPath = patrolPaths[i];
                    }
                }
            }
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, seekTargetRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectedDistance);
        }

#endif
    }
}