using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Bura;

namespace TS.Gambling.DataProviders
{

    /// <summary>
    /// Summary description for BuraGameProvider
    /// </summary>
    public class BuraGameListProvider
    {
        public BuraGameListProvider()
        {
        }

        public static List<BuraGameItem> GetBuraGamesList()
        {
            List<BuraGameItem> list = new List<BuraGameItem>();

            foreach (int gameId in BuraGameController.CurrentInstanse.BuraGames.Keys)
            {
                BuraGame game = BuraGameController.CurrentInstanse.BuraGames[gameId];
                BuraPlayer player1 = (BuraPlayer) game.Players.First().Value;
                BuraPlayer player2 = (game.Players.Count > 1) ? (BuraPlayer) game.Players.Skip(1).First().Value : null;
                BuraGameItem item = new BuraGameItem(
                    game.DBGameId, 
                    player1.PlayerName,
                    player2 != null ? player2.PlayerName : string.Empty,
                    game.Amount,
                    player2 == null ? "? : ?" : player1.Score + " : " + player2.Score,
                    game.PlayingTill,
                    player1.PlayerId,
                    (player2 == null ? -1 : player2.PlayerId)
                    );
                list.Add(item);
            }

            return list;
        }

    }


    public class BuraGameItem
    {

        public BuraGameItem(int gameId, string player1Name, string player2Name, double amount, string score, 
                int playingTill, int player1Id, int player2Id)
        {
            _gameId = gameId;
            _player1Name = player1Name;
            _player2Name = player2Name;
            _amount = amount;
            _score = score;
            _playingTill = playingTill;
            _player1Id = player1Id;
            _player2Id = player2Id;
        }

        private int _gameId;
        private string _player1Name;
        private string _player2Name;
        private double _amount;
        private string _score;
        private int _playingTill;
        private int _player1Id;
        private int _player2Id;

        public int Player2Id
        {
            get { return _player2Id; }
            set { _player2Id = value; }
        }

        public int Player1Id
        {
          get { return _player1Id; }
          set { _player1Id = value; }
        }

        public int PlayingTill
        {
            get { return _playingTill; }
            set { _playingTill = value; }
        }

        public string Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string Player2Name
        {
            get { return _player2Name; }
            set { _player2Name = value; }
        }

        public string Player1Name
        {
            get { return _player1Name; }
            set { _player1Name = value; }
        }

        public int GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }

    }

}