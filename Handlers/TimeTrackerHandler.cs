﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLifeBalance.HandlerClasses;

namespace WorkLifeBalance.Handlers
{
    public class TimeTrackerHandler
    {
        private static TimeTrackerHandler? _instance;
        public static TimeTrackerHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimeTrackerHandler();
                }
                return _instance;
            }
        }
        
        public delegate void SpentTimeEvent();

        public event SpentTimeEvent? OnSpentTimeChange;

        private TimeSpan OneSec = new TimeSpan(0, 0, 1);

        public void UpdateSpentTime()
        {
            switch (DataHandler.Instance.AppTimmerState)
            {
                case TimmerState.Working:
                    DataHandler.Instance.TodayData.WorkedAmmountC = DataHandler.Instance.TodayData.WorkedAmmountC.Add(OneSec);
                    break;

                case TimmerState.Resting:
                    DataHandler.Instance.TodayData.RestedAmmountC = DataHandler.Instance.TodayData.RestedAmmountC.Add(OneSec);
                    break;
            }
            OnSpentTimeChange?.Invoke();

            //temp
            try
            {
                IntPtr foregroundWindowHandle = WindowOptionsHelper.GetForegroundWindow();
                string applicationName = WindowOptionsHelper.GetApplicationName(foregroundWindowHandle);

                Console.WriteLine($"Application: {applicationName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //temp
        }
    }


    public enum TimmerState
    {
        Working,
        Resting
    }
}