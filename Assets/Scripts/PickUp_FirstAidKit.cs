using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Подбираемая аптечка
    /// </summary>
    public class PickUp_FirstAidKit : TriggerInteractAction
    {
        protected override void OnEndAction(GameObject owner)
        {
            base.OnEndAction(owner);

            Destructible dest = owner.transform.root.GetComponent<Destructible>();

            if (dest != null)
            {
                dest.HealFull();
            }

            Destroy(gameObject);
        }

        protected override void ActionEnded()
        {
            base.ActionEnded();

            action.IsCanEnd = true;
        }
    }
}