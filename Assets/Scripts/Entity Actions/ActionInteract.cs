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
        EnteringCode,
        /// <summary>
        /// Забирание по лестнице
        /// </summary>
        ClimbingLadder
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
        /// Скорость перемещения к точке взаимодействия
        /// </summary>
        [SerializeField] private float moveToActionInteractTransformSpeed;

        /// <summary>
        /// Тип взаимодействия
        /// </summary>
        [SerializeField] private InteractType type;
        public InteractType Type => type;

        /// <summary>
        /// Свойства действия
        /// </summary>
        private new ActionInteractProperties Properties;

        /// <summary>
        /// Позиции игрока и точки взаимодействия совпадают
        /// </summary>
        private bool positionEquals = false;

        /// <summary>
        /// Действие стартовало
        /// </summary>
        private bool actionStarted = false;


        private void Update()
        {
            if (actionStarted)
            {
                owner.position = Vector3.MoveTowards(owner.position, Properties.InteractTransform.position, moveToActionInteractTransformSpeed * Time.deltaTime);
                if (owner.position == Properties.InteractTransform.position)
                {
                    positionEquals = true;
                    StartAction();
                }
            }
        }


        public override void SetProperties(EntityActionProperties prop)
        {
            Properties = (ActionInteractProperties) prop;
        }

        public override void StartAction()
        {
            if (IsCanStart == false) return;

            actionStarted = true;
            owner.GetComponent<CharacterController>().enabled = false;

            if (positionEquals != true) return;

            owner.GetComponent<CharacterController>().enabled = true;

            base.StartAction();

            actionStarted = false;
            positionEquals = false;
        }
    }
}