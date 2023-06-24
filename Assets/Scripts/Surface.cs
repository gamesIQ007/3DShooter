using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Поверхность
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Surface : MonoBehaviour
    {
        /// <summary>
        /// Тип эффекта
        /// </summary>
        [SerializeField] private ImpactType impactType;
        public ImpactType Type => impactType;


        /// <summary>
        /// Добавить ко всем объектам
        /// </summary>
        [ContextMenu("AddToAllObjects")]
        public void AddToAllObjects()
        {
            Transform[] allTransforms = FindObjectsOfType<Transform>();

            for (int i = 0; i < allTransforms.Length; i++)
            {
                if (allTransforms[i].GetComponent<Collider>() != null)
                {
                    if (allTransforms[i].GetComponent<Surface>() == null)
                    {
                        allTransforms[i].gameObject.AddComponent<Surface>();
                    }
                }
            }
        }
    }
}