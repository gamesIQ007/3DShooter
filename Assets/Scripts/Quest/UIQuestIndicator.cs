using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    /// <summary>
    /// Указатель к цели квеста
    /// </summary>
    public class UIQuestIndicator : MonoBehaviour
    {
        /// <summary>
        /// Сборщик квестов
        /// </summary>
        [SerializeField] private QuestCollector questCollector;

        /// <summary>
        /// Камера
        /// </summary>
        [SerializeField] private new Camera camera;

        /// <summary>
        /// Указатель на точку назначения
        /// </summary>
        [SerializeField] private Image indicator;

        /// <summary>
        /// Точка назначения
        /// </summary>
        private Transform reachedPoint;


        #region Unity Events

        private void Start()
        {
            indicator.gameObject.SetActive(false);

            questCollector.QuestReceived += OnQuestReceived;
            questCollector.QuestCompleted += OnQuestCompleted;
        }

        private void Update()
        {
            if (reachedPoint == null) return;

            Vector3 pos = camera.WorldToScreenPoint(reachedPoint.position);

            if (pos.z > 0)
            {
                if (pos.x < 0) pos.x = 0;
                if (pos.x > Screen.width) pos.x = Screen.width;
                if (pos.y < 0) pos.y = 0;
                if (pos.y > Screen.height) pos.y = Screen.height;

                indicator.transform.position = pos;
            }
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
            indicator.gameObject.SetActive(true);
            reachedPoint = quest.ReachedPoint;
        }

        /// <summary>
        /// При выполнении квеста
        /// </summary>
        /// <param name="quest">Квест</param>
        private void OnQuestCompleted(Quest quest)
        {
            indicator.gameObject.SetActive(false);
        }
    }
}