namespace Shooter3D
{
    /// <summary>
    /// Игровой персонаж
    /// </summary>
    public class SpaceSoldier : Destructible
    {
        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();
        }
    }
}