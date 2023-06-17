using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    /// <summary>
    /// Отображение информации о текущем квесте
    /// </summary>
    public class UIQuestInfo : MonoBehaviour
    {
        /// <summary>
        /// Сборщик квестов
        /// </summary>
        [SerializeField] private QuestCollector questCollector;

        /// <summary>
        /// Описание
        /// </summary>
        [SerializeField] private Text description;
        /// <summary>
        /// Задание
        /// </summary>
        [SerializeField] private Text task;


        #region Unity Events

        private void Start()
        {
            description.gameObject.SetActive(false);
            task.gameObject.SetActive(false);

            questCollector.QuestReceived += OnQuestReceived;
            questCollector.QuestCompleted += OnQuestCompleted;
        }

        private void OnDestroy()
        {
            questCollector.QuestReceived -= OnQuestReceived;
            questCollector.QuestCompleted -= OnQuestCompleted;
        }

        #endregion


        /// <summary>
        /// При получении квеста
        /// </summary>
        /// <param name="quest">Квест</param>
        private void OnQuestReceived(Quest quest)
        {
            description.gameObject.SetActive(true);
            task.gameObject.SetActive(true);

            description.text = quest.Properties.Description;
            task.text = quest.Properties.Task;
        }

        /// <summary>
        /// При выполнении квеста
        /// </summary>
        /// <param name="quest">Квест</param>
        private void OnQuestCompleted(Quest quest)
        {
            description.gameObject.SetActive(false);
            task.gameObject.SetActive(false);
        }
    }
}