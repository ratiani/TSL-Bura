using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Core;

namespace TS.Gambling.Bura
{
    /// <summary>
    /// Summary description for BuraPlayer
    /// </summary>
    public class BuraPlayer : Player
    {
    
        public BuraPlayer()
        {
            _playerCards = new List<Card>();
            _takenCards = new List<Card>();
            _placedCards = new List<Card>();
        }

        private List<Card> _playerCards;
        private List<Card> _takenCards;
        private List<Card> _placedCards;
        private byte[] _avatar;
        private int _score;

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public byte[] Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }

        public List<Card> PlacedCards
        {
            get { return _placedCards; }
            set { _placedCards = value; }
        }

        public List<Card> TakenCards
        {
            get { return _takenCards; }
            set { _takenCards = value; }
        }

        public List<Card> PlayerCards
        {
            get { return _playerCards; }
            set { _playerCards = value; }
        }


    }

}