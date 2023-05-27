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
            CirclePatrol
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
        /// Путь перемещения
        /// </summary>
        private NavMeshPath navMeshPath;
        /// <summary>
        /// Текущая точка перемещения
        /// </summary>
        private PatrolPathNode currentPathNode;


        #region Unity Events

        private void Start()
        {
            characterMovement.UpdatePosition = false;
            navMeshPath = new NavMeshPath();
            StartBehaviour(aIBehaviour);
        }

        private void Update()
        {
            SyncAgentAndCharacterMovement();
            UpdateAI();
        }

        #endregion


        /// <summary>
        /// Синхронизация агента и движения персонажа
        /// </summary>
        private void SyncAgentAndCharacterMovement()
        {
            float factor = agent.velocity.magnitude / agent.speed;
            characterMovement.TargetDirectionControl = transform.InverseTransformDirection(agent.velocity.normalized) * factor;
        }

        /// <summary>
        /// Обновление ИИ
        /// </summary>
        private void UpdateAI()
        {
            if (aIBehaviour == AIBehaviour.Idle)
            {
                return;
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
        /// Начать поведение
        /// </summary>
        /// <param name="behaviour">Поведение</param>
        private void StartBehaviour(AIBehaviour state)
        {
            if (state == AIBehaviour.Idle)
            {
                agent.isStopped = true;
            }

            if (state == AIBehaviour.PatrolRandom)
            {
                agent.isStopped = false;
                SetDestinationByPathNode(patrolPath.GetRandomPathNode());
            }

            if (state == AIBehaviour.CirclePatrol)
            {
                agent.isStopped = false;
                SetDestinationByPathNode(patrolPath.GetNextNode(ref patrolPathNodeIndex));
            }

            aIBehaviour = state;
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

        IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
        {
            AIBehaviour previous = aIBehaviour;
            aIBehaviour = state;
            StartBehaviour(aIBehaviour);

            yield return new WaitForSeconds(second);

            StartBehaviour(previous);
        }
    }
}