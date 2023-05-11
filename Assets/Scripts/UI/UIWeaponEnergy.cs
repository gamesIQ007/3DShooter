using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    /// <summary>
    /// Отображение энергии оружия в интерфейсе
    /// </summary>
    public class UIWeaponEnergy : MonoBehaviour
    {
        /// <summary>
        /// Оружие
        /// </summary>
        [SerializeField] private Weapon targetWeapon;
        /// <summary>
        /// Слайдер
        /// </summary>
        [SerializeField] private Slider slider;

        /// <summary>
        /// Изображения
        /// </summary>
        [SerializeField] private Image[] images;


        #region Unity Events

        private void Start()
        {
            slider.maxValue = targetWeapon.PrimaryMaxEnergy;
            slider.value = slider.maxValue;
        }

        private void Update()
        {
            slider.value = targetWeapon.PrimaryEnergy;

            SetActiveImages(targetWeapon.PrimaryEnergy != targetWeapon.PrimaryMaxEnergy);
        }

        #endregion


        /// <summary>
        /// Задать активность изображений
        /// </summary>
        /// <param name="active">Активны ли?</param>
        private void SetActiveImages(bool active)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = active;
            }
        }
    }
}