using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Web
{

    /// <summary>
    /// Summary description for SessionKey
    /// </summary>
    public class SessionKey
    {

        public const string CURRENT_GAME = "GAMBLING_CURRENT_GAME";
        public const string CURRENT_PLAYER = "GAMBLING_CURRENT_PLAYER";
        public const string SELECTED_CARDS = "GAMBLING_SELECTED_CARDS";
        public const string CURRNET_GAME_VERSION = "GAMBLING_CURRENT_GAME_VERSION";
        public const string VIEW_BURA_GAME_ID = "GAMBLING_VIEW_BURA_GAME_ID";
        public const string VIEW_BURA_GAME = "GAMBLING_VIEW_BURA_GAME";
        public const string VIEW_BURA_GAME_VERSION = "GAMBLING_VIEW_BURA_GAME_VERSION";

        public SessionKey()
        {
        }
    }

}