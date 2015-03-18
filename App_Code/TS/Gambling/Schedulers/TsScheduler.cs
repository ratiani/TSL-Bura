using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Gambling.Schedulers
{
    
    /// <summary>
    /// Summary description for TsScheduler
    /// </summary>
    public class TsScheduler
    {

        private static TsScheduler _instance = null;

        public static void StartScheduler()
        {
            if (_instance == null)
            {
                _instance = new TsScheduler();
                _instance.Start();
            }
        }

        protected TsScheduler()
        {
        }

        protected const int UPDATE_TIME_IN_SECONDS = 1;

        public void Start()
        {
            // Create the timer callback delegate.
            System.Threading.TimerCallback cb = new System.Threading.TimerCallback(ProcessTimerEvent);

            // Create the timer. It is autostart, so creating the timer will start it.
            timer = new System.Threading.Timer(cb, String.Empty, 500, UPDATE_TIME_IN_SECONDS * 1000);
        }

        private System.Threading.Timer timer;

        protected virtual void ProcessTimerEvent(object obj)
        {
        }

    }

}