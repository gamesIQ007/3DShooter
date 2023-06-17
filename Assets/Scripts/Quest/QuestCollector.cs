using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    /// <summary>
    /// Сборщик квестов
    /// </summary>
    public class QuestCollector : MonoBehaviour
    {
        /// <summary>
        /// Событие при получении квеста
        /// </summary>
        public UnityAction<Quest> QuestReceived;
        /// <summary>
        /// Событие при завершении квеста
        /// </summary>
        public UnityAction<Quest> QuestCompleted;
        /// <summary>
        /// Событие при завершении последнего квеста
        /// </summary>
        public UnityAction LastQuestCompleted;

        /// <summary>
        /// Текущий квест
        /// </summary>
        [SerializeField] private Quest currentQuest;
        public Quest CurrentQuest => currentQuest;


        #region Unity Events

        private void Start()
        {
            if (currentQuest != null)
            {
                AssignQuest(currentQuest);
            }
        }

        private void OnDestroy()
        {
            currentQuest.Completed -= OnQuestCompleted;
        }

        #endregion


        /// <summary>
        /// Принять квест
        /// </summary>
        /// <param name="quest">Принимаемый квест</param>
        public void AssignQuest(Quest quest)
        {
            currentQuest = quest;

            QuestReceived?.Invoke(currentQuest);

            currentQuest.Completed += OnQuestCompleted;
        }


        /// <summary>
        /// При завершении квеста
        /// </summary>
        private void OnQuestCompleted()
        {
            currentQuest.Completed -= OnQuestCompleted;

            QuestCompleted?.Invoke(currentQuest);

            if (currentQuest.NextQuest != null)
            {
                AssignQuest(currentQuest.NextQuest);
            }
            else
            {
                LastQuestCompleted?.Invoke();
            }
        }
    }
}