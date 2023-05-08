using UnityEngine;

namespace Shooter3D
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponMode mode;
        public WeaponMode Mode => mode;

        [SerializeField] private WeaponProperties weaponProperties;

        [SerializeField] private AudioSource audioSource;

        [SerializeField] private ParticleSystem muzzleParticleSystem;

        [SerializeField] private Transform firePoint;

        [SerializeField] private float primaryMaxEnergy;
        public float PrimaryMaxEnergy => primaryMaxEnergy;

        private float primaryEnergy;
        public float PrimaryEnergy => primaryEnergy;

        private float refireTimer;

        /// <summary>
        /// Энергия восстанавливается
        /// </summary>
        private bool energyIsRestored;

        private Destructible owner;

        public bool CanFire => refireTimer <= 0 && energyIsRestored == false;

        private void Start()
        {
            primaryEnergy = primaryMaxEnergy;
            owner = transform.root.GetComponent<Destructible>();
        }

        private void Update()
        {
            if (refireTimer > 0)
            {
                refireTimer -= Time.deltaTime;
            }

            UpdateEnergy();
        }

        private void UpdateEnergy()
        {
            primaryEnergy += (float)weaponProperties.EnergyRegenPerSecond * Time.deltaTime;
            primaryEnergy = Mathf.Clamp(primaryEnergy, 0, primaryMaxEnergy);

            if (primaryEnergy >= weaponProperties.EnergyAmountToStartFire)
            {
                energyIsRestored = false;
            }
        }

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

        public void FirePointLookAt(Vector3 pos)
        {
            Vector3 offset = Random.insideUnitSphere * weaponProperties.SpreadShotRange;

            if (weaponProperties.SpreadShotDistanceFactor != 0)
            {
                offset = offset * Vector3.Distance(firePoint.position, pos) * weaponProperties.SpreadShotDistanceFactor;
            }

            firePoint.LookAt(pos + offset);
        }

        public void AssignLoadout(WeaponProperties props)
        {
            if (mode != props.Mode) return;

            refireTimer = 0;

            weaponProperties = props;
        }
    }
}