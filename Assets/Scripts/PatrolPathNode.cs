using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Точка маршрута патрулирования
    /// </summary>
    public class PatrolPathNode : MonoBehaviour
    {
        /// <summary>
        /// Время ожидания
        /// </summary>
        [SerializeField] private float idleTime;
        public float IdleTime => idleTime;


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
#endif
    }
}