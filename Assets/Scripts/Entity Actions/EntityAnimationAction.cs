using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Анимационное действие
    /// </summary>
    public class EntityAnimationAction : EntityAction
    {
        /// <summary>
        /// Аниматор
        /// </summary>
        [SerializeField] private Animator animator;

        /// <summary>
        /// Название действия
        /// </summary>
        [SerializeField] private string actionAnimationName;

        /// <summary>
        /// Длительность перехода
        /// </summary>
        [SerializeField] private float timeDuration;

        /// <summary>
        /// Таймер
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Анимация проигрывается
        /// </summary>
        private bool isPlayingAnimation;


        #region Public API

        public override void StartAction()
        {
            base.StartAction();

            animator.CrossFade(actionAnimationName, timeDuration);

            timer = Timer.CreateTimer(timeDuration, true);
            timer.OnTick += OnTimerTick;
        }

        public override void EndAction()
        {
            base.EndAction();

            timer.OnTick -= OnTimerTick;
        }

        #endregion


        /// <summary>
        /// Тик таймера
        /// </summary>
        private void OnTimerTick()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(actionAnimationName))
            {
                isPlayingAnimation = true;
            }

            if (isPlayingAnimation)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(actionAnimationName) == false)
                {
                    isPlayingAnimation = false;

                    EndAction();
                }
            }
        }
    }
}