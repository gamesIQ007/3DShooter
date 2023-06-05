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
        /// <summary>
        /// Можно закончить действие
        /// </summary>
        public bool IsCanEnd;


        public override void StartAction()
        {
            if (IsCanStart == false) return;

            IsCanStart = false;

            base.StartAction();
        }

        public override void EndAction()
        {
            if (IsCanEnd == false) return;

            IsCanEnd = false;

            base.EndAction();
        }
    }
}