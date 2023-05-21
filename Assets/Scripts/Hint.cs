using UnityEngine;

namespace Shooter3D
{
    [RequireComponent(typeof(Canvas))]

    /// <summary>
    /// Всплывающая подсказка
    /// </summary>
    public class Hint : MonoBehaviour
    {
        /// <summary>
        /// Подсказка
        /// </summary>
        [SerializeField] private GameObject hint;

        /// <summary>
        /// Радиус отображения
        /// </summary>
        [SerializeField] private float activeRadius;

        /// <summary>
        /// Канвас с подсказкой
        /// </summary>
        private Canvas canvas;
        /// <summary>
        /// Цель, активирующая подсказку
        /// </summary>
        private Transform target;
        /// <summary>
        /// Трансформа взгляда камеры
        /// </summary>
        private Transform lookTransform;


        #region Unity Events

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            lookTransform = Camera.main.transform;
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            hint.transform.LookAt(lookTransform);

            if (Vector3.Distance(transform.position, target.position) < activeRadius)
            {
                hint.SetActive(true);
            }
            else
            {
                hint.SetActive(false);
            }
        }

        #endregion


#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, activeRadius);
        }

#endif
    }
}