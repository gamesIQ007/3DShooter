using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Тип взаимодействия
    /// </summary>
    public enum InteractType
    {
        /// <summary>
        /// Подбор предмета
        /// </summary>
        PickupItem,
        /// <summary>
        /// Ввод кода
        /// </summary>
        EnteringCode
    }

    [System.Serializable]
    /// <summary>
    /// Свойства действий взаимодействий
    /// </summary>
    public class ActionInteractProperties : EntityActionProperties
    {
        /// <summary>
        /// Трансформа точки взаимодействия
        /// </summary>
        [SerializeField] private Transform interactTransform;
        public Transform InteractTransform => interactTransform;
    }

    /// <summary>
    /// Действие взаимодействия
    /// </summary>
    public class ActionInteract : EntityContextAction
    {
        /// <summary>
        /// Тот, кто совершает действие
        /// </summary>
        [SerializeField] private Transform owner;

        /// <summary>
        /// Тип взаимодействия
        /// </summary>
        [SerializeField] private InteractType type;
        public InteractType Type => type;

        /// <summary>
        /// Свойства действия
        /// </summary>
        private new ActionInteractProperties Properties;


        public override void SetProperties(EntityActionProperties prop)
        {
            Properties = (ActionInteractProperties) prop;
        }

        public override void StartAction()
        {
            if (IsCanStart == false) return;

            base.StartAction();

            owner.position = Properties.InteractTransform.position;
        }
    }
}