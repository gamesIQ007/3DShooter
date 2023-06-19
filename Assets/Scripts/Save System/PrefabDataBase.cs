using System.Collections.Generic;
using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// База данных префабов
    /// </summary>
    [CreateAssetMenu]
    public class PrefabDataBase : ScriptableObject
    {
        /// <summary>
        /// Список всех префабов
        /// </summary>
        public List<Entity> AllPrefabs;


        /// <summary>
        /// Создать объект по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Созданный объект</returns>
        public GameObject CreateObjectFromID(long id)
        {
            foreach (var entity in AllPrefabs)
            {
                if (entity is ISerializableEntity == false) continue;

                if ((entity as ISerializableEntity).EntityId == id)
                {
                    return Instantiate(entity.gameObject);
                }
            }

            return null;
        }
    }
}