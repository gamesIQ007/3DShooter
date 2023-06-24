using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Тип эффекта
    /// </summary>
    public enum ImpactType
    {
        /// <summary>
        /// Без дырок
        /// </summary>
        NoDecal,
        /// <summary>
        /// По умолчанию
        /// </summary>
        Default
    }

    /// <summary>
    /// Импакт эффект
    /// </summary>
    public class ImpactEffect : MonoBehaviour
    {
        /// <summary>
        /// Время жизни
        /// </summary>
        [SerializeField] private float lifeTime;

        /// <summary>
        /// Дырка от пули
        /// </summary>
        [SerializeField] private GameObject decal;

        /// <summary>
        /// Таймер
        /// </summary>
        private float timer;


        private void Update()
        {
            if (timer < lifeTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// Обновить тип эффекта
        /// </summary>
        /// <param name="type">Тип эффекта</param>
        public void UpdateType(ImpactType type)
        {
            if (type == ImpactType.NoDecal)
            {
                decal.SetActive(false);
            }
        }
    }
}