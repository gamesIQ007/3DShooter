using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Смотрящий на коллайдеры
    /// </summary>
    public class ColliderViewer : MonoBehaviour
    {
        /// <summary>
        /// Угол обзора
        /// </summary>
        [SerializeField] private float viewingAngle;

        /// <summary>
        /// Дальность обзора
        /// </summary>
        [SerializeField] private float viewingDistance;
        
        /// <summary>
        /// Высота обзора (на каком уровне глаза)
        /// </summary>
        [SerializeField] private float viewingHeight;


        /// <summary>
        /// Виден ли объект
        /// </summary>
        /// <param name="target">Объект</param>
        /// <returns>Виден ли объект</returns>
        public bool IsObjectVisible(GameObject target)
        {
            ColliderViewpoints viewpoints = target.GetComponent<ColliderViewpoints>();

            if (viewpoints == false) return false;

            return viewpoints.IsVisibleFromPoint(transform.position + new Vector3(0, viewingHeight, 0), transform.forward, viewingAngle, viewingDistance);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, viewingHeight, 0), transform.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, viewingAngle, viewingDistance, 0, 1);
        }
#endif
    }
}