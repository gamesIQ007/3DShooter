using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Риг разброса при стрельбе
    /// </summary>
    public class SpreadShootRig : MonoBehaviour
    {
        /// <summary>
        /// Риг
        /// </summary>
        [SerializeField] private UnityEngine.Animations.Rigging.Rig spreadRig;

        /// <summary>
        /// Коэффициент изменения веса
        /// </summary>
        [SerializeField] private float changeWeightLerpRate;

        /// <summary>
        /// Целевой вес
        /// </summary>
        private float targetWeight;


        private void Update()
        {
            spreadRig.weight = Mathf.MoveTowards(spreadRig.weight, targetWeight, changeWeightLerpRate * Time.deltaTime);

            if (spreadRig.weight == 1)
            {
                targetWeight = 0;
            }
        }


        /// <summary>
        /// Разброс
        /// </summary>
        public void Spread()
        {
            targetWeight = 1;
        }
    }
}