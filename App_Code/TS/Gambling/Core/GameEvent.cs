using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Core
{

    public enum EventType
    {
        DealCards,
        AddCard,
        PlaceCard,
        TakeCard,
        DoublingOffer,
        ContinueQuestion,
        PlayerTurn,
        ShowCards,
        WinDeal,
        LooseDeal,
        ContinueGame,
        WinAndRematchOffer,
        LooseAndRematchOffer,
        WaitForOpponent,
        TimeoutWin,
        TimeoutLoose
    }

    /// <summary>
    /// Summary description for GameEvent
    /// </summary>
    public class GameEvent
    {

        public const int EVENT_TIME = 60;

        public GameEvent()
        {
        }

        private int _eventId;
        private EventType _type;
        private int _viewerPlayerId;
        private object _eventValue;
        private bool _eventPlayed;
        private DateTime _eventDate;

        public DateTime EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; }
        }

        public bool EventPlayed
        {
            get { return _eventPlayed; }
            set { 
                if (_type != EventType.WaitForOpponent)
                    _eventPlayed = value;
            }
        }

        public object EventValue
        {
            get { return _eventValue; }
            set { _eventValue = value; }
        }

        public int ViewerPlayerId
        {
            get { return _viewerPlayerId; }
            set { _viewerPlayerId = value; }
        }

        public EventType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int EventId
        {
            get { return _eventId; }
            set { _eventId = value; }
        }

    }

}