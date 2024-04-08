﻿using GraphApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphApp.ViewModel
{
    public class EducationMaterialViewModel: ViewModel, INotifyPropertyChanged
    {
        private bool _needInvokeCallback;

        private TimeSpan _timerValue;

        private double _timerOpasity;

        public TimeSpan TimerValue 
        {
            get
            {
                return _timerValue;
            }
            set
            {
                _timerValue = value;
                OnPropertyChanged();
            }
        }

        public double TimerOpasity 
        {
            get
            {
                return _timerOpasity;
            }
            set
            {
                _timerOpasity = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual async void StartTimer(TimeSpan timerTime)
        {
            TimerOpasity = 1;
            TimerValue = timerTime;

            while (TimerValue.Seconds > 0)
            {
                await Task.Delay(1000);
                TimerValue -= new TimeSpan(0, 0, 1);
            }

            TimerOpasity = 0;
        }

        protected async void SetTimer(
            TimeSpan timerTime, 
            Action<object> callback, 
            object callbackParams = null)
        {
            _needInvokeCallback = true;
            await Task.Delay(timerTime);

            if (_needInvokeCallback)
            {
                callback.Invoke(callbackParams);
            }
        }

        protected void StopTimer()
        {
            _needInvokeCallback = false;
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
