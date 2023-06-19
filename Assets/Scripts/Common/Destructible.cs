using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    /// <summary>
    /// Класс уничтожаемых сущностей
    /// </summary>
    public class Destructible : Entity, ISerializableEntity
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
        /// Событие при получении урона
        /// </summary>
        [SerializeField] private UnityEvent eventOnGetDamage;
        /// <summary>
        /// Действие при получении урона
        /// </summary>
        public UnityAction<Destructible> OnGetDamage;

        /// <summary>
        /// Объект мёртв?
        /// </summary>
        private bool isDead = false;
        public bool IsDead => isDead;

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
        /// <param name="other">Тот, кто наносит урон</param>
        public void ApplyDamage(int damage, Destructible other)
        {
            if (isDead) return;

            currentHitPoints -= damage;

            OnGetDamage?.Invoke(other);

            eventOnGetDamage?.Invoke();

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

        /// <summary>
        /// Найти ближайшего
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <returns>Ближайший дестрактибл</returns>
        public static Destructible FindNearest(Vector3 position)
        {
            float minDist = float.MaxValue;
            Destructible target = null;

            foreach (Destructible dest in AllDestructibles)
            {
                float curDist = Vector3.Distance(dest.transform.position, position);

                if (curDist < minDist)
                {
                    curDist = minDist;
                    target = dest;
                }
            }

            return target;
        }

        /// <summary>
        /// Найти ближайшего из другой команды
        /// </summary>
        /// <param name="destructible">Исходный дестрактибл</param>
        /// <returns>Ближайший дестрактибл</returns>
        public static Destructible FindNearestNonTeamMember(Destructible destructible)
        {
            float minDist = float.MaxValue;
            Destructible target = null;

            foreach (Destructible dest in AllDestructibles)
            {
                float curDist = Vector3.Distance(dest.transform.position, destructible.transform.position);

                if (curDist < minDist && dest.TeamId != destructible.TeamId)
                {
                    curDist = minDist;
                    target = dest;
                }
            }

            return target;
        }

        /// <summary>
        /// Получить список всех членов команды
        /// </summary>
        /// <param name="teamId">ID команды</param>
        /// <returns>Список членов команды</returns>
        public static List<Destructible> GetAllTeamMembers(int teamId)
        {
            List<Destructible> teamDestructible = new List<Destructible>();

            foreach (Destructible dest in AllDestructibles)
            {
                if (dest.TeamId == teamId)
                {
                    teamDestructible.Add(dest);
                }
            }

            return teamDestructible;
        }

        /// <summary>
        /// Получить список всех не членов команды
        /// </summary>
        /// <param name="teamId">ID команды</param>
        /// <returns>Список не членов команды</returns>
        public static List<Destructible> GetAllNonTeamMembers(int teamId)
        {
            List<Destructible> teamDestructible = new List<Destructible>();

            foreach (Destructible dest in AllDestructibles)
            {
                if (dest.TeamId != teamId)
                {
                    teamDestructible.Add(dest);
                }
            }

            return teamDestructible;
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


        #region Serializable

        /// <summary>
        /// Класс для состояния объекта
        /// </summary>
        [System.Serializable]
        public class State
        {
            /// <summary>
            /// Расположение
            /// </summary>
            public Vector3 Position;
            /// <summary>
            /// Здоровье
            /// </summary>
            public int HitPoints;

            public State() { }
        }

        /// <summary>
        /// ID сущности
        /// </summary>
        [SerializeField] private int entityID;
        public long EntityId => entityID;

        public bool IsSerializable()
        {
            return currentHitPoints > 0;
        }

        public string SerializableState()
        {
            State s = new State();

            s.Position = transform.position;
            s.HitPoints = currentHitPoints;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            State s = JsonUtility.FromJson<State>(state);

            transform.position = s.Position;
            currentHitPoints = s.HitPoints;
        }

        #endregion
    }
}
