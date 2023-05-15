using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    /// <summary>
    /// Свойства действия с объектом
    /// </summary>
    public abstract class EntityActionProperties { }

    /// <summary>
    /// Действие с объектом
    /// </summary>
    public abstract class EntityAction : MonoBehaviour
    {
        /// <summary>
        /// Событие при начале действия
        /// </summary>
        [SerializeField] private UnityEvent eventOnStart;
        public UnityEvent EventOnStart => eventOnStart;

        /// <summary>
        /// Событие при завершении действия
        /// </summary>
        [SerializeField] private UnityEvent eventOnEnd;
        public UnityEvent EventOnEnd => eventOnEnd;

        /// <summary>
        /// Свойства
        /// </summary>
        private EntityActionProperties properties;
        public EntityActionProperties Properties => properties;

        /// <summary>
        /// Началось
        /// </summary>
        private bool isStarted;


        #region Public API

        /// <summary>
        /// Начало действия
        /// </summary>
        public virtual void StartAction()
        {
            if (isStarted) return;

            isStarted = true;
            eventOnStart?.Invoke();
        }

        /// <summary>
        /// Завершение действия
        /// </summary>
        public virtual void EndAction()
        {
            isStarted = false;
            eventOnEnd?.Invoke();
        }

        /// <summary>
        /// Задать свойства
        /// </summary>
        /// <param name="prop">Свойства</param>
        public virtual void SetProperties(EntityActionProperties prop)
        {
            properties = prop;
        }

        #endregion
    }
}