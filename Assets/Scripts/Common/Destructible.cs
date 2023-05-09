using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    public class Destructible : Entity
    {
        [SerializeField] private bool indestructible;
        public bool IsIndestructible => indestructible;

        [SerializeField] protected int hitPoints;
        public int MaxHitPoints => hitPoints;

        private int currentHitPoints;
        public int HitPoints => currentHitPoints;

        [SerializeField] protected UnityEvent eventOnDeath;
        public UnityEvent EventOnDeath => eventOnDeath;

        private float timeOfTemporaryIndestructible;

        [SerializeField] private UnityEvent eventOnEnableTemporaryIndestructible;

        [SerializeField] private UnityEvent eventOnDisableTemporaryIndestructible;

        private bool isDead = false;

        protected virtual void Start()
        {
            currentHitPoints = hitPoints;
        }

        protected virtual void Update()
        {
            if (timeOfTemporaryIndestructible <= 0) return;

            timeOfTemporaryIndestructible -= Time.deltaTime;

            if (timeOfTemporaryIndestructible <= 0)
            {
                DisableTemporaryIndestructible();
            }
        }

        public void ApplyDamage(int damage)
        {
            if (indestructible || isDead) return;

            currentHitPoints -= damage;
            if (currentHitPoints <= 0)
            {
                isDead = true;
                OnDeath();
            }
        }

        public void ApplyTemporaryIndestructible(float time)
        {
            timeOfTemporaryIndestructible += time;
            indestructible = true;
            eventOnEnableTemporaryIndestructible?.Invoke();
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);
            eventOnDeath?.Invoke();
        }

        private void DisableTemporaryIndestructible()
        {
            indestructible = false;
            timeOfTemporaryIndestructible = 0;
            eventOnDisableTemporaryIndestructible?.Invoke();
        }

        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

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
    }
}
