using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for GameDoubling
    /// </summary>
    public class GameDoubling
    {

        public const int MAX_DOUBLING_VALUE = 6;

        public static GameDoubling[] Items = new GameDoubling[6] {
            new GameDoubling(1, ""),
            new GameDoubling(2, "დავი"),
            new GameDoubling(3, "სე"),
            new GameDoubling(4, "ჩარი"),
            new GameDoubling(5, "ფანჯი"),
            new GameDoubling(6, "შაში")
        };

        protected GameDoubling(int doublingValue, string doublingText)
        {
            _doublingValue = doublingValue;
            _doublingText = doublingText;
        }

        private int _doublingValue;
        private string _doublingText;

        public int DoublingValue
        {
            get { return _doublingValue; }
            set { _doublingValue = value; }
        }

        public string DoublingText
        {
            get { return _doublingText; }
            set { _doublingText = value; }
        }

    }

}