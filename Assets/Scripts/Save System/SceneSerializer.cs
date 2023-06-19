using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Сохраняет и загружает объекты с интерфейсом
    /// </summary>
    public class SceneSerializer : MonoBehaviour
    {
        /// <summary>
        /// Состояние объектов сцены
        /// </summary>
        [System.Serializable]
        public class SceneObjectState
        {
            /// <summary>
            /// ID сцены
            /// </summary>
            public int SceneID;
            /// <summary>
            /// ID сущности
            /// </summary>
            public long EntityID;
            /// <summary>
            /// Состояние
            /// </summary>
            public string State;
        }


        /// <summary>
        /// База данных префабов
        /// </summary>
        [SerializeField] private PrefabDataBase prefabDataBase;


        // Чисто для тестов
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SaveToFile("test.dat");
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                LoadFromFile("test.dat");
            }
        }


        #region Public API

        /// <summary>
        /// Сохранить в файл
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void SaveToFile(string filePath)
        {
            List<SceneObjectState> savedObjects = new List<SceneObjectState>();

            // Получение всех сохраняемых объектов сцены
            foreach (var entity in FindObjectsOfType<Entity>())
            {
                ISerializableEntity serializableEntity = entity as ISerializableEntity;

                if (serializableEntity == null) continue;

                if (serializableEntity.IsSerializable() == false) continue;

                SceneObjectState s = new SceneObjectState();

                s.EntityID = serializableEntity.EntityId;
                s.State = serializableEntity.SerializableState();

                savedObjects.Add(s);
            }

            if (savedObjects.Count == 0)
            {
                Debug.Log("Нечего сохранять.");
                return;
            }

            // Сохранение в файл
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + filePath);

            bf.Serialize(file, savedObjects);
            file.Close();

            Debug.Log("Сохранено. Путь к файлу сохранения: " + Application.persistentDataPath + "/" + filePath);
        }

        /// <summary>
        /// Загрузить из файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void LoadFromFile(string filePath)
        {
            Player.Instance.Destroy();

            foreach (var entity in FindObjectsOfType<Entity>())
            {
                Destroy(entity.gameObject);
            }

            // Загружаем информацию об объектах
            List<SceneObjectState> loadedObjects = new List<SceneObjectState>();

            if (File.Exists(Application.persistentDataPath + "/" + filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + filePath, FileMode.Open);

                loadedObjects = (List<SceneObjectState>) bf.Deserialize(file);
                file.Close();
            }

            // Спавним объекты
            foreach (var v in loadedObjects)
            {
                GameObject g = prefabDataBase.CreateObjectFromID(v.EntityID);

                g.GetComponent<ISerializableEntity>().DeserializeState(v.State);
            }

            Debug.Log("Загружено из файла " + Application.persistentDataPath + "/" + filePath);
        }

        #endregion
    }
}