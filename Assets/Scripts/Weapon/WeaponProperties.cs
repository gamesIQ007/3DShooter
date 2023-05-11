using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Перечень режимов оружия
    /// </summary>
    public enum WeaponMode
    {
        /// <summary>
        /// Основной
        /// </summary>
        Primary,
        /// <summary>
        /// Вторичный
        /// </summary>
        Secondary
    }

    [CreateAssetMenu]

    /// <summary>
    /// Свойства оружия
    /// </summary>
    public sealed class WeaponProperties : ScriptableObject
    {
        /// <summary>
        /// Режим оружия
        /// </summary>
        [SerializeField] private WeaponMode mode;
        public WeaponMode Mode => mode;

        /// <summary>
        /// Префаб снаряда
        /// </summary>
        [SerializeField] private Projectile projectilePrefab;
        public Projectile ProjectilePrefab => projectilePrefab;

        /// <summary>
        /// Частота стрельбы
        /// </summary>
        [SerializeField] private float rateOfFire;
        public float RateOfFire => rateOfFire;

        /// <summary>
        /// Использование энергии
        /// </summary>
        [SerializeField] private int energyUsage;
        public int EnergyUsage => energyUsage;

        /// <summary>
        /// Количество энергии, необходимое для начала стрельбы
        /// </summary>
        [SerializeField] private int energyAmountToStartFire;
        public int EnergyAmountToStartFire => energyAmountToStartFire;

        /// <summary>
        /// Регенерация энергии в секунду
        /// </summary>
        [SerializeField] private int energyRegenPerSecond;
        public int EnergyRegenPerSecond => energyRegenPerSecond;

        /// <summary>
        /// Диапазон разброса стрельбы
        /// </summary>
        [SerializeField] private float spreadShotRange;
        public float SpreadShotRange => spreadShotRange;

        /// <summary>
        /// Фактор зависимости разброса от дистанции до цели
        /// </summary>
        [SerializeField] private float spreadShotDistanceFactor = 0.1f;
        public float SpreadShotDistanceFactor => spreadShotDistanceFactor;

        /// <summary>
        /// Звук выстрела
        /// </summary>
        [SerializeField] private AudioClip launchSFX;
        public AudioClip LaunchSFX => launchSFX;
    }
}