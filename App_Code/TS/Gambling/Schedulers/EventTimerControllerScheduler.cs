using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Bura;
using TS.Gambling.Core;
using System.Diagnostics;


namespace TS.Gambling.Schedulers
{

    /// <summary>
    /// Summary description for EventTimerControllerScheduler
    /// </summary>
    public class EventTimerControllerScheduler
    {

        private static EventTimerControllerScheduler _instance = null;

        public static void StartScheduler()
        {
            if (_instance == null)
            {
                _instance = new EventTimerControllerScheduler();
                _instance.Start();
            }
        }

        protected EventTimerControllerScheduler()
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

        protected void ProcessTimerEvent(object obj)
        {
            long currentTicks = DateTime.Now.Ticks;
            Dictionary<int, BuraGame> games = BuraGameController.CurrentInstanse.BuraGames;
            foreach (int gameId in games.Keys)
            {
                try
                {
                    foreach (int playerId in games[gameId].Players.Keys)
                    {
                        BuraPlayer player = (BuraPlayer)games[gameId].Players[playerId];
                        if (player.Events.Count > 0)
                        {
                            if (player.Events.First().Value.EventDate.Ticks + GameEvent.EVENT_TIME * TimeSpan.TicksPerSecond < currentTicks
                                && player.Events.First().Value.Type != EventType.WaitForOpponent)
                            {
                                // event has expired, game must be stopped
                                games[gameId].PlayerTimeout(player.PlayerId);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }


    }


}