using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    /// <summary>
    /// Отображение полоски здоровья в интерфейсе
    /// </summary>
    public class UIHitPointSlider : MonoBehaviour
    {
        /// <summary>
        /// Объект, имеющий здровье
        /// </summary>
        [SerializeField] private Destructible destructible;

        /// <summary>
        /// Слайдер
        /// </summary>
        [SerializeField] private Slider slider;


        #region Unity Events

        private void Start()
        {
            slider.maxValue = destructible.MaxHitPoints;
            slider.value = slider.maxValue;
        }

        private void Update()
        {
            slider.value = destructible.HitPoints;
        }

        #endregion
    }
}