using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Забирание по лестнице
    /// </summary>
    public class ClimbingLadder : TriggerInteractAction
    {
        /// <summary>
        /// Верхняя точка лестницы
        /// </summary>
        [SerializeField] private Transform topPosition;
        /// <summary>
        /// Конечная точка перемещения
        /// </summary>
        [SerializeField] private Transform endPosition;

        /// <summary>
        /// Скорость перемещения по лестнице
        /// </summary>
        [SerializeField] private float clumbingSpeed;

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
        /// Позиции игрока и верхней точки совпадают
        /// </summary>
        private bool topPositionEquals = false;
        /// <summary>
        /// Позиции игрока и конечной точки совпадают
        /// </summary>
        private bool endPositionEquals = false;

        /// <summary>
        /// Действие стартовало
        /// </summary>
        private bool actionStarted = false;

        /// <summary>
        /// Анимация проигрывается
        /// </summary>
        private bool isPlayingAnimation;

        /// <summary>
        /// Игрок
        /// </summary>
        private GameObject player;

        private void Update()
        {
            if (actionStarted)
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

                        animator.Play(actionAnimationName);
                    }
                }
                
                if (topPositionEquals == false)
                {
                    player.transform.root.position = Vector3.MoveTowards(player.transform.root.position, topPosition.position, clumbingSpeed * Time.deltaTime);
                    if (player.transform.root.position == topPosition.position)
                    {
                        topPositionEquals = true;
                    }
                }
                if (topPositionEquals)
                {
                    player.transform.root.position = Vector3.MoveTowards(player.transform.root.position, endPosition.position, clumbingSpeed * Time.deltaTime);
                    if (player.transform.root.position == endPosition.position)
                    {
                        endPositionEquals = true;
                        OnStartAction(player);
                    }
                }
            }
        }

        protected override void OnStartAction(GameObject owner)
        {
            actionStarted = true;
            player = owner;
            owner.GetComponent<CharacterController>().enabled = false;
            
            //animator.CrossFade(actionAnimationName, timeDuration);
            
            if (endPositionEquals != true) return;

            owner.GetComponent<CharacterController>().enabled = true;

            base.OnEndAction(owner);

            actionStarted = false;
            topPositionEquals = false;
            endPositionEquals = false;
        }
    }
}