using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Солдат пришельцев
    /// </summary>
    public class AlienSoldier : Destructible
    {
        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();
        }
    }
}