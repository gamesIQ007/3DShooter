using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Материал объекта (внутриигровой, задающий поведение)
    /// </summary>
    public class ObjectMaterial : MonoBehaviour
    {
        /// <summary>
        /// Перечень материалов объектов
        /// </summary>
        public enum ObjectMaterials
        {
            /// <summary>
            /// Камень
            /// </summary>
            Stone,
            /// <summary>
            /// Металл
            /// </summary>
            Metal
        }

        /// <summary>
        /// Текущий материал объекта
        /// </summary>
        [SerializeField] private ObjectMaterials currentObjectMaterial;
        public ObjectMaterials CurrentObjectMaterial => currentObjectMaterial;
    }
}