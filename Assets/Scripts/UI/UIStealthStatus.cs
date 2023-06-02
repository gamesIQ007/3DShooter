using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter3D
{
    /// <summary>
    /// Статус видимости игрока
    /// </summary>
    public class UIStealthStatus : MonoBehaviour
    {
        /// <summary>
        /// Иконка видимости в боковом зрении
        /// </summary>
        [SerializeField] private Image imageCanSee;
        /// <summary>
        /// Иконка поиска
        /// </summary>
        [SerializeField] private Image imageSeek;
        /// <summary>
        /// Иконка обнаружения
        /// </summary>
        [SerializeField] private Image imageVisible;

        /// <summary>
        /// Перечень вражеских солдатов
        /// </summary>
        private AIAlienSoldier[] alienSoldiers;

        private bool canSee = false;
        private bool seek = false;
        private bool isVisible = false;


        private void Start()
        {
            imageCanSee.enabled = false;
            imageSeek.enabled = false;
            imageVisible.enabled = false;

            alienSoldiers = FindObjectsOfType<AIAlienSoldier>();
        }

        private void Update()
        {
            canSee = false;
            seek = false;
            isVisible = false;

            for (int i = 0; i < alienSoldiers.Length; i++)
            {
                if (alienSoldiers[i].enabled && alienSoldiers[i].AI_Behaviour == AIAlienSoldier.AIBehaviour.PursuitTarget)
                {
                    isVisible = true;
                    break;
                }
            }
            if (isVisible == false)
            {
                for (int i = 0; i < alienSoldiers.Length; i++)
                {
                    if (alienSoldiers[i].enabled && (alienSoldiers[i].AI_Behaviour == AIAlienSoldier.AIBehaviour.SeekTarget || alienSoldiers[i].AI_Behaviour == AIAlienSoldier.AIBehaviour.SeekTargetInArea))
                    {
                        seek = true;
                        break;
                    }
                }
            }
            if (isVisible == false && seek == false)
            {
                for (int i = 0; i < alienSoldiers.Length; i++)
                {
                    if (alienSoldiers[i].enabled && alienSoldiers[i].TargetInSideView)
                    {
                        canSee = true;
                        break;
                    }
                }
            }

            imageCanSee.enabled = canSee;
            imageSeek.enabled = seek;
            imageVisible.enabled = isVisible;
        }
    }
}