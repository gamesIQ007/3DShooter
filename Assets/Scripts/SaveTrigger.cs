using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Триггер сохранения
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class SaveTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Player>() == null) return;

            SceneSerializer sceneSerializer = other.transform.root.GetComponent<SceneSerializer>();

            if (sceneSerializer == null) return;

            sceneSerializer.SaveScene();
        }
    }
}