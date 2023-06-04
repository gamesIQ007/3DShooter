using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Нацеливание объекта на цель
    /// </summary>
    public class LookAt : MonoBehaviour
    {
        /// <summary>
        /// Цель
        /// </summary>
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.LookAt(target);
        }
    }
}