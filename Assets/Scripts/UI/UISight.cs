using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    /// <summary>
    /// Отображение прицела в интерфейсе
    /// </summary>
    public class UISight : MonoBehaviour
    {
        /// <summary>
        /// Перемещатель персонажа
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;
        /// <summary>
        /// Изображение прицела
        /// </summary>
        [SerializeField] private Image imageSight;


        private void Update()
        {
            imageSight.enabled = characterMovement.IsAiming;
        }
    }
}