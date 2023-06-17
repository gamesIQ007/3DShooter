using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Квест на убийства
    /// </summary>
    public class QuestKillDestractible : Quest
    {
        /// <summary>
        /// Список уничтожаемых сущностей
        /// </summary>
        [SerializeField] private Destructible[] destructibles;

        private int amountDestractibleDead = 0;


        #region Unity Events

        private void Start()
        {
            if (destructibles == null) return;

            for (int i = 0; i < destructibles.Length; i++)
            {
                destructibles[i].EventOnDeath.AddListener(OnDestractibleDead);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < destructibles.Length; i++)
            {
                destructibles[i].EventOnDeath.RemoveListener(OnDestractibleDead);
            }
        }

        #endregion


        /// <summary>
        /// При смерти дестрактибла
        /// </summary>
        private void OnDestractibleDead()
        {
            amountDestractibleDead++;

            if (amountDestractibleDead >= destructibles.Length)
            {
                for (int i = 0; i < destructibles.Length; i++)
                {
                    if (destructibles[i] != null)
                    {
                        destructibles[i].EventOnDeath.RemoveListener(OnDestractibleDead);
                    }
                }

                Completed?.Invoke();
            }
        }
    }
}