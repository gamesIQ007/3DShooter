using UnityEngine;

namespace Shooter3D
{
    [System.Serializable]
    /// <summary>
    /// Класс с именами параметров аниматора
    /// </summary>
    public class CharacterAnimatorParametersName
    {
        /// <summary>
        /// Нормализованное перемещение по оси X
        /// </summary>
        public string NormalizeMovementX;
        /// <summary>
        /// Нормализованное перемещение по оси Z
        /// </summary>
        public string NormalizeMovementZ;
        /// <summary>
        /// Спринт
        /// </summary>
        public string Sprint;
        /// <summary>
        /// Крадучесть
        /// </summary>
        public string Croach;
        /// <summary>
        /// Прицеливание
        /// </summary>
        public string Aiming;
        /// <summary>
        /// На земле
        /// </summary>
        public string Ground;
        /// <summary>
        /// Прыжок
        /// </summary>
        public string Jump;
        /// <summary>
        /// Скорость перемещения по земле
        /// </summary>
        public string GroundSpeed;
        /// <summary>
        /// Расстояние до земли
        /// </summary>
        public string DistanceToGround;
    }

    [System.Serializable]
    /// <summary>
    /// Параметры перехода анимации
    /// </summary>
    public class AnimationCrossFadeParameters
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name;
        /// <summary>
        /// Длительность
        /// </summary>
        public float Duration;
    }

    public class CharacterAnimationState : MonoBehaviour
    {
        /// <summary>
        /// Постоянная плавного контроля
        /// </summary>
        private const float INPUT_CONTROL_LERP_RATE = 10.0f;

        /// <summary>
        /// Контроллер персонажа
        /// </summary>
        [SerializeField] private CharacterController targetCharacterController;
        /// <summary>
        /// Класс, контролирующий перемещение персонажа
        /// </summary>
        [SerializeField] private CharacterMovement targetCharacterMovement;

        /// <summary>
        /// Имена параметров аниматора
        /// </summary>
        [SerializeField] [Space(5)] private CharacterAnimatorParametersName animatorParametersName;

        /// <summary>
        /// Аниматор
        /// </summary>
        [SerializeField] private Animator targetAnimator;
        
        [Header("Fades")] [Space(5)]
        /// <summary>
        /// Состояние падения
        /// </summary>
        [SerializeField] private AnimationCrossFadeParameters fallFade;
        /// <summary>
        /// Минимальная дистанция до земли для падения
        /// </summary>
        [SerializeField] private float minDistanceToGroundByFall;

        /// <summary>
        /// Состояние прыжка на месте
        /// </summary>
        [SerializeField] private AnimationCrossFadeParameters jumpIdleFade;

        /// <summary>
        /// Состояние прыжка в движении
        /// </summary>
        [SerializeField] private AnimationCrossFadeParameters jumpMoveFade;

        /// <summary>
        /// Вектор управления
        /// </summary>
        private Vector3 inputControl;


        private void Update()
        {
            Vector3 movementSpeed = transform.InverseTransformDirection(targetCharacterController.velocity);

            // Чтобы не было дёрганья модели при движении камеры по горизонтали
            inputControl = Vector3.MoveTowards(inputControl, targetCharacterMovement.TargetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

            targetAnimator.SetFloat(animatorParametersName.NormalizeMovementX, inputControl.x);
            targetAnimator.SetFloat(animatorParametersName.NormalizeMovementZ, inputControl.z);

            targetAnimator.SetBool(animatorParametersName.Ground, targetCharacterMovement.IsGrounded);
            targetAnimator.SetBool(animatorParametersName.Croach, targetCharacterMovement.IsCrouch);
            targetAnimator.SetBool(animatorParametersName.Aiming, targetCharacterMovement.IsAiming);
            targetAnimator.SetBool(animatorParametersName.Sprint, targetCharacterMovement.IsSprint);

            Vector3 groundSpeed = movementSpeed;
            groundSpeed.y = 0;
            targetAnimator.SetFloat(animatorParametersName.GroundSpeed, groundSpeed.magnitude);

            if (targetCharacterMovement.IsJump)
            {
                if (groundSpeed.magnitude <= 0.01f)
                {
                    CrossFade(jumpIdleFade);
                }
                if (groundSpeed.magnitude > 0.01f)
                {
                    CrossFade(jumpMoveFade);
                }
            }

            if (targetCharacterMovement.IsGrounded == false)
            {
                targetAnimator.SetFloat(animatorParametersName.Jump, movementSpeed.y);

                if (movementSpeed.y < 0 && targetCharacterMovement.DistanceToGround > minDistanceToGroundByFall)
                {
                    CrossFade(fallFade);
                }
            }

            targetAnimator.SetFloat(animatorParametersName.DistanceToGround, targetCharacterMovement.DistanceToGround);
        }


        /// <summary>
        /// Переход состояния
        /// </summary>
        /// <param name="parameters">Параметры перехода состояния</param>
        private void CrossFade(AnimationCrossFadeParameters parameters)
        {
            targetAnimator.CrossFade(parameters.Name, parameters.Duration);
        }
    }
}