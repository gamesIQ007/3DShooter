using UnityEngine;
using UnityEngine.Events;

namespace Shooter3D
{
    /// <summary>
    /// Таймер
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// Коллектор таймеров
        /// </summary>
        private static GameObject timerCollector;

        /// <summary>
        /// Событие о завершении работы таймера
        /// </summary>
        public event UnityAction OnTimeRunOut;
        /// <summary>
        /// Событие тика таймера
        /// </summary>
        public event UnityAction OnTick;

        /// <summary>
        /// Зацикленный
        /// </summary>
        public bool IsLoop;

        /// <summary>
        /// Максимальное время
        /// </summary>
        private float maxTime;
        public float MaxTime => maxTime;

        /// <summary>
        /// Текущее время
        /// </summary>
        private float currentTime;
        public float CurrentTime => currentTime;

        /// <summary>
        /// Пауза
        /// </summary>
        private bool isPause;
        public bool IsPause => isPause;

        /// <summary>
        /// Завершённый
        /// </summary>
        public bool IsCompleted => currentTime <= 0;


        private void Update()
        {
            if (isPause) return;

            currentTime -= Time.deltaTime;

            OnTick?.Invoke();

            if (currentTime <= 0)
            {
                currentTime = 0;

                OnTimeRunOut?.Invoke();

                if (IsLoop)
                {
                    currentTime = maxTime;
                }
            }
        }


        #region Public API

        /// <summary>
        /// Создание таймера
        /// </summary>
        /// <param name="time">Время</param>
        /// <param name="isLoop">Зацикленность</param>
        /// <returns>Таймер</returns>
        public static Timer CreateTimer(float time, bool isLoop)
        {
            if (timerCollector == null)
            {
                timerCollector = new GameObject("Timers");
            }

            Timer timer = timerCollector.AddComponent<Timer>();
            timer.maxTime = time;
            timer.IsLoop = isLoop;

            return timer;
        }


        /// <summary>
        /// Создание таймера
        /// </summary>
        /// <param name="time">Время</param>
        /// <returns>Таймер</returns>
        public static Timer CreateTimer(float time)
        {
            if (timerCollector == null)
            {
                timerCollector = new GameObject("Timers");
            }

            Timer timer = timerCollector.AddComponent<Timer>();
            timer.maxTime = time;

            return timer;
        }

        /// <summary>
        /// Запустить
        /// </summary>
        public void Play()
        {
            isPause = false;
        }

        /// <summary>
        /// Приостановить
        /// </summary>
        public void Pause()
        {
            isPause = true;
        }

        /// <summary>
        /// Остановить
        /// </summary>
        public void Completed()
        {
            isPause = false;
            currentTime = 0;
        }

        /// <summary>
        /// Прекратить не вызывая событий
        /// </summary>
        public void CompletedWithoutEvent()
        {
            Destroy(this);
        }

        /// <summary>
        /// Перезапустить
        /// </summary>
        /// <param name="time">Время</param>
        public void Restart(float time)
        {
            maxTime = time;
            currentTime = maxTime;
        }

        /// <summary>
        /// Перезапустить
        /// </summary>
        public void Restart()
        {
            currentTime = maxTime;
        }

        #endregion
    }
}