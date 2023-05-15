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
        /// Свойства действия взаимодействия
        /// </summary>
        [SerializeField] private ActionInteractProperties actionProperties;

        /// <summary>
        /// Событие при взаимодействии
        /// </summary>
        [SerializeField] private UnityEvent eventOnInteract;
        public UnityEvent EventOnInteract => eventOnInteract;

        /// <summary>
        /// Тот, кто совершает действие
        /// </summary>
        private GameObject owner;

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
        }

        /// <summary>
        /// Действие завершилось
        /// </summary>
        private void ActionEnded()
        {
            action.IsCanStart = false;
            action.EventOnStart.RemoveListener(ActionStarted);
            action.EventOnEnd.RemoveListener(ActionEnded);

            eventOnInteract?.Invoke();

            OnEndAction(owner);

            interactAmount--;
        }
    }
}