using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Свойства шагов
    /// </summary>
    [System.Serializable]
    public class FootStepProperties
    {
        /// <summary>
        /// Скорость
        /// </summary>
        public float Speed;
        /// <summary>
        /// Задержка
        /// </summary>
        public float Delay;
    }

    /// <summary>
    /// Звуки шагов
    /// </summary>
    public class FootStepSound : MonoBehaviour
    {
        /// <summary>
        /// Массив свойств шагов
        /// </summary>
        [SerializeField] private FootStepProperties[] properties;

        /// <summary>
        /// Контроллер персонажа
        /// </summary>
        [SerializeField] private CharacterController characterController;

        /// <summary>
        /// Источник звука
        /// </summary>
        [SerializeField] private NoiseAudioSource noiseAudioSource;

        /// <summary>
        /// Задержка
        /// </summary>
        private float delay;

        /// <summary>
        /// Таймер
        /// </summary>
        private float tick;


        private void Update()
        {
            if (IsPlay() == false)
            {
                tick = 0;
                return;
            }

            tick += Time.deltaTime;
            delay = GetDelayBySpeed(GetSpeed());

            if (tick >= delay)
            {
                noiseAudioSource.Play();
                tick = 0;
            }
        }


        /// <summary>
        /// Получить скорость
        /// </summary>
        /// <returns>Скорость</returns>
        private float GetSpeed()
        {
            return characterController.velocity.magnitude;
        }

        /// <summary>
        /// Получить задержку, соответствующую скорости
        /// </summary>
        /// <param name="speed">Скорость</param>
        /// <returns>Задержка</returns>
        private float GetDelayBySpeed(float speed)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (speed <= properties[i].Speed)
                {
                    return properties[i].Delay;
                }
            }

            return properties[properties.Length - 1].Delay;
        }

        /// <summary>
        /// Проигрывается ли звук
        /// </summary>
        /// <returns>Проигрывается ли звук</returns>
        private bool IsPlay()
        {
            if (GetSpeed() < 0.01f || characterController.isGrounded == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}