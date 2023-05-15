using System.Collections.Generic;
using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Коллектор действий с объектами
    /// </summary>
    public class EntityActionCollector : MonoBehaviour
    {
        /// <summary>
        /// Трансформа с действиями
        /// </summary>
        [SerializeField] private Transform parentTransformWithAction;

        /// <summary>
        /// Все действия
        /// </summary>
        private List<EntityAction> allActions = new List<EntityAction>();


        private void Awake()
        {
            for (int i = 0; i < parentTransformWithAction.childCount; i++)
            {
                if (parentTransformWithAction.GetChild(i).gameObject.activeSelf)
                {
                    EntityAction action = parentTransformWithAction.GetChild(i).GetComponent<EntityAction>();

                    if (action != null)
                    {
                        allActions.Add(action);
                    }
                }
            }
        }


        /// <summary>
        /// Получить действие
        /// </summary>
        /// <typeparam name="T">Тип действия</typeparam>
        /// <returns>Действие</returns>
        public T GetAction<T>() where T : EntityAction
        {
            for (int i = 0; i < allActions.Count; i++)
            {
                if (allActions[i] is T)
                {
                    return (T) allActions[i];
                }
            }

            return null;
        }


        /// <summary>
        /// Получить список действий
        /// </summary>
        /// <typeparam name="T">Тип действия</typeparam>
        /// <returns>Список действий</returns>
        public List<T> GetActionList<T>() where T : EntityAction
        {
            List<T> actions = new List<T>();

            for (int i = 0; i < allActions.Count; i++)
            {
                if (allActions[i] is T)
                {
                    actions.Add((T) allActions[i]);
                }
            }

            return actions;
        }
    }
}