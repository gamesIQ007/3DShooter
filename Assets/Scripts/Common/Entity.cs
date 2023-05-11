using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Игровая сущность
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// Имя сущности
        /// </summary>
        [SerializeField] private string nickname;
        public string Nickname => nickname;
    }
}
