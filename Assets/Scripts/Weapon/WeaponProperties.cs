using UnityEngine;

namespace Shooter3D
{
    public enum WeaponMode
    {
        Primary,
        Secondary
    }

    [CreateAssetMenu]
    public sealed class WeaponProperties : ScriptableObject
    {
        [SerializeField] private WeaponMode mode;
        public WeaponMode Mode => mode;

        [SerializeField] private Projectile projectilePrefab;
        public Projectile ProjectilePrefab => projectilePrefab;

        [SerializeField] private float rateOfFire;
        public float RateOfFire => rateOfFire;

        [SerializeField] private int energyUsage;
        public int EnergyUsage => energyUsage;

        [SerializeField] private int energyAmountToStartFire;
        public int EnergyAmountToStartFire => energyAmountToStartFire;

        [SerializeField] private int energyRegenPerSecond;
        public int EnergyRegenPerSecond => energyRegenPerSecond;

        [SerializeField] private float spreadShotRange;
        public float SpreadShotRange => spreadShotRange;

        [SerializeField] private float spreadShotDistanceFactor = 0.1f;
        public float SpreadShotDistanceFactor => spreadShotDistanceFactor;

        [SerializeField] private AudioClip launchSFX;
        public AudioClip LaunchSFX => launchSFX;
    }
}