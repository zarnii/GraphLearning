using GraphApp.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GraphApp.Services
{
    /// <summary>
    /// Таймер с вызовом колюэка.
    /// </summary>
    public class Timer
    {
        #region fields
        /// <summary>
        /// Флаг, показывающий нужно ли вызывать колбэк.
        /// </summary>
        private bool _timerIsStopped;

        /// <summary>
        /// Значение таймера.
        /// </summary>
        private TimeSpan _timerValue;

        /// <summary>
        /// Время, на которое заведен таймер.
        /// </summary>
        private TimeSpan _timerTime;

        /// <summary>
        /// Колбэк.
        /// </summary>
        private Action<object> _callback;

        /// <summary>
        /// Параметры колбэка.
        /// </summary>
        private object _callbackParam;

        /// <summary>
        /// Колбэк для кажжого тика таймера.
        /// </summary>
        private Action<object> _onEachTimerTick;

        /// <summary>
        /// Параметры колбэка каждого тика таймера.
        /// </summary>
        private object _onEachTimerTickParam;
        #endregion

        #region properties
        /// <summary>
        /// Значение таймера.
        /// </summary>
        public TimeSpan TimerValue
        {
            get 
            { 
                return _timerValue; 
            }
        }

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="timerTime">Время, на которое заводиться таймер.</param>
        /// <param name="callback">Колбэк, который выполниться после таймера.</param>
        /// <param name="callbackParam">Параметры колбэка.</param>
        /// <param name="onEachTimerTick">Колбэк каждого тика таймера.</param>
        /// <param name="onEachTimerTickParam">Параметры колбэка каждого тика таймера.</param>
        public Timer(
            TimeSpan timerTime,
            Action<object> callback = null,
            object callbackParam = null,
            Action<object> onEachTimerTick = null,
            object onEachTimerTickParam = null)
        {
            _timerTime = timerTime;
            _callback = callback;
            _callbackParam = callbackParam;
            _onEachTimerTick = onEachTimerTick;
            _onEachTimerTickParam = onEachTimerTickParam;
        }
        #endregion

        /// <summary>
        /// Запуск таймера.
        /// </summary>
        public async void Start()
        {
            _timerValue = _timerTime;
            _timerIsStopped = false;
            var stopTime = new TimeSpan(0, 0, 0, 0, 0);
            var subtrahendTime = new TimeSpan(0, 0, 1);

            while (_timerValue > stopTime)
            {
                if (_timerIsStopped)
                {
                    return;
                }

                _onEachTimerTick?.Invoke(_onEachTimerTickParam);
                await Task.Delay(1000);
                _timerValue -= subtrahendTime;
            }

            _callback?.Invoke(_callbackParam);
        }

        /// <summary>
        /// Остановка таймера.
        /// </summary>
        public void Stop()
        {
            _timerIsStopped = true;
        }
    }
}
