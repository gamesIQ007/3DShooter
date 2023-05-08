using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    public class UIWeaponEnergy : MonoBehaviour
    {
        [SerializeField] private Weapon targetWeapon;
        [SerializeField] private Slider slider;

        [SerializeField] private Image[] images;

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

        private void SetActiveImages(bool active)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = active;
            }
        }
    }
}