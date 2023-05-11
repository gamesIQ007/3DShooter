using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Класс, отвечающий за стрельбу игрока
    /// </summary>
    public class PlayerShooter : MonoBehaviour
    {
        /// <summary>
        /// Перемещаемый персонаж
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;
        /// <summary>
        /// Оружие
        /// </summary>
        [SerializeField] private Weapon weapon;
        /// <summary>
        /// Риг разброса при стрельбе
        /// </summary>
        [SerializeField] private SpreadShootRig spreadShootRig;
        /// <summary>
        /// Камера
        /// </summary>
        [SerializeField] private new Camera camera;
        /// <summary>
        /// Знак прицела
        /// </summary>
        [SerializeField] private RectTransform imageSight;


        /// <summary>
        /// Выстрел
        /// </summary>
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