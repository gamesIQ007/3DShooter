using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Путь патрулирования
    /// </summary>
    public class PatrolPath : MonoBehaviour
    {
        /// <summary>
        /// Массив точек патрулирования
        /// </summary>
        [SerializeField] private PatrolPathNode[] nodes;


        private void Start()
        {
            UpdatePathNode();
        }


        #region Public API

        /// <summary>
        /// Получить случайную точку патрулирования
        /// </summary>
        /// <returns>Случайная точка патрулирования</returns>
        public PatrolPathNode GetRandomPathNode()
        {
            return nodes[Random.Range(0, nodes.Length)];
        }

        /// <summary>
        /// Получить следующую точку патрулирования
        /// </summary>
        /// <param name="index">Индекс точки патрулирования</param>
        /// <returns>Следующая точка патрулирования</returns>
        public PatrolPathNode GetNextNode(ref int index)
        {
            index = Mathf.Clamp(index, 0, nodes.Length - 1);

            index++;

            if (index >= nodes.Length)
            {
                index = 0;
            }

            return nodes[index];
        }

        #endregion


        [ContextMenu("Update Path Node")]
        /// <summary>
        /// Обновить массив путевых точек
        /// </summary>
        private void UpdatePathNode()
        {
            nodes = new PatrolPathNode[transform.childCount];

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (nodes == null) return;

            Gizmos.color = Color.red;
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                Gizmos.DrawLine(nodes[i].transform.position + new Vector3(0, 0.5f, 0), nodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
            }

            Gizmos.DrawLine(nodes[0].transform.position + new Vector3(0, 0.5f, 0), nodes[nodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
        }
#endif
    }
}