using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Свойства квеста
    /// </summary>
    [CreateAssetMenu]
    public class QuestProperties : ScriptableObject
    {
        /// <summary>
        /// Описание
        /// </summary>
        [TextArea]
        [SerializeField] private string description;
        public string Description => description;

        /// <summary>
        /// Задание
        /// </summary>
        [TextArea]
        [SerializeField] private string task;
        public string Task => task;
    }
}