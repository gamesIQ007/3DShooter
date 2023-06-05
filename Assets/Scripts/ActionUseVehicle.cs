using UnityEngine;

namespace Shooter3D
{
    [System.Serializable]
    /// <summary>
    /// Свойства использования транспорта
    /// </summary>
    public class ActionUseVehiclesProperties : ActionInteractProperties
    {
        /// <summary>
        /// Транспорт
        /// </summary>
        public Vehicle vehicle;

        /// <summary>
        /// Управление транспортом
        /// </summary>
        public VehicleInputControl vehicleInput;

        /// <summary>
        /// Подсказка
        /// </summary>
        public GameObject hint;
    }


    /// <summary>
    /// Действие использования транспорта
    /// </summary>
    public class ActionUseVehicle : ActionInteract
    {
        [Header("Action Components")]
        /// <summary>
        /// Контроллер персонажа
        /// </summary>
        [SerializeField] private CharacterController characterController;

        /// <summary>
        /// Перемещение персонажа
        /// </summary>
        [SerializeField] private CharacterMovement characterMovement;

        /// <summary>
        /// Управление вводом персонажа
        /// </summary>
        [SerializeField] private CharacterInputController characterInputController;

        /// <summary>
        /// Визуальная модель игрока
        /// </summary>
        [SerializeField] private GameObject visualModel;

        /// <summary>
        /// Камера
        /// </summary>
        [SerializeField] private ThirdPersonCamera thirdPersonCamera;

        /// <summary>
        /// В транспорте
        /// </summary>
        private bool inVehicle;

        #region Unity Events

        private void Start()
        {
            EventOnStart.AddListener(OnActionStarted);
            EventOnEnd.AddListener(OnActionEnded);
        }

        private void OnDestroy()
        {
            EventOnStart.RemoveListener(OnActionStarted);
            EventOnEnd.RemoveListener(OnActionEnded);
        }

        private void Update()
        {
            if (inVehicle)
            {
                IsCanEnd = (Properties as ActionUseVehiclesProperties).vehicle.LinearVelocity < 2;
                (Properties as ActionUseVehiclesProperties).hint.SetActive(IsCanEnd);
            }
        }

        #endregion


        /// <summary>
        /// При начале действия
        /// </summary>
        private void OnActionStarted()
        {
            ActionUseVehiclesProperties prop = Properties as ActionUseVehiclesProperties;

            inVehicle = true;

            // Camera
            prop.vehicleInput.AssignCamera(thirdPersonCamera);

            // Vehicle Input
            prop.vehicleInput.enabled = true;

            // Character
            characterInputController.enabled = false;
            characterController.enabled = false;
            characterMovement.enabled = false;

            // Hide visual model
            visualModel.transform.localPosition = visualModel.transform.localPosition + new Vector3(0, 100000, 0);
        }

        /// <summary>
        /// При Завершении действия
        /// </summary>
        private void OnActionEnded()
        {
            ActionUseVehiclesProperties prop = Properties as ActionUseVehiclesProperties;

            inVehicle = false;

            // Camera
            characterInputController.AssignCamera(thirdPersonCamera);

            // Vehicle Input
            prop.vehicleInput.enabled = false;

            // Character
            owner.position = prop.InteractTransform.position;
            characterInputController.enabled = true;
            characterController.enabled = true;
            characterMovement.enabled = true;

            // Show visual model
            visualModel.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}