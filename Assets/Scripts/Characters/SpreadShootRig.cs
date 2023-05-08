using UnityEngine;

namespace Shooter3D
{
    public class SpreadShootRig : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animations.Rigging.Rig spreadRig;

        [SerializeField] private float changeWeightLerpRate;

        private float targetWeight;

        private void Update()
        {
            spreadRig.weight = Mathf.MoveTowards(spreadRig.weight, targetWeight, changeWeightLerpRate * Time.deltaTime);

            if (spreadRig.weight == 1)
            {
                targetWeight = 0;
            }
        }

        public void Spread()
        {
            targetWeight = 1;
        }
    }
}