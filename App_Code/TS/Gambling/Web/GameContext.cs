using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Core;

namespace TS.Gambling.Web
{

    /// <summary>
    /// Summary description for GameContext
    /// </summary>
    public class GameContext
    {
        public GameContext()
        {
        }

        public static CardGame GetCurrentGame()
        {
            return (CardGame) HttpContext.Current.Session[SessionKey.CURRENT_GAME];
        }

        public static void SetCurrentGame(CardGame game)
        {
            HttpContext.Current.Session[SessionKey.CURRENT_GAME] = game;
        }

        public static Player GetCurrentPlayer()
        {
            return (Player)HttpContext.Current.Session[SessionKey.CURRENT_PLAYER];
        }

        public static void SetCurrentPlayer(Player player)
        {
            HttpContext.Current.Session[SessionKey.CURRENT_PLAYER] = player;
        }

        public static int GetCurrentGameVersion()
        {
            if (HttpContext.Current.Session[SessionKey.CURRNET_GAME_VERSION] == null)
                return 0;
            return int.Parse(HttpContext.Current.Session[SessionKey.CURRNET_GAME_VERSION].ToString());
        }

        public static void SetCurrentGameVersion(int gameVersion)
        {
            HttpContext.Current.Session[SessionKey.CURRNET_GAME_VERSION] = gameVersion;
        }

        public static bool MustUpdateGame()
        {
            if (GetCurrentGame() == null || GetCurrentPlayer() == null)
                return false;
            return (GetCurrentGame().GameVersion > GetCurrentGameVersion() && GetCurrentPlayer().Events.Count == 0);
        }


    }

}