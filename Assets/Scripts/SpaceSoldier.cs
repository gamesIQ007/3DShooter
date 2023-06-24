using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Игровой персонаж
    /// </summary>
    public class SpaceSoldier : Destructible
    {
        /// <summary>
        /// Перемещение персонажа
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;

        /// <summary>
        /// Множитель урона от падения
        /// </summary>
        [SerializeField] private float damageFallFactor;


        #region Unity Events

        protected override void Start()
        {
            base.Start();

            characterMovement.Land += OnLand;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            characterMovement.Land -= OnLand;
        }

        #endregion


        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();

            characterMovement.Land -= OnLand;
        }


        /// <summary>
        /// Приземление
        /// </summary>
        /// <param name="vel">Скорость приземления</param>
        private void OnLand(Vector3 vel)
        {
            if (Mathf.Abs(vel.y) < 10) return;

            ApplyDamage((int) (Mathf.Abs(vel.y) * damageFallFactor), this);
        }
    }
}