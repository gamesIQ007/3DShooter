using UnityEngine;

namespace Shooter3D
{
    public class ObjectMaterial : MonoBehaviour
    {
        public enum ObjectMaterials
        {
            Stone,
            Metal
        }

        public ObjectMaterials CurrentObjectMaterial;
    }
}