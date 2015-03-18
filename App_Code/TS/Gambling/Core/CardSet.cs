using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for CardSet
    /// </summary>
    public abstract class CardSet
    {

        public static Dictionary<string, Card> BURA_CARDS = new Dictionary<string, Card>()
        {
            {"EmptyCard", new Card("EmptyCard", CardType.EmptyCard, 0, 0)},
            {"Hidden", new Card("Hidden", CardType.Hidden, 0, 0)},
            {"Spade", new Card("Spades", CardType.EmptyCard, 0, 0)},
            {"Club", new Card("Clubs", CardType.EmptyCard, 0, 0)},
            {"Diamond", new Card("Diamonds", CardType.EmptyCard, 0, 0)},
            {"Heart", new Card("Hearts", CardType.EmptyCard, 0, 0)},
            {"10_Spades", new Card("10_Spades", CardType.Spade, 10, 10)},
            {"Jack_Spades", new Card("Jack_Spades", CardType.Spade, 2, 2)},
            {"Queen_Spades", new Card("Queen_Spades", CardType.Spade, 3, 3)},
            {"King_Spades", new Card("King_Spades", CardType.Spade, 4, 4)},
            {"Ace_Spades", new Card("Ace_Spades", CardType.Spade, 11, 11)},
            {"10_Clubs", new Card("10_Clubs", CardType.Club, 10, 10)},
            {"Jack_Clubs", new Card("Jack_Clubs", CardType.Club, 2, 2)},
            {"Queen_Clubs", new Card("Queen_Clubs", CardType.Club, 3, 3)},
            {"King_Clubs", new Card("King_Clubs", CardType.Club, 4, 4)},
            {"Ace_Clubs", new Card("Ace_Clubs", CardType.Club, 11, 11)},
            {"10_Diamonds", new Card("10_Diamonds", CardType.Diamond, 10, 10)},
            {"Jack_Diamonds", new Card("Jack_Diamonds", CardType.Diamond, 2, 2)},
            {"Queen_Diamonds", new Card("Queen_Diamonds", CardType.Diamond, 3, 3)},
            {"King_Diamonds", new Card("King_Diamonds", CardType.Diamond, 4, 4)},
            {"Ace_Diamonds", new Card("Ace_Diamonds", CardType.Diamond, 11, 11)},
            {"10_Hearts", new Card("10_Hearts", CardType.Heart, 10, 10)},
            {"Jack_Hearts", new Card("Jack_Hearts", CardType.Heart, 2, 2)},
            {"Queen_Hearts", new Card("Queen_Hearts", CardType.Heart, 3, 3)},
            {"King_Hearts", new Card("King_Hearts", CardType.Heart, 4, 4)},
            {"Ace_Hearts", new Card("Ace_Hearts", CardType.Heart, 11, 11)},
        };

        public static Dictionary<string, Card> LONG_BURA_CARDS = new Dictionary<string, Card>()
        {
            {"EmptyCard", new Card("EmptyCard", CardType.EmptyCard, 0, 0)},
            {"Hidden", new Card("Hidden", CardType.Hidden, 0, 0)},
            {"Spade", new Card("Spades", CardType.EmptyCard, 0, 0)},
            {"Club", new Card("Clubs", CardType.EmptyCard, 0, 0)},
            {"Diamond", new Card("Diamonds", CardType.EmptyCard, 0, 0)},
            {"Heart", new Card("Hearts", CardType.EmptyCard, 0, 0)},
            {"6_Spades", new Card("6_Spades", CardType.Spade, 6, 0)},
            {"7_Spades", new Card("7_Spades", CardType.Spade, 7, 0)},
            {"8_Spades", new Card("8_Spades", CardType.Spade, 8, 0)},
            {"9_Spades", new Card("9_Spades", CardType.Spade, 9, 0)},
            {"10_Spades", new Card("10_Spades", CardType.Spade, 10, 10)},
            {"Jack_Spades", new Card("Jack_Spades", CardType.Spade, 11, 10)},
            {"Queen_Spades", new Card("Queen_Spades", CardType.Spade, 12, 10)},
            {"King_Spades", new Card("King_Spades", CardType.Spade, 13, 10)},
            {"Ace_Spades", new Card("Ace_Spades", CardType.Spade, 14, 11)},
            {"6_Clubs", new Card("6_Clubs", CardType.Club, 6, 0)},
            {"7_Clubs", new Card("7_Clubs", CardType.Club, 7, 0)},
            {"8_Clubs", new Card("8_Clubs", CardType.Club, 8, 0)},
            {"9_Clubs", new Card("9_Clubs", CardType.Club, 9, 0)},
            {"10_Clubs", new Card("10_Clubs", CardType.Club, 10, 10)},
            {"Jack_Clubs", new Card("Jack_Clubs", CardType.Club, 11, 10)},
            {"Queen_Clubs", new Card("Queen_Clubs", CardType.Club, 12, 10)},
            {"King_Clubs", new Card("King_Clubs", CardType.Club, 13, 10)},
            {"Ace_Clubs", new Card("Ace_Clubs", CardType.Club, 14, 11)},
            {"6_Diamonds", new Card("6_Diamonds", CardType.Diamond, 6, 0)},
            {"7_Diamonds", new Card("7_Diamonds", CardType.Diamond, 7, 0)},
            {"8_Diamonds", new Card("8_Diamonds", CardType.Diamond, 8, 0)},
            {"9_Diamonds", new Card("9_Diamonds", CardType.Diamond, 9, 0)},
            {"10_Diamonds", new Card("10_Diamonds", CardType.Diamond, 10, 10)},
            {"Jack_Diamonds", new Card("Jack_Diamonds", CardType.Diamond, 11, 10)},
            {"Queen_Diamonds", new Card("Queen_Diamonds", CardType.Diamond, 12, 10)},
            {"King_Diamonds", new Card("King_Diamonds", CardType.Diamond, 13, 10)},
            {"Ace_Diamonds", new Card("Ace_Diamonds", CardType.Diamond, 14, 11)},
            {"6_Hearts", new Card("6_Hearts", CardType.Heart, 6, 0)},
            {"7_Hearts", new Card("7_Hearts", CardType.Heart, 7, 0)},
            {"8_Hearts", new Card("8_Hearts", CardType.Heart, 8, 0)},
            {"9_Hearts", new Card("9_Hearts", CardType.Heart, 9, 0)},
            {"10_Hearts", new Card("10_Hearts", CardType.Heart, 10, 10)},
            {"Jack_Hearts", new Card("Jack_Hearts", CardType.Heart, 11, 10)},
            {"Queen_Hearts", new Card("Queen_Hearts", CardType.Heart, 12, 10)},
            {"King_Hearts", new Card("King_Hearts", CardType.Heart, 13, 10)},
            {"Ace_Hearts", new Card("Ace_Hearts", CardType.Heart, 14, 11)},
        };


        public CardSet()
        {
        }



    }

}