using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    [RequireComponent(typeof(BoxCollider))]

    /// <summary>
    /// Триггер действия взаимодействия
    /// </summary>
    public class TriggerInteractAction : MonoBehaviour
    {
        /// <summary>
        /// Тип взаимодействия
        /// </summary>
        [SerializeField] private InteractType interactType;

        /// <summary>
        /// Количество взаимодействий
        /// </summary>
        [SerializeField] private int interactAmount;

        /// <summary>
        /// Событие при начале взаимодействии
        /// </summary>
        [SerializeField] private UnityEvent eventStartInteract;
        public UnityEvent EventStartInteract => eventStartInteract;

        /// <summary>
        /// Событие при окончании взаимодействии
        /// </summary>
        [SerializeField] private UnityEvent eventEndInteract;
        public UnityEvent EventEndInteract => eventEndInteract;

        /// <summary>
        /// Свойства действия взаимодействия
        /// </summary>
        [SerializeField] protected ActionInteractProperties actionProperties;

        /// <summary>
        /// Тот, кто совершает действие
        /// </summary>
        protected GameObject owner;

        /// <summary>
        /// Действие взаимодействия
        /// </summary>
        protected ActionInteract action;


        #region Unity Events

        private void OnTriggerEnter(Collider other)
        {
            if (interactAmount == 0) return;

            EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

            if (actionCollector != null)
            {
                action = GetActionInteract(actionCollector);

                if (action != null)
                {
                    InitActionProperties();

                    action.IsCanStart = true;
                    action.EventOnStart.AddListener(ActionStarted);
                    action.EventOnEnd.AddListener(ActionEnded);
                    owner = other.gameObject;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (interactAmount == 0) return;

            EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

            if (actionCollector != null)
            {
                action = GetActionInteract(actionCollector);

                if (action != null)
                {
                    action.IsCanStart = false;
                    action.EventOnStart.RemoveListener(ActionStarted);
                    action.EventOnEnd.RemoveListener(ActionEnded);
                }
            }
        }

        #endregion


        /// <summary>
        /// Инициализация свойств действия
        /// </summary>
        protected virtual void InitActionProperties()
        {
            action.SetProperties(actionProperties);
        }

        /// <summary>
        /// Старт действия
        /// </summary>
        /// <param name="owner">Тот, кто совершает действие</param>
        protected virtual void OnStartAction(GameObject owner) { }

        /// <summary>
        /// Конец действия
        /// </summary>
        /// <param name="owner">Тот, кто совершает действие</param>
        protected virtual void OnEndAction(GameObject owner) { }


        /// <summary>
        /// Получить действие взаимодействия
        /// </summary>
        /// <param name="entityActionCollector">Коллектор действий с объектами</param>
        /// <returns>Действие взаимодействия</returns>
        private ActionInteract GetActionInteract(EntityActionCollector entityActionCollector)
        {
            List<ActionInteract> actions = entityActionCollector.GetActionList<ActionInteract>();

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].Type == interactType)
                {
                    return actions[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Действие началось
        /// </summary>
        private void ActionStarted()
        {
            OnStartAction(owner);

            interactAmount--;

            eventStartInteract?.Invoke();
        }

        /// <summary>
        /// Действие завершилось
        /// </summary>
        protected virtual void ActionEnded()
        {
            action.IsCanStart = false;
            action.IsCanEnd = false;

            action.EventOnStart.RemoveListener(ActionStarted);
            action.EventOnEnd.RemoveListener(ActionEnded);

            eventEndInteract?.Invoke();

            OnEndAction(owner);
        }
    }
}