using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Квест на достижение области
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class QuestReachedTrigger : Quest
    {
        /// <summary>
        /// Кто выполняет квест
        /// </summary>
        [SerializeField] private GameObject owner;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != owner) return;

            Completed?.Invoke();
        }
    }
}