using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Дрон
    /// </summary>
    public class Drone : Destructible
    {
        [Header("Main")]
        /// <summary>
        /// Основной меш
        /// </summary>
        [SerializeField] private Transform mainMesh;
        public Transform MainMesh => mainMesh;

        /// <summary>
        /// Турели
        /// </summary>
        [SerializeField] private Weapon[] turrets;

        [Header("View")]
        /// <summary>
        /// Массив внутренних мешей
        /// </summary>
        [SerializeField] private GameObject[] meshComponents;
        /// <summary>
        /// Массив отображений мешей
        /// </summary>
        [SerializeField] private Renderer[] meshRenderers;
        /// <summary>
        /// Массив посмертных материалов
        /// </summary>
        [SerializeField] private Material[] deadMaterials;

        [Header("Movement")]
        /// <summary>
        /// Скорость перемещения
        /// </summary>
        [SerializeField] private float movementSpeed;
        /// <summary>
        /// Скорость вращения
        /// </summary>
        [SerializeField] private float rotationLerpFactor;
        /// <summary>
        /// Амплитуда парения
        /// </summary>
        [SerializeField] private float hoverAmplitude;
        /// <summary>
        /// Скорость парения
        /// </summary>
        [SerializeField] private float hoverSpeed;


        #region Unity Events

        private void Update()
        {
            Hover();
        }

        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();

            enabled = false;

            for (int i = 0; i < meshComponents.Length; i++)
            {
                if (meshComponents[i].GetComponent<Rigidbody>() == null)
                {
                    meshComponents[i].AddComponent<Rigidbody>();
                }
            }

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = deadMaterials[i];
            }
        }

        #endregion


        #region Public API

        /// <summary>
        /// Смотреть на цель
        /// </summary>
        /// <param name="target">Положение цели</param>
        public void LookAt(Vector3 target)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), Time.deltaTime * rotationLerpFactor);
        }

        /// <summary>
        /// Перемещение к цели
        /// </summary>
        /// <param name="target">Позиция цели</param>
        public void MoveTo(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
        }

        /// <summary>
        /// Стрельба по цели
        /// </summary>
        /// <param name="target">Положение цели</param>
        public void Fire(Vector3 target)
        {
            for (int i = 0; i < turrets.Length; i++)
            {
                turrets[i].FirePointLookAt(target);
                turrets[i].Fire();
            }
        }

        #endregion


        /// <summary>
        /// Парение на месте
        /// </summary>
        private void Hover()
        {
            mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime, 0);
        }
    }
}