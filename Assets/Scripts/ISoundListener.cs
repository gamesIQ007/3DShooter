using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Интерфейс слушателя
    /// </summary>
    public interface ISoundListener
    {
        /// <summary>
        /// Услышать
        /// </summary>
        /// <param name="distance">Дистанция</param>
        void Heard(float distance);
    }
}