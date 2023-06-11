using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Индикатор обнаружения
    /// </summary>
    public class UIPlayerNotic : MonoBehaviour
    {
        /// <summary>
        /// Объект, показывающий, что игрок замечен
        /// </summary>
        [SerializeField] private GameObject notic;


        /// <summary>
        /// Показать
        /// </summary>
        public void Show()
        {
            notic.SetActive(true);
        }

        /// <summary>
        /// Скрыть
        /// </summary>
        public void Hide()
        {
            notic.SetActive(false);
        }
    }
}