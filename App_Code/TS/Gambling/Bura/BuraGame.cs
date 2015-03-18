using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Core;

namespace TS.Gambling.Bura
{

    /// <summary>
    /// Summary description for BuraGame
    /// </summary>
    public class BuraGame : CardGame
    {

        public const int CARD_COUNT = 3;
        public const int LONG_BURA_CARD_COUNT = 5;
        public const int WINNING_SUM = 31;
        public const int LONG_BURA_DRAW_SUM = 60;

        public BuraGame(Player player, int playTill, double amount, bool longGameStyle, bool stickAllowed, bool passHiddenCards)
        {
            Players[player.PlayerId] = player;
            Board = new BuraBoard();
            Board.Game = this;
            Status = GameStatus.WaitingForOponent;
            // initilize game params
            PlayingTill = playTill;
            Amount = amount;
            LongGameStyle = longGameStyle;
            StickAllowed = stickAllowed;
            PassHiddenCards = passHiddenCards;

            Cards = LongGameStyle ? CardSet.LONG_BURA_CARDS : CardSet.BURA_CARDS;

            // create Starting game history record
            GamblingModel.Entities entities = new GamblingModel.Entities();
            GameHistory = GamblingModel.BuraGame.CreateBuraGame(1, LongGameStyle, PassHiddenCards, StickAllowed, Amount, PlayingTill, DateTime.Now);
            entities.AddToBuraGames(GameHistory);
            entities.SaveChanges();
            DBGameId = GameHistory.GameId;
        }

        public BuraPlayer getPlayer(int playerId)
        {
            return ((BuraPlayer)Players[playerId]);
        }

        public BuraPlayer getOponentPlayer(int playerId)
        {
            if (Players.Count != 2)
                return null;
            int oponentId = -1;
            if (Players.First().Key != playerId)
                oponentId = Players.First().Key;
            else
                oponentId = Players.Skip(1).First().Key;
            return getPlayer(oponentId);
        }


        public List<Card> parseCards(string cardsString)
        {
            string[] cards = cardsString.Split(";".ToCharArray());
            List<Card> cardsList = new List<Card>();
            for (int index = 0; index < cards.Length; index++)
            {
                string card = cards[index];
                if (card.StartsWith("CD_"))
                    card = card.Substring("CD_".Length);
                if (Cards.ContainsKey(card))
                    cardsList.Add(Cards[card]);
            }
            return cardsList;
        }

        public override void ProcessEvent(int playerId, GameEvent gameEvent)
        {
            EndEvent(playerId, gameEvent.EventId);
        }

        /*
         * EndEvent is called on the end of every event 
         * */
        public override void EndEvent(int playerId, int eventId)
        {
            Player player = getPlayer(playerId);
            player.Events.Remove(eventId);
            // adjust remaining events time
            foreach (int nextEventId in player.Events.Keys)
            {
                player.Events[nextEventId].EventDate = DateTime.Now;
            }
            GameVersion++;
        }

        /*
         * Reject Doubling Offer Event Handler
         * Game must be stopped
         * */
        public override void RejectDoubling(int rejectorPlayerId)
        {
            EndGame(getOponentPlayer(rejectorPlayerId));
        }

        /*
         * Player Place Card Event Handler
         * */
        public bool PlaceCards(int playerId, string cardsString, bool wantsTakeCards)
        {
            BuraPlayer player = getPlayer(playerId);
            BuraPlayer oponent = getOponentPlayer(playerId);
            List<Card> cards = parseCards(cardsString);
            // check if it is player turn 
            if (PlayerTurn != player.PlayerId)
                return false;

            // check if player has these cards
            foreach (Card card in cards)
            {
                if (!player.PlayerCards.Contains(card))
                    // player is trying to play wrong cards
                    return false;
            }

            if (CanPlaceCards(playerId, cards, wantsTakeCards))
            {

                // place selected cards
                foreach (Card card in cards)
                {
                    int cardIndex = player.PlayerCards.IndexOf(card);
                    if (cardIndex > -1)
                    {
                        player.PlayerCards[cardIndex] = Cards["EmptyCard"];
                        player.PlacedCards.Add(card);
                    }
                }

                // finilize PlayerTurn Event (remove it)
                int playerTurnEventId = -1;
                foreach (int eventId in player.Events.Keys)
                {
                    if (player.Events[eventId].Type == EventType.PlayerTurn)
                    {
                        playerTurnEventId = eventId;
                        break;
                    }
                }
                if (playerTurnEventId != -1)
                    EndEvent(player.PlayerId, playerTurnEventId);

                // check for BURA
                if (Status == GameStatus.Finished)
                {
                    EndGame(LastCardTakerPlayer);
                    return true;
                }
                else
                {
                    // generate place card events
                    AddPlayerEvent(player, EventType.PlaceCard, player, -IdGenerator.NextValue);
                    AddPlayerEvent(oponent, EventType.PlaceCard, player, -IdGenerator.NextValue);

                    // check if both players have played
                    if (oponent.PlacedCards.Count > 0)
                    {
                        //LastCardTakerPlayer is determined inside CanPlaceCards function
                        // check if turn has finished (stick was not played)
                        if (player.PlacedCards.Count == oponent.PlacedCards.Count)
                        {
                            // add taken cards
                            LastCardTakerPlayer.TakenCards.AddRange(player.PlacedCards);
                            LastCardTakerPlayer.TakenCards.AddRange(oponent.PlacedCards);

                            // calculate non empty cards
                            int nonEmptyCardCount = 0;
                            foreach (Card card in player.PlayerCards)
                            {
                                nonEmptyCardCount += card.Type == CardType.EmptyCard ? 0 : 1;
                            }

                            // prepeare next deal
                            if (DealingCards.Count > 0 || nonEmptyCardCount > 0)
                            {
                                AddPlayerEvent(LastCardTakerPlayer, EventType.ContinueQuestion, string.Empty);
                            }
                            else
                            {
                                // there are no more cards to play
                                ShowPlayerCards(LastCardTakerPlayer.PlayerId);
                            }
                        }
                    }
                    else
                    {
                        // oponent has not played and now is his turn
                        PreparePlayerTurn(oponent.PlayerId);
                    }

                    GameVersion++;
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        /*
         * Check player PlaceCard action
         * assigns value to LastCardTakenPlayer
         * */
        public bool CanPlaceCards(int playerId, List<Card> playedCards, bool wantsTakeCards)
        {
            // player has to select cards
            if (playedCards.Count == 0)
                return false;

            BuraPlayer player = getPlayer(playerId);
            BuraPlayer oponent = getOponentPlayer(playerId);

            // add already played cards, if exists
            List<Card> cards = playedCards;
            if (player.PlacedCards.Count > 0)
                cards.AddRange(player.PlacedCards);

            // check for BURA
            if (isBuraCombination(cards))
            {
                LastCardTakerPlayer = player;
                Status = GameStatus.Finished;
                return true;
            }

            // check if this is first player
            if (oponent.PlacedCards.Count == 0)
            {
                return areSimilarCards(cards);
            }

            // oponent has played , before player

            // if player dont wants to take cards, then only card count must be same
            if (!wantsTakeCards)
            {
                LastCardTakerPlayer = oponent;
                return cards.Count == oponent.PlacedCards.Count;
            }


            // oponent has played and player wants to take cards

            // placed card count must not less than oponents cards
            if (oponent.PlacedCards.Count > cards.Count)
                return false;

            // if placed card count is more than oponents cards, then players card combination 
            // must be stick and stick must be allowed and oponent must place more cards ...
            if (cards.Count > oponent.PlacedCards.Count)
            {
                if (StickAllowed && isStickCombination(cards))
                {
                    LastCardTakerPlayer = player;
                    PreparePlayerTurn(oponent.PlayerId);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // card count are same, check if player cards can take oponets cards...
            // cards can take other cards , if any n-th min card is greater than oponent n-th min card
            List<Card> playerCards = new List<Card>();
            playerCards.AddRange(cards);
            List<Card> oponentCards = new List<Card>();
            oponentCards.AddRange(oponent.PlacedCards);
            for (int index = 0; index < cards.Count; index++)
            {
                Card playerMinCard = getMinCard(playerCards);
                playerCards.Remove(playerMinCard);
                Card oponentMinCard = getMinCard(oponentCards);
                oponentCards.Remove(oponentMinCard);

                if (!isCardGreater(playerMinCard, oponentMinCard))
                {
                    // oponents n-th min card is greater
                    return false;
                }
            }

            // player can take oponents cards 
            LastCardTakerPlayer = player;
            return true;
        }

        /*
         * return true if Card1 is smaller than Card2
         * */
        private bool isCardGreater(Card card1, Card card2)
        {

            if (card1.Type == Trump.Type && card2.Type != Trump.Type)
            {
                // card1 is trump and card2 not
                return true;
            }
            else
            {
                if (card1.Type != Trump.Type && card2.Type == Trump.Type)
                    // card2 is trump and card1 not
                    return false;
                else
                {
                    if (card1.Rank > card2.Rank && card1.Type == card2.Type)
                        // card1 and card2 are same type, but card1 value is greater
                        return true;
                    else
                        return false;
                }
            }
        }

        /*
         * determine and retuen most small card from list
         * */
        private Card getMinCard(List<Card> cards)
        {
            Card minCard = cards.First();
            foreach (Card card in cards)
            {
                // compare cards ...
                if (isCardGreater(minCard, card))
                    minCard = card;
            }
            return minCard;
        }

        /*
         * returns true , if all card types are same
         * */
        private bool areSimilarCards(List<Card> cards)
        {
            if (cards == null || cards.Count <= 1)
                return true;
            CardType cardType = cards.First().Type;
            foreach (Card card in cards)
            {
                if (card.Type != cardType)
                    return false;
            }
            return true;
        }

        /*
         * returns true, if all card types are same and card count is 
         * max card count
         * */
        private bool isStickCombination(List<Card> cards)
        {
            return
                areSimilarCards(cards) &&
                ((LongGameStyle && cards.Count == LONG_BURA_CARD_COUNT)
                  || (!LongGameStyle && cards.Count == CARD_COUNT));
        }

        /*
         * return true if card combination is BURA, in 
         * this case game must be stopped
         * */
        private bool isBuraCombination(List<Card> cards)
        {
            return isStickCombination(cards) && cards.First().Type == Trump.Type;
        }

        /*
         * Shufle CardSet at GameStart
         * */
        public void ShufleDealingCards()
        {
            _dealingCards = new List<Card>();
            Random random = new Random();
            foreach (string cardId in Cards.Keys)
            {
                Card card = Cards[cardId];
                if (card.Type == CardType.EmptyCard || card.Type == CardType.Hidden)
                    continue;
                int cardIndex = random.Next(0, _dealingCards.Count);
                _dealingCards.Insert(cardIndex, card);
            }
        }

        /*
         * End of game
         * */
        public void EndGame(BuraPlayer winnerPlayer)
        {
            Status = GameStatus.Finished;

            // clear all previous events, but ShowCard event
            foreach (int playerId in Players.Keys)
            {
                int[] eventIds = Players[playerId].Events.Keys.ToArray();
                foreach (int eventId in eventIds)
                {
                    if (Players[playerId].Events[eventId].Type != EventType.ShowCards)
                        Players[playerId].Events.Remove(eventId);
                }
            }

            if (PlayerHasTimeouted)
            {
                // player has timeouted and game is stopped
                BuraPlayer looserPlayer = getOponentPlayer(winnerPlayer.PlayerId);
                AddPlayerEvent(winnerPlayer, EventType.TimeoutWin, "");
                AddPlayerEvent(looserPlayer, EventType.TimeoutLoose, "");
            } 
            else if (winnerPlayer != null)
            {
                BuraPlayer looserPlayer = getOponentPlayer(winnerPlayer.PlayerId);
                winnerPlayer.Score += Doubling.DoublingValue;
                // add win and loose events
                AddPlayerEvent(winnerPlayer, EventType.WinDeal, "");
                AddPlayerEvent(looserPlayer, EventType.LooseDeal, "");
                if (winnerPlayer.Score >= PlayingTill)
                {
                    // game is finished
                    int newGameId = IdGenerator.NextValue;
                    AddPlayerEvent(winnerPlayer, EventType.WinAndRematchOffer, newGameId);
                    AddPlayerEvent(looserPlayer, EventType.LooseAndRematchOffer, newGameId);
                }
                else
                {
                    // start new Deal
                    AddPlayerEvent(winnerPlayer, EventType.ContinueGame, winnerPlayer);
                }
                // 
                LastCardTakerPlayer = winnerPlayer;
            }
            else
            {
                // game has drawed
                AddPlayerEvent(winnerPlayer, EventType.ContinueGame, LastCardTakerPlayer);
            }
        }

        /*
         * Start of Game, is called when game starts
         * StartGame Event Handler
         * */
        public void StartGame()
        {
            // check game status
            if (Status != GameStatus.Finished && Status != GameStatus.WaitingForOponent)
                return;

            // refresh all data
            Doubling = GameDoubling.Items[0];
            foreach (int playerId in Players.Keys)
            {
                BuraPlayer player = getPlayer(playerId);
                player.TakenCards.Clear();
                player.PlacedCards.Clear();
                player.PlayerCards.Clear();
                player.Events.Clear();
                int cardCount = LongGameStyle ? BuraGame.LONG_BURA_CARD_COUNT : BuraGame.CARD_COUNT;
                for (int i = 0; i < cardCount; i++)
                {
                    player.PlayerCards.Add(LongGameStyle ? CardSet.LONG_BURA_CARDS["EmptyCard"] : CardSet.BURA_CARDS["EmptyCard"]);
                }
            }

            if (LastCardTakerPlayer == null)
            {
                LastCardTakerPlayer = getPlayer(Players.Keys.First());
                PreviousCardTakerPlayer = LastCardTakerPlayer;
            }
            Status = GameStatus.Playing;
            DoublingOfferedBy = -1;
            RematchOffered = false;
            ShufleDealingCards();
            _trump = _dealingCards[_dealingCards.Count - 1];
            DealCards();
            PreparePlayerTurn(LastCardTakerPlayer.PlayerId);
        }

        /*
         * Deal Cards event Generator
         * Empty cards are replaced by new cards
         * */
        public void DealCards()
        {
            if (LastCardTakerPlayer == null)
                return;

            // determine card count to deal
            int cardCount = 0;
            BuraPlayer player = LastCardTakerPlayer;
            for (int index = 0; index < player.PlayerCards.Count; index++)
            {
                if (player.PlayerCards[index].Type == CardType.EmptyCard)
                {
                    if (_dealingCards.Count > 0)
                    {
                        cardCount++;
                    }
                }
            }

            // reorder cards (empty cards must be after non empty cards)
            foreach (int currentPlayerId in Players.Keys)
            {
                BuraPlayer currentPlayer = getPlayer(currentPlayerId);
                for (int index = 0; index < currentPlayer.PlayerCards.Count; index++)
                {
                    if (currentPlayer.PlayerCards[index].Type == CardType.EmptyCard)
                    {
                        // find non empty card
                        bool foundCard = false;
                        int nonEmptyCardIndex = -1;
                        for (int nextCardIndex = index + 1; nextCardIndex < currentPlayer.PlayerCards.Count; nextCardIndex++)
                        {
                            if (currentPlayer.PlayerCards[nextCardIndex].Type != CardType.EmptyCard)
                            {
                                nonEmptyCardIndex = nextCardIndex;
                                foundCard = true;
                                break;
                            }
                        }
                        if (foundCard)
                        {
                            Card emptyCard = currentPlayer.PlayerCards[index];
                            currentPlayer.PlayerCards[index] = currentPlayer.PlayerCards[nonEmptyCardIndex];
                            currentPlayer.PlayerCards[nonEmptyCardIndex] = emptyCard;
                        }
                    }
                }
            }

            // deal cards
            int dealedCardCount = 0;
            BuraPlayer oponent = getOponentPlayer(player.PlayerId);
            BuraPlayer[] dealPlayers = new BuraPlayer[2] { player, oponent };
            for (int dealIndex = 0; dealIndex < cardCount; dealIndex++)
            {

                foreach (BuraPlayer buraPlayer in dealPlayers)
                {
                    for (int index = 0; index < buraPlayer.PlayerCards.Count; index++)
                    {
                        if (buraPlayer.PlayerCards[index].Type == CardType.EmptyCard)
                        {
                            if (_dealingCards.Count > 0)
                            {
                                buraPlayer.PlayerCards[index] = _dealingCards[0];
                                _dealingCards.RemoveAt(0);
                                dealedCardCount++;
                                break;
                            }
                        }
                    }

                }
            }
            dealedCardCount = dealedCardCount / 2;

            // add deal events
            AddPlayerEvent(player, EventType.DealCards, dealedCardCount);
            AddPlayerEvent(oponent, EventType.DealCards, dealedCardCount);
            GameVersion++;
        }

        /*
         * Player Turn event generator
         * place card message is shown
         * */
        public void PreparePlayerTurn(int playerId)
        {
            AddPlayerEvent(getPlayer(playerId), EventType.PlayerTurn, playerId);
            PlayerTurn = playerId;
        }

        /*
         * Continues Games is called after Continue Question
         * Deals cards and prepares player turn
         * */
        public void ContinueGame(int playerId)
        {
            // check if it is player turn
            if (playerId != LastCardTakerPlayer.PlayerId)
                return;

            // remove taken cards
            BuraPlayer takerPlayer = getPlayer(playerId);
            BuraPlayer oponent = getOponentPlayer(takerPlayer.PlayerId);
            takerPlayer.PlacedCards.Clear();
            oponent.PlacedCards.Clear();

            // start new turn
            DealCards();
            PreparePlayerTurn(playerId);
            LastCardTakerPlayer = null;
            GameVersion++;
        }


        /*
         * Show Player Taken cards.
         * Game must end after this event
         * */
        public void ShowPlayerCards(int playerId)
        {
            // check if it is player turn
            if (playerId != LastCardTakerPlayer.PlayerId)
                return;

            BuraPlayer player = getPlayer(playerId);
            BuraPlayer oponent = getOponentPlayer(playerId);
            AddPlayerEvent(player, EventType.ShowCards, playerId);
            AddPlayerEvent(oponent, EventType.ShowCards, playerId);

            // calculate winner 
            BuraPlayer winnerPlayer = null;
            int totalValue = 0;
            foreach (Card card in player.TakenCards)
            {
                totalValue += card.Value;
            }
            if (!LongGameStyle)
            {
                winnerPlayer = (totalValue >= WINNING_SUM) ? player : oponent;
            }
            else
            {
                // long bura game
                if (totalValue > LONG_BURA_DRAW_SUM)
                    winnerPlayer = player;
                else if (totalValue < LONG_BURA_DRAW_SUM)
                    winnerPlayer = oponent;
                else
                {
                    // no one is winner, game has drawed
                }
            }

            EndGame(winnerPlayer);
        }


        public void RematchOffer(int playerId, int eventId)
        {
            int newGameId = int.Parse(getPlayer(playerId).Events[eventId].EventValue.ToString());
            BuraGameController.CurrentInstanse.RematchGame(newGameId);
        }


        /*
         * Player has timeouted and has lost game.
         * this event is throwed by Scheduler
         * */
        public void PlayerTimeout(int playerId)
        {
            if (Status != GameStatus.Finished && Status != GameStatus.WaitingForOponent)
            {
                Status = GameStatus.Finished;
                PlayerHasTimeouted = true;
                BuraPlayer winnerPlayer = getOponentPlayer(playerId);
                EndGame(winnerPlayer);
            }
        }

        public void LeaveGame(int playerId)
        {
            throw new NotImplementedException();
        }


        /*
         * Save Current Board into DB
         * */
        public override void SaveCurrentGameVersion(int currentVersionNumber)
        {
            GamblingModel.Entities entities = new GamblingModel.Entities();
            int histGameId = GameHistory.GameId;
            GamblingModel.BuraGameVersion buraGameVersion =
                GamblingModel.BuraGameVersion.CreateBuraGameVersion(histGameId, currentVersionNumber, Status.ToString(), PlayerHasTimeouted, Doubling.DoublingValue, DealingCards.Count);
            if (this.Trump != null)
                buraGameVersion.Trump = this.Trump.Type.ToString();
            if (this.LastCardTakerPlayer != null)
                buraGameVersion.LastCardTakerPlayer = this.LastCardTakerPlayer.PlayerId;
            if (this.PreviousCardTakerPlayer != null)
                buraGameVersion.PreviousCardTakerPlayer = this.PreviousCardTakerPlayer.PlayerId;
            if (this.DoublingOfferedBy != -1)
                buraGameVersion.LastDoublingOfferer = this.DoublingOfferedBy;
            entities.AddToBuraGameVersions(buraGameVersion);
            entities.SaveChanges();

            foreach (int playerId in Players.Keys)
            {
                BuraPlayer player = getPlayer(playerId);
                // create bura player for current game version
                GamblingModel.BuraGamePlayer buraGamePlayer =
                    GamblingModel.BuraGamePlayer.CreateBuraGamePlayer(0, histGameId, currentVersionNumber, playerId, player.Score);
                entities.AddToBuraGamePlayers(buraGamePlayer);
                entities.SaveChanges();

                int buraPlayerVerionId = buraGamePlayer.BuraGamePlayerId;
                buraGameVersion.BuraGamePlayers.Add(buraGamePlayer);
                // add player cards 
                foreach (Card card in player.PlayerCards)
                {
                    GamblingModel.BuraPlayerCard playerCard = GamblingModel.BuraPlayerCard.CreateBuraPlayerCard(0, buraPlayerVerionId, card.Name);
                    buraGamePlayer.PlayerCards.Add(playerCard);
                }
                // add placed cards 
                foreach (Card card in player.PlacedCards)
                {
                    GamblingModel.BuraPlayerPlacedCard placedCard = GamblingModel.BuraPlayerPlacedCard.CreateBuraPlayerPlacedCard(0, buraPlayerVerionId, card.Name);
                    buraGamePlayer.PlacedCards.Add(placedCard);
                }
                // add taken cards 
                foreach (Card card in player.TakenCards)
                {
                    GamblingModel.BuraPlayerTakenCard takenCard = GamblingModel.BuraPlayerTakenCard.CreateBuraPlayerTakenCard(0, buraPlayerVerionId, card.Name, card.Value);
                    buraGamePlayer.TakenCards.Add(takenCard);
                }
                // add player events 
                foreach (int eventId in player.Events.Keys)
                {
                    GameEvent gameEvent = player.Events[eventId];
                    GamblingModel.BuraGameEvent buraGameEvent = 
                        GamblingModel.BuraGameEvent.CreateBuraGameEvent(0, buraPlayerVerionId, eventId, gameEvent.Type.ToString(), gameEvent.EventPlayed );
                    buraGameEvent.EventValue = gameEvent.EventValue.ToString();
                    buraGamePlayer.Events.Add(buraGameEvent);
                }

            }
            //entities.S(buraGameVersion);
            entities.SaveChanges();
        }


        private Card _trump;
        private List<Card> _dealingCards;
        private BuraPlayer _lastCardTakerPlayer;
        private bool _passHiddenCards;
        private bool _stickAllowed;
        private bool _longGameStyle;
        private Dictionary<string, Card> _cards;
        private int _playingTill;
        private bool _rematchOffered;
        private double _amount;
        private BuraPlayer _previousCardTakerPlayer;
        private bool _playerHasTimeouted;
        private GamblingModel.BuraGame _gameHistory;
        private int _dbGameId;

        public int DBGameId
        {
            get { return _dbGameId; }
            set { _dbGameId = value; }
        }

        public GamblingModel.BuraGame GameHistory
        {
            get { return _gameHistory; }
            set { _gameHistory = value; }
        }

        public bool PlayerHasTimeouted
        {
            get { return _playerHasTimeouted; }
            set { _playerHasTimeouted = value; }
        }

        public BuraPlayer PreviousCardTakerPlayer
        {
            get { return _previousCardTakerPlayer; }
            set { _previousCardTakerPlayer = value; }
        }

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public bool RematchOffered
        {
            get { return _rematchOffered; }
            set { _rematchOffered = value; }
        }

        public int PlayingTill
        {
            get { return _playingTill; }
            set { _playingTill = value; }
        }

        public Dictionary<string, Card> Cards
        {
            get { return _cards; }
            set { _cards = value; }
        }

        public bool LongGameStyle
        {
            get { return _longGameStyle; }
            set { _longGameStyle = value; }
        }

        public bool StickAllowed
        {
            get { return _stickAllowed; }
            set { _stickAllowed = value; }
        }

        public bool PassHiddenCards
        {
            get { return _passHiddenCards; }
            set { _passHiddenCards = value; }
        }

        public BuraPlayer LastCardTakerPlayer
        {
            get { return _lastCardTakerPlayer; }
            set
            {
                if (_lastCardTakerPlayer != null)
                    _previousCardTakerPlayer = _lastCardTakerPlayer;
                _lastCardTakerPlayer = value;
            }
        }

        public Card Trump
        {
            get { return _trump; }
            set { _trump = value; }
        }

        public List<Card> DealingCards
        {
            get { return _dealingCards; }
            set { _dealingCards = value; }
        }

    }

}