using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    public class UISight : MonoBehaviour
    {
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private Image imageSight;

        private void Update()
        {
            imageSight.enabled = characterMovement.IsAiming;
        }
    }
}