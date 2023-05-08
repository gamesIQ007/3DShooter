using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private Weapon weapon;
        [SerializeField] private SpreadShootRig spreadShootRig;
        [SerializeField] private new Camera camera;
        [SerializeField] private RectTransform imageSight;

        public void Shoot()
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(imageSight.position);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                weapon.FirePointLookAt(hit.point);
            }

            if (weapon.CanFire)
            {
                weapon.Fire();
                spreadShootRig.Spread();
                // Можно добавить отключение рига Rifle_Aim, для тряски оружия. Я на половину просто отключил его
            }
        }
    }
}