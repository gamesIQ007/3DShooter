using System.Collections.Generic;
using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Управление вводом при контекстных действиях
    /// </summary>
    public class ContextActionInputControl : MonoBehaviour
    {
        /// <summary>
        /// Сборщик действий
        /// </summary>
        [SerializeField] private EntityActionCollector targetActionCollector;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                List<EntityContextAction> actionsList = targetActionCollector.GetActionList<EntityContextAction>();

                for (int i = 0; i < actionsList.Count; i++)
                {
                    actionsList[i].StartAction();
                    actionsList[i].EndAction();
                }
            }
        }
    }
}