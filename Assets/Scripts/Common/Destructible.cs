using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    /// <summary>
    /// Класс уничтожаемых сущностей
    /// </summary>
    public class Destructible : Entity
    {
        /// <summary>
        /// Максимальное количество здоровья
        /// </summary>
        [SerializeField] protected int hitPoints;
        public int MaxHitPoints => hitPoints;

        /// <summary>
        /// Текущее количество здоровья
        /// </summary>
        private int currentHitPoints;
        public int HitPoints => currentHitPoints;

        /// <summary>
        /// Эвент, происходящий со смертью
        /// </summary>
        [SerializeField] protected UnityEvent eventOnDeath;
        public UnityEvent EventOnDeath => eventOnDeath;

        /// <summary>
        /// Объект мёртв?
        /// </summary>
        private bool isDead = false;

        /// <summary>
        /// Коллекция всех уничтожаемых объектов
        /// </summary>
        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        /// <summary>
        /// ID нейтральной команды
        /// </summary>
        public const int TeamIdNeutral = 0;

        /// <summary>
        /// ID команды
        /// </summary>
        [SerializeField] private int teamId;
        public int TeamId => teamId;


        #region Unity Events

        protected virtual void Start()
        {
            currentHitPoints = hitPoints;
        }

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        #endregion


        #region Public API

        /// <summary>
        /// Применить урон к объекту
        /// </summary>
        /// <param name="damage">Применяемый урон</param>
        public void ApplyDamage(int damage)
        {
            if (isDead) return;

            currentHitPoints -= damage;
            if (currentHitPoints <= 0)
            {
                isDead = true;
                OnDeath();
            }
        }

        /// <summary>
        /// Восстановить здоровье
        /// </summary>
        /// <param name="heal">Количество восстанавливаемого здоровья</param>
        public void ApplyHeal(int heal)
        {
            currentHitPoints += heal;

            if (currentHitPoints > hitPoints)
            {
                currentHitPoints = hitPoints;
            }
        }

        /// <summary>
        /// Полное восстановление здоровья
        /// </summary>
        public void HealFull()
        {
            currentHitPoints = hitPoints;
        }

        #endregion


        /// <summary>
        /// Смерть объекта
        /// </summary>
        protected virtual void OnDeath()
        {
            Destroy(gameObject);
            eventOnDeath?.Invoke();
        }
    }
}
