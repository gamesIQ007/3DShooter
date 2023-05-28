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
            SeekTarget
        }

        /// <summary>
        /// Поведение
        /// </summary>
        [SerializeField] private AIBehaviour aIBehaviour;

        /// <summary>
        /// Солдат пришельцев
        /// </summary>
        [SerializeField] private AlienSoldier alienSoldier;

        /// <summary>
        /// Перемещение персонажа
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;

        [SerializeField] private float aimingDistance;

        /// <summary>
        /// Агент перемещения
        /// </summary>
        [SerializeField] private NavMeshAgent agent;

        /// <summary>
        /// Маршрут патрулирования
        /// </summary>
        [SerializeField] private PatrolPath patrolPath;

        /// <summary>
        /// Индекс точки маршрута патрулирования
        /// </summary>
        [SerializeField] private int patrolPathNodeIndex = 0;

        /// <summary>
        /// Смотрящий на коллайдеры
        /// </summary>
        [SerializeField] private ColliderViewer colliderViewer;

        /// <summary>
        /// Путь перемещения
        /// </summary>
        private NavMeshPath navMeshPath;
        /// <summary>
        /// Текущая точка перемещения
        /// </summary>
        private PatrolPathNode currentPathNode;

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


        #region Unity Events

        private void Start()
        {
            potencialTarget = Destructible.FindNearestNonTeamMember(alienSoldier)?.gameObject;

            characterMovement.UpdatePosition = false;
            navMeshPath = new NavMeshPath();
            StartBehaviour(aIBehaviour);

            alienSoldier.OnGetDamage += OnGetDamage;
        }

        private void Update()
        {
            SyncAgentAndCharacterMovement();
            UpdateAI();
        }

        private void OnDestroy()
        {
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

                    alienSoldier.Fire(pursuitTarget.position + new Vector3(0, 1, 0));
                }
                else
                {
                    characterMovement.UnAiming();
                }
            }

            if (aIBehaviour == AIBehaviour.SeekTarget)
            {
                agent.CalculatePath(seekTarget, navMeshPath);
                agent.SetPath(navMeshPath);

                if (AgentReachedDestination())
                {
                    StartBehaviour(AIBehaviour.PatrolRandom);
                }
            }

            if (aIBehaviour == AIBehaviour.PatrolRandom)
            {
                if (AgentReachedDestination())
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }

            if (aIBehaviour == AIBehaviour.CirclePatrol)
            {
                if (AgentReachedDestination())
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }
        }

        #endregion


        #region Actions

        /// <summary>
        /// Действие обновления цели
        /// </summary>
        private void ActionUpdateTarget()
        {
            if (potencialTarget == null) return;

            if (colliderViewer.IsObjectVisible(potencialTarget))
            {
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
    }
}