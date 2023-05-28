using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Солдат пришельцев
    /// </summary>
    public class AlienSoldier : Destructible
    {
        /// <summary>
        /// Оружие
        /// </summary>
        [SerializeField] private Weapon weapon;

        /// <summary>
        /// Риг разброса
        /// </summary>
        [SerializeField] private SpreadShootRig spreadShootRig;


        /// <summary>
        /// Выстрел
        /// </summary>
        /// <param name="target">Цель выстрела</param>
        public void Fire(Vector3 target)
        {
            if (weapon.CanFire == false) return;

            weapon.FirePointLookAt(target);
            weapon.Fire();

            spreadShootRig.Spread();
        }


        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();
        }
    }
}