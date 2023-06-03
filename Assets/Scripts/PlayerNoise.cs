using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Шум от игрока
    /// </summary>
    public class PlayerNoise : MonoBehaviour
    {
        /// <summary>
        /// Перемещающийся персонаж
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;

        /// <summary>
        /// Оружие игрока
        /// </summary>
        [SerializeField] private Weapon weapon;

        /// <summary>
        /// Радиус шума при движении в приседе
        /// </summary>
        [SerializeField] private float noiseRadiusCrouch;
        /// <summary>
        /// Радиус шума при ходьбе
        /// </summary>
        [SerializeField] private float noiseRadiusWalk;
        /// <summary>
        /// Радиус шума при движении в прицеливании
        /// </summary>
        [SerializeField] private float noiseRadiusAim;
        /// <summary>
        /// Радиус шума при беге
        /// </summary>
        [SerializeField] private float noiseRadiusSprint;
        /// <summary>
        /// Радиус шума при стрельбе
        /// </summary>
        [SerializeField] private float noiseRadiusFire;

        /// <summary>
        /// Неслышимая скорость перемещения
        /// </summary>
        [SerializeField] private float safetyMoveSpeed;

        /// <summary>
        /// Текущий уровень шума
        /// </summary>
        private float currentNoiseRadius;

        /// <summary>
        /// Перечень вражеских солдатов
        /// </summary>
        private AIAlienSoldier[] alienSoldiers;

        /// <summary>
        /// Источник звука оружия
        /// </summary>
        private AudioSource weaponAudioSource;


        private void Start()
        {
            alienSoldiers = FindObjectsOfType<AIAlienSoldier>();
            weaponAudioSource = weapon.GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (characterMovement.CurrentSpeed * characterMovement.TargetDirectionControl.sqrMagnitude > safetyMoveSpeed)
            {
                if (characterMovement.IsCrouch)
                {
                    currentNoiseRadius = noiseRadiusCrouch;
                }
                else
                {
                    if (characterMovement.IsAiming)
                    {
                        currentNoiseRadius = noiseRadiusAim;
                    }
                    else
                    {
                        if (characterMovement.IsSprint)
                        {
                            currentNoiseRadius = noiseRadiusSprint;
                        }
                        else
                        {
                            currentNoiseRadius = noiseRadiusWalk;
                        }
                    }
                }
            }
            else
            {
                currentNoiseRadius = 0;
            }
            if (weaponAudioSource.isPlaying)
            {
                currentNoiseRadius = noiseRadiusFire;
            }

            for (int i = 0; i < alienSoldiers.Length; i++)
            {
                if (alienSoldiers[i].enabled && currentNoiseRadius > Vector3.Distance(alienSoldiers[i].transform.position, transform.position))
                {
                    alienSoldiers[i].SetPursuitTarget(transform);
                }
            }
        }
    }
}