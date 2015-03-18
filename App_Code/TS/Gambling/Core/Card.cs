using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for Card
    /// </summary>
    public class Card
    {
        public Card(string name, CardType type, int rank, int value)
        {
            _name = name;
            _type = type;
            _rank = rank;
            _value = value;
        }

        private string _name;
        private CardType _type;
        private int _value;
        private int _rank;

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public CardType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


    }


    public enum CardType
    {
        EmptyCard = -2,
        Hidden = -1,
        Joker = 0,
        Club,
        Diamond,
        Heart,
        Spade
    }

}