using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Интерфейс сериализуемой сущности (что будет сохраняться/загружаться)
    /// </summary>
    public interface ISerializableEntity
    {
        /// <summary>
        /// ID сущности
        /// </summary>
        long EntityId { get; }

        /// <summary>
        /// Может ли быть сериализован
        /// </summary>
        /// <returns>Может ли быть сериализован</returns>
        bool IsSerializable();
        /// <summary>
        /// Сериализовать состояние
        /// </summary>
        /// <returns>Сериализованная строка</returns>
        string SerializableState();
        /// <summary>
        /// Десериализовать состояние
        /// </summary>
        /// <param name="state">Сериализованная строка</param>
        void DeserializeState(string state);
    }
}