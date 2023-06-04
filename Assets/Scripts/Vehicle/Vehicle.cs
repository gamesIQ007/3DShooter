using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Транспорт
    /// </summary>
    public class Vehicle : Destructible
    {
        /// <summary>
        /// Максимальная скорость
        /// </summary>
        [SerializeField] protected float maxLinearVelocity;

        /// <summary>
        /// Линейная скорость
        /// </summary>
        public virtual float LinearVelocity => 0;

        /// <summary>
        /// Нормализованная линейная скорость
        /// </summary>
        public float NormalizedLinearVelocity
        {
            get
            {
                if (Mathf.Approximately(0, LinearVelocity)) return 0;

                return Mathf.Clamp01(LinearVelocity / maxLinearVelocity);
            }
        }

        /// <summary>
        /// Входные данные с управления
        /// </summary>
        protected Vector3 targetInputControl;

        /// <summary>
        /// Задать данные с управления
        /// </summary>
        /// <param name="control">Вектор управления</param>
        public void SetTargetControl(Vector3 control)
        {
            targetInputControl = control.normalized;
        }
        
        [Header("Engine Sound")]
        /// <summary>
        /// Истояник звука двигателя
        /// </summary>
        [SerializeField] private AudioSource engineSFX;

        /// <summary>
        /// Модификатор звука
        /// </summary>
        [SerializeField] private float engineSFXModifier;

        protected virtual void Update()
        {
            UpdateEngineSFX();
        }

        /// <summary>
        /// Обновление звука двигателя
        /// </summary>
        private void UpdateEngineSFX()
        {
            if (engineSFX != null)
            {
                engineSFX.pitch = 1.0f + engineSFXModifier * NormalizedLinearVelocity;
                engineSFX.volume = 0.5f + NormalizedLinearVelocity;
            }
        }
    }
}