using UnityEngine;

namespace Shooter3D
{
    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            Loop
        }

        [SerializeField] private Entity[] entityPrefabs;

        [SerializeField] private CubeArea area;

        [SerializeField] private SpawnMode spawnMode;

        [SerializeField] private int numSpawns;

        [SerializeField] private float respawnTime;

        private float timer;

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