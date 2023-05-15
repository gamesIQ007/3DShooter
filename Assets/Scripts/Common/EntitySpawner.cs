using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Спавнер объектов
    /// </summary>
    public class EntitySpawner : MonoBehaviour
    {
        /// <summary>
        /// Перечень режимов спавна
        /// </summary>
        public enum SpawnMode
        {
            /// <summary>
            /// При старте
            /// </summary>
            Start,
            /// <summary>
            /// В цикле
            /// </summary>
            Loop
        }

        /// <summary>
        /// Префаб объекта
        /// </summary>
        [SerializeField] private Entity[] entityPrefabs;

        /// <summary>
        /// Область спавна
        /// </summary>
        [SerializeField] private CubeArea area;

        /// <summary>
        /// Режим спавна
        /// </summary>
        [SerializeField] private SpawnMode spawnMode;

        /// <summary>
        /// Количество
        /// </summary>
        [SerializeField] private int numSpawns;

        /// <summary>
        /// Время респавна
        /// </summary>
        [SerializeField] private float respawnTime;

        /// <summary>
        /// Таймер
        /// </summary>
        private float timer;


        #region Unity Events

        private void Start()
        {
            if (spawnMode == SpawnMode.Start)
            {
                SpawnEntities();
            }

            timer = respawnTime;
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (spawnMode == SpawnMode.Loop && timer < 0)
            {
                SpawnEntities();

                timer = respawnTime;
            }
        }

        #endregion


        /// <summary>
        /// Заспавнить объекты
        /// </summary>
        private void SpawnEntities()
        {
            for (int i = 0; i < numSpawns; i++)
            {
                int index = Random.Range(0, entityPrefabs.Length);

                GameObject e = Instantiate(entityPrefabs[index].gameObject);
                e.transform.position = area.GetRandomInsideZone();
            }
        }
    }
}