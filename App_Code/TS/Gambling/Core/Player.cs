using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for Player
    /// </summary>
    public class Player
    {

        public Player()
        {
            _events = new SortedDictionary<int, GameEvent>();
        }

        private string _playerName;
        private int _clientId;
        private SortedDictionary<int, GameEvent> _events;

        public SortedDictionary<int, GameEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public int PlayerId
        {
            get { return _clientId; }
        }

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
        }

        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; }
        }

    }

}