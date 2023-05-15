using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Риг прицеливания
    /// </summary>
    public class AimingRig : MonoBehaviour
    {
        /// <summary>
        /// Перемещатель персонажа
        /// </summary>
        [SerializeField] private CharacterMovement targetCharacter;
        /// <summary>
        /// Массив ригов
        /// </summary>
        [SerializeField] private UnityEngine.Animations.Rigging.Rig[] rigs;

        /// <summary>
        /// Коэффициент изменения веса рига
        /// </summary>
        [SerializeField] private float changeWeightLerpRate;

        /// <summary>
        /// Целевой вес рига
        /// </summary>
        private float targetWeight;


        private void Update()
        {
            for (int i = 0; i < rigs.Length; i++)
            {
                rigs[i].weight = Mathf.MoveTowards(rigs[i].weight, targetWeight, changeWeightLerpRate * Time.deltaTime);
            }

            if (targetCharacter.IsAiming)
            {
                targetWeight = 1;
            }
            else
            {
                targetWeight = 0;
            }
        }
    }
}