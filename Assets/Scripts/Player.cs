using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Игрок
    /// </summary>
    public class Player : SingletonBase<Player>
    {
        /// <summary>
        /// Индикатор обнаружения
        /// </summary>
        [SerializeField] private UIPlayerNotic uiPlayerNotic;

        /// <summary>
        /// Количество преследователей
        /// </summary>
        private int pursuitNumber;


        /// <summary>
        /// Начало преследования
        /// </summary>
        public void StartPursuit()
        {
            pursuitNumber++;

            uiPlayerNotic.Show();
        }

        /// <summary>
        /// Конец преследования
        /// </summary>
        public void StopPursuit()
        {
            pursuitNumber--;

            if (pursuitNumber == 0)
            {
                uiPlayerNotic.Hide();
            }
        }
    }
}