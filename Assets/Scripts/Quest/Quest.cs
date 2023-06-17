using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    /// <summary>
    /// Квест
    /// </summary>
    public class Quest : MonoBehaviour
    {
        /// <summary>
        /// Событие при завершении квеста
        /// </summary>
        public UnityAction Completed;

        /// <summary>
        /// Следующий квест
        /// </summary>
        [SerializeField] private Quest nextQuest;
        public Quest NextQuest => nextQuest;

        /// <summary>
        /// Свойства квеста
        /// </summary>
        [SerializeField] private QuestProperties properties;
        public QuestProperties Properties => properties;

        /// <summary>
        /// Точка назначения
        /// </summary>
        [SerializeField] private Transform reachedPoint;
        public Transform ReachedPoint => reachedPoint;


        private void Update()
        {
            UpdateCompleteCondition();
        }


        /// <summary>
        /// Обновление условий завершения
        /// </summary>
        protected virtual void UpdateCompleteCondition() { }
    }
}