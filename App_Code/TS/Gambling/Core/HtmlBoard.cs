using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Web;
using System.Text;

namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for Board
    /// </summary>
    public class HtmlBoard
    {

        public const int CARD_HEIGHT = 111;
        public const int CARD_WIDTH = 81;

        public HtmlBoard()
        {
        }

        protected string GetImageUrl(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return string.Empty;
            return string.Format("{0}{1}", VirtualPathUtility.ToAbsolute("~/Skins/NewDesign/Images/"), imageName);
        }

        protected string GetCardImage(Card card)
        {
            if (card == null)
                return string.Empty;
            return string.Format("{0}{1}.png", VirtualPathUtility.ToAbsolute("~/Skins/NewDesign/Images/Cards/"), card.Name);
        }

        protected string GetHiddenCardImage()
        {
            return string.Format("{0}Cover.png", VirtualPathUtility.ToAbsolute("~/Skins/NewDesign/Images/Cards/"));
        }

        private CardGame _game;

        public CardGame Game
        {
            get { return _game; }
            set { _game = value; }
        }

    }

}