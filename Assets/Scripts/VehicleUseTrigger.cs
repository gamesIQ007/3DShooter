using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Триггер использования транспорта
    /// </summary>
    public class VehicleUseTrigger : TriggerInteractAction
    {
        /// <summary>
        /// Свойства использования транспорта
        /// </summary>
        [SerializeField] private ActionUseVehiclesProperties useProperties;


        protected override void InitActionProperties()
        {
            action.SetProperties(useProperties);
        }
    }
}