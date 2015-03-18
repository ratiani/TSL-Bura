using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Core
{

    public enum GameStatus
    {
        WaitingForOponent = 0,
        Playing,
        Finished
    }


    /// <summary>
    /// Summary description for Game
    /// </summary>
    public abstract class CardGame
    {
        public CardGame()
        {
            _players = new Dictionary<int, Player>();
            _doubling = GameDoubling.Items[0];
        }


        public abstract void ProcessEvent(int playerId, GameEvent gameEvent);
        public abstract void EndEvent(int playerId, int eventId);
        public abstract void RejectDoubling(int rejectorPlayerId);
        public abstract void SaveCurrentGameVersion(int currentVersionNumber);


        public void AddPlayerEvent(Player player, EventType type, object eventValue, int eventId)
        {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventId = eventId;
            gameEvent.Type = type;
            gameEvent.ViewerPlayerId = player.PlayerId;
            gameEvent.EventValue = eventValue;
            gameEvent.EventDate = DateTime.Now;
            player.Events[gameEvent.EventId] = gameEvent;

            // all events which were played has to be replayed
            foreach (int oldEventId in player.Events.Keys)
            {
                player.Events[oldEventId].EventPlayed = false;
            }

        }

        public void AddPlayerEvent(Player player, EventType type, object eventValue)
        {
            int eventId = IdGenerator.NextValue; ;
            AddPlayerEvent(player, type, eventValue, eventId);
        }

        /*
         * Offer Game Doubling to other players
         * @offererPlayerId - player which offers doubling
         * */
        public void DoublingOffer(int offererPlayerId)
        {
            if (_doubling.DoublingValue >= GameDoubling.MAX_DOUBLING_VALUE)
                throw new GamblingException("Wrong Doubling Value");
            if (offererPlayerId == _doublingOfferedBy)
                throw new GamblingException("Wrong Doubling Offerer");
            foreach (int playerId in _players.Keys)
            {
                if (playerId == offererPlayerId)
                    continue;
                
                // doubling and redoubling offer must be first in events
                AddPlayerEvent(_players[playerId], EventType.DoublingOffer, _doubling, -IdGenerator.NextValue);
            }
            _doublingOfferedBy = offererPlayerId;
            // offerer player must wait for opponent
            int waitingEventId = -IdGenerator.NextValue;
            AddPlayerEvent(_players[offererPlayerId], EventType.WaitForOpponent, "", waitingEventId);
            GameVersion++;
        }

        /*
         * Accpet Doubling offer
         * */
        public void AcceptDoubling(int playerId, int gameEventId)
        {
            if (_doubling.DoublingValue >= GameDoubling.MAX_DOUBLING_VALUE)
                throw new GamblingException("Wrong Doubling Value");
            if (! _players[playerId].Events.ContainsKey(gameEventId) || _players[playerId].Events[gameEventId].Type != EventType.DoublingOffer)
                throw new GamblingException("Wrong Doubling Event Id");
            _doubling = GameDoubling.Items[_doubling.DoublingValue];
            EndEvent(playerId, gameEventId);
            // remove "waiting for oponent" event
            foreach (int oponentId in _players.Keys)
            {
                Player oponent = _players[oponentId];
                if (oponent.Events != null && oponent.Events.Count > 0 && oponent.Events.First().Value.Type == EventType.WaitForOpponent)
                {
                    EndEvent(oponent.PlayerId, oponent.Events.First().Key);
                }
            }
            GameVersion++;
        }

        /*
         * Accept and send ReDoubling offer
         * */
        public void RedoubleOffer(int offererPlayerId, int gameEventId)
        {
            AcceptDoubling(offererPlayerId, gameEventId);
            DoublingOffer(offererPlayerId);
        }
             
        private Dictionary<int, Player> _players;
        private int _playerTurn;
        private HtmlBoard _board;
        private GameDoubling _doubling;
        private int _doublingOfferedBy;
        private int _gameVersion;
        private int _gameId;
        private GameStatus _status;
        private bool _isRematch;

        public bool IsRematch
        {
            get { return _isRematch; }
            set { _isRematch = value; }
        }

        public GameStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }

        public int GameVersion
        {
            get { return _gameVersion; }
            set 
            { 
                _gameVersion = value;
                SaveCurrentGameVersion(value);
            }
        }

        public int DoublingOfferedBy
        {
            get { return _doublingOfferedBy; }
            set { _doublingOfferedBy = value; }
        }

        public GameDoubling Doubling
        {
            get { return _doubling; }
            set { _doubling = value; }
        }

        public HtmlBoard Board
        {
            get { return _board; }
            set { _board = value; }
        }

        public int PlayerTurn
        {
            get { return _playerTurn; }
            set { _playerTurn = value; }
        }

        public Dictionary<int, Player> Players
        {
            get { return _players; }
            set { _players = value; }
        }


    }

}