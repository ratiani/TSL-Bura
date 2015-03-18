using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Core;
using TS.Gambling.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;


namespace TS.Gambling.Bura
{

    /// <summary>
    /// Summary description for BuraGameController
    /// </summary>
    public class BuraGameController
    {

        private static BuraGameController _instanse;

        public static BuraGameController CurrentInstanse
        {
            get 
            {
                if (_instanse == null)
                    _instanse = new BuraGameController();
                return _instanse; 
            }
        }

        private BuraGameController()
        {
            _games = new Dictionary<int, BuraGame>();
        }

        private Dictionary<int, BuraGame> _games;

        public void CreateGame(int gameId, int playerId, int playTill, double amount, bool longGameStyle, bool stickAllowed, bool passHiddenCards)
        {
            if (_games.ContainsKey(gameId))
                throw new GamblingException("GameID is busy");

            BuraPlayer player = new BuraPlayer();
            player.ClientId = playerId;
            player.PlayerName = "Name " + playerId;
            
            Stream imageStream = new MemoryStream();
            string path = HttpContext.Current.Server.MapPath("~/Skins/Default/Images/Common/img1.png");
            Bitmap image = new Bitmap(path); 
            image.Save(imageStream, ImageFormat.Png);

            // rewind the memory stream
            imageStream.Position = 0;

            player.Avatar = new byte[imageStream.Length];
            imageStream.Read(player.Avatar, 0, (int)imageStream.Length);

            BuraGame game = new BuraGame(player, playTill, amount, longGameStyle, stickAllowed, passHiddenCards);
            game.PlayerTurn = playerId;
            game.GameId = gameId;

            GameContext.SetCurrentGame(game);
            GameContext.SetCurrentPlayer(player);

            _games[gameId] = game;
        }

        public void JoinGame(int gameId, int playerId)
        {
            if (! _games.ContainsKey(gameId))
                throw new GamblingException("Game with GameID is not started");

            BuraPlayer player = new BuraPlayer();
            player.ClientId = playerId;
            player.PlayerName = "Name " + playerId;

            Stream imageStream = new MemoryStream();
            string path = HttpContext.Current.Server.MapPath("~/Skins/Default/Images/Common/img.png");
            Bitmap image = new Bitmap(path);
            image.Save(imageStream, ImageFormat.Png);
            // rewind the memory stream
            imageStream.Position = 0;
            player.Avatar = new byte[imageStream.Length];
            imageStream.Read(player.Avatar, 0, (int)imageStream.Length);

            _games[gameId].Players[playerId] = player;

            GameContext.SetCurrentGame(_games[gameId]);
            GameContext.SetCurrentPlayer(player);

            _games[gameId].StartGame();
        }

        public void RematchGame(int newGameId)
        {
            CardGame rematchGame = GetGame(newGameId);
            int playerId = GameContext.GetCurrentPlayer().PlayerId;
            if (rematchGame == null)
            {
                // create rematch game
                BuraGame buraGame = (BuraGame)GameContext.GetCurrentGame();
                int gameId = newGameId;
                
                int playTill = buraGame.PlayingTill;
                double amount = buraGame.Amount;
                bool longGameStyle = buraGame.LongGameStyle;
                bool stickAllowed = buraGame.StickAllowed;
                bool passHiddenCards = buraGame.PassHiddenCards;

                CreateGame(gameId, playerId, playTill, amount, longGameStyle, stickAllowed, passHiddenCards);
                rematchGame = GetGame(gameId);
                rematchGame.IsRematch = true;
            }
            else
            {
                // join to rematch game 
                JoinGame(newGameId, playerId);
            }
            GameContext.SetCurrentGame(rematchGame);
        }

        public BuraGame GetGame(int gameId)
        {
            if (_games.ContainsKey(gameId))
                return _games[gameId];
            else
                return null;
        }

        public Dictionary<int, BuraGame> BuraGames
        {
            get
            {
                return _games;
            }
        }

    }

}