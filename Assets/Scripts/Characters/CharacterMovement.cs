using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Перемещение персонажа
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        /// <summary>
        /// Контроллер персонажа
        /// </summary>
        [SerializeField] private CharacterController characterController;

        [Header("Movement")]
        /// <summary>
        /// Скорость перемещение с винтовкой
        /// </summary>
        [SerializeField] private float rifleRunSpeed;
        /// <summary>
        /// Скорость бега с винтовкой
        /// </summary>
        [SerializeField] private float rifleSprintSpeed;
        /// <summary>
        /// Скорость перемещения при прицеливании
        /// </summary>
        [SerializeField] private float aimingWalkSpeed;
        /// <summary>
        /// Скорость бега при прицеливании
        /// </summary>
        [SerializeField] private float aimingRunSpeed;
        /// <summary>
        /// Коэффициент ускорения
        /// </summary>
        [SerializeField] private float accelerationRate;
        /// <summary>
        /// Скорость крадучести
        /// </summary>
        [SerializeField] private float crouchSpeed;

        /// <summary>
        /// Скорость прыжка
        /// </summary>
        [SerializeField] private float jumpSpeed;
        public float JumpSpeed => jumpSpeed;

        [Header("State")]
        /// <summary>
        /// Высота при крадучести
        /// </summary>
        [SerializeField] private float crouchHeight;
        public float CrouchHeight => crouchHeight;

        /// <summary>
        /// Прицеливается
        /// </summary>
        private bool isAiming;
        public bool IsAiming => isAiming;

        /// <summary>
        /// В прыжке
        /// </summary>
        private bool isJump;
        public bool IsJump => isJump;

        /// <summary>
        /// Крадётся
        /// </summary>
        private bool isCrouch;
        public bool IsCrouch => isCrouch;

        /// <summary>
        /// Бежит
        /// </summary>
        private bool isSprint;
        public bool IsSprint => isSprint;

        /// <summary>
        /// Расстояние до земли
        /// </summary>
        private float distanceToGround;
        public float DistanceToGround => distanceToGround;
        public bool IsGrounded => distanceToGround < 0.001f; // в обучении говорится про 0.01f, но с ним работает хуже

        // Public
        /// <summary>
        /// Вектор управления
        /// </summary>
        [HideInInspector] public Vector3 TargetDirectionControl;


        // Private
        /// <summary>
        /// Базовая высота персонажа
        /// </summary>
        private float BaseCharacterHeight;
        /// <summary>
        /// Базовое смещение высоты персонажа
        /// </summary>
        private float BaseCharacterHeightOffset;

        /// <summary>
        /// Вектор управления
        /// </summary>
        private Vector3 directionControl;
        /// <summary>
        /// Направление перемещения
        /// </summary>
        private Vector3 movementDirection;


        #region Unity Events

        private void Start()
        {
            BaseCharacterHeight = characterController.height;
            BaseCharacterHeightOffset = characterController.center.y;
        }

        private void Update()
        {
            Move();
            UpdateDistanceToGround();
        }

        #endregion


        #region Public API

        /// <summary>
        /// Прыжок
        /// </summary>
        public void Jump()
        {
            if (IsGrounded == false) return;
            if (isCrouch) return;

            isJump = true;
        }

        /// <summary>
        /// Присед
        /// </summary>
        public void Crouch()
        {
            if (IsGrounded == false) return;
            if (isSprint) return;
            isCrouch = true;
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0, characterController.center.y / 2, 0);
        }
        /// <summary>
        /// Поднятие из приседа
        /// </summary>
        public void UnCrouch()
        {
            isCrouch = false;
            characterController.height = BaseCharacterHeight;
            characterController.center = new Vector3(0, BaseCharacterHeightOffset, 0);
        }

        /// <summary>
        /// Бег
        /// </summary>
        public void Sprint()
        {
            if (IsGrounded == false) return;
            if (isCrouch) return;

            isSprint = true;
        }
        /// <summary>
        /// Прекращение бега
        /// </summary>
        public void UnSprint()
        {
            isSprint = false;
        }

        /// <summary>
        /// Прицеливание
        /// </summary>
        public void Aiming()
        {
            isAiming = true;
        }
        /// <summary>
        /// Прекращение прицеливания
        /// </summary>
        public void UnAiming()
        {
            isAiming = false;
        }

        /// <summary>
        /// Получение текущей скорости в зависимости от состояния
        /// </summary>
        /// <returns>Текущая скорость перемещения</returns>
        public float GetCurrentSpeedByState()
        {
            if (isCrouch) return crouchSpeed;

            if (isAiming)
            {
                if (isSprint) return aimingRunSpeed;
                else return aimingWalkSpeed;
            }

            if (isAiming == false)
            {
                if (isSprint) return rifleSprintSpeed;
                else return rifleRunSpeed;
            }

            return rifleRunSpeed;
        }

        #endregion


        /// <summary>
        /// Движение
        /// </summary>
        private void Move()
        {
            directionControl = Vector3.MoveTowards(directionControl, TargetDirectionControl, Time.deltaTime * accelerationRate);

            if (IsGrounded)
            {
                movementDirection = directionControl * GetCurrentSpeedByState();

                if (isJump == true)
                {
                    movementDirection.y = JumpSpeed;
                    isJump = false;
                }

                movementDirection = transform.TransformDirection(movementDirection);
            }

            movementDirection += Physics.gravity * Time.deltaTime;

            characterController.Move(movementDirection * Time.deltaTime);
        }

        /// <summary>
        /// Обновление дистанции до земли
        /// </summary>
        private void UpdateDistanceToGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000))
            {
                distanceToGround = hit.distance;
            }
        }
    }
}