using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Оружие
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Режим оружия
        /// </summary>
        [SerializeField] private WeaponMode mode;
        public WeaponMode Mode => mode;

        /// <summary>
        /// Свойства
        /// </summary>
        [SerializeField] private WeaponProperties weaponProperties;

        /// <summary>
        /// Источник звука
        /// </summary>
        [SerializeField] private AudioSource audioSource;

        /// <summary>
        /// Система частиц вспышка огня
        /// </summary>
        [SerializeField] private ParticleSystem muzzleParticleSystem;

        /// <summary>
        /// Точка, из которой ведётся огонь
        /// </summary>
        [SerializeField] private Transform firePoint;

        /// <summary>
        /// Максимум энергии
        /// </summary>
        [SerializeField] private float primaryMaxEnergy;
        public float PrimaryMaxEnergy => primaryMaxEnergy;

        /// <summary>
        /// Текущее количество энергии
        /// </summary>
        private float primaryEnergy;
        public float PrimaryEnergy => primaryEnergy;

        /// <summary>
        /// Таймер перезарядки
        /// </summary>
        private float refireTimer;

        /// <summary>
        /// Энергия восстанавливается
        /// </summary>
        private bool energyIsRestored;

        /// <summary>
        /// Стрелок
        /// </summary>
        private Destructible owner;

        /// <summary>
        /// Возможность стрелять
        /// </summary>
        public bool CanFire => refireTimer <= 0 && energyIsRestored == false;


        #region Unity Events

        private void Start()
        {
            primaryEnergy = primaryMaxEnergy;
            owner = transform.root.GetComponent<Destructible>();
        }

        protected virtual void Update()
        {
            if (refireTimer > 0)
            {
                refireTimer -= Time.deltaTime;
            }

            UpdateEnergy();
        }

        #endregion


        #region Public API

        /// <summary>
        /// Выстрел
        /// </summary>
        public void Fire()
        {
            if (energyIsRestored == true) return;
            if (weaponProperties == null) return;
            if (CanFire == false) return;
            if (TryDrawEnergy(weaponProperties.EnergyUsage) == false) return;

            Projectile projectile = Instantiate(weaponProperties.ProjectilePrefab);
            projectile.transform.position = firePoint.position;
            projectile.transform.forward = firePoint.forward;
            projectile.SetParentShooter(owner);

            refireTimer = weaponProperties.RateOfFire;

            muzzleParticleSystem.time = 0;
            muzzleParticleSystem.Play();

            audioSource.clip = weaponProperties.LaunchSFX;
            audioSource.Play();
        }

        /// <summary>
        /// Направить точку стрельбы на позицию
        /// </summary>
        /// <param name="pos">Позиция</param>
        public void FirePointLookAt(Vector3 pos)
        {
            Vector3 offset = Random.insideUnitSphere * weaponProperties.SpreadShotRange;

            if (weaponProperties.SpreadShotDistanceFactor != 0)
            {
                offset = offset * Vector3.Distance(firePoint.position, pos) * weaponProperties.SpreadShotDistanceFactor;
            }

            firePoint.LookAt(pos + offset);
        }

        /// <summary>
        /// Применить свойства оружия
        /// </summary>
        /// <param name="props">Свойства</param>
        public void AssignLoadout(WeaponProperties props)
        {
            if (mode != props.Mode) return;

            refireTimer = 0;

            weaponProperties = props;
        }

        #endregion


        /// <summary>
        /// Обновление энергии
        /// </summary>
        private void UpdateEnergy()
        {
            primaryEnergy += (float)weaponProperties.EnergyRegenPerSecond * Time.deltaTime;
            primaryEnergy = Mathf.Clamp(primaryEnergy, 0, primaryMaxEnergy);

            if (primaryEnergy >= weaponProperties.EnergyAmountToStartFire)
            {
                energyIsRestored = false;
            }
        }

        /// <summary>
        /// Попытка потратить энергию
        /// </summary>
        /// <param name="count">Количество энергии</param>
        /// <returns>Успех попытки</returns>
        private bool TryDrawEnergy(int count)
        {
            if (count == 0)
            {
                return true;
            }
            if (primaryEnergy >= count)
            {
                primaryEnergy -= count;
                return true;
            }
            energyIsRestored = true;
            return false;
        }

        
    }
}