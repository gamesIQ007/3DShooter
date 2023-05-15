namespace Shooter3D
{
    /// <summary>
    /// Контекст действия
    /// </summary>
    public class EntityContextAction : EntityAnimationAction
    {
        /// <summary>
        /// Можно начать действие
        /// </summary>
        public bool IsCanStart;


        public override void StartAction()
        {
            if (IsCanStart == false) return;

            base.StartAction();
        }
    }
}