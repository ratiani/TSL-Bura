using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.Gambling.Core;
using TS.Gambling.Web;
using System.Text;


namespace TS.Gambling.Bura
{
    /// <summary>
    /// Summary description for BuraBoard
    /// </summary>
    public class BuraBoard : HtmlBoard
    {

        protected HtmlArea[] playerAreas = new HtmlArea[] {
            new HtmlArea(480, 250, 500, 130),
            new HtmlArea(20, 250, 500, 130),
        };
        protected HtmlArea[] placedCardsAreas = new HtmlArea[] {
            new HtmlArea(300, 250, 500, 120),
            new HtmlArea(230, 250, 500, 120),
        };
        protected HtmlArea[] takenCardsAreas = new HtmlArea[] {
            new HtmlArea(380, 70, 120, 120),
            new HtmlArea(120, 70, 120, 120),
        };

        protected HtmlArea trumpArea = new HtmlArea(280, 810, 120, 120);
        protected HtmlArea buttonsArea = new HtmlArea(450, 650, 100, 100);
        protected HtmlArea showCardsArea = new HtmlArea(280, 250, 600, 111);
        protected HtmlArea eventsArea = new HtmlArea(50, 50, 200, 200);

        public BuraBoard()
        {
        }

        public HtmlResponse GetBoardHtml(int playerId)
        {
            HtmlResponse response = new HtmlResponse();

            
            // draw player cards
            foreach (int localPlayerId in Game.Players.Keys)
            {
                BuraPlayer player = getBuraGame().getPlayer(localPlayerId);
                bool isMainPlayer = (localPlayerId == playerId);
                int index = isMainPlayer ? 0 : 1;
                GeneratePlayerCards(response, player, player.PlayerCards, playerAreas[index], isMainPlayer);
                GeneratePlacedCards(response, player, player.PlacedCards, placedCardsAreas[index], isMainPlayer);
                if (player.Events.Count > 0)
                {
                    GenerateTimerScript(response, localPlayerId, player.Events.First().Value);
                }
                index++;
            }

            // draw trump & remaining cards
            GenerateTrumpCard(response, ((BuraGame)Game).Trump);

            // add event scripts for player
            if (Game.Players[playerId].Events != null && Game.Players[playerId].Events.Count > 0)
            {
                GameEvent gameEvent = Game.Players[playerId].Events.First().Value;
                switch (gameEvent.Type)
                {
                    case EventType.DealCards: GenerateDealScript(response, playerId, gameEvent); break;
                    case EventType.PlaceCard: GeneratePlaceCardScript(response, playerId, gameEvent); break;
                    case EventType.DoublingOffer: GenerateDoublingOfferScript(response, gameEvent); break;
                    case EventType.ContinueQuestion: GenerateContinueQuestionEventScript(response, gameEvent); break;
                    case EventType.ShowCards: GenerateShowCardsEventScript(response, gameEvent); break;
                    case EventType.PlayerTurn: GeneratePlayerTurnScript(response, gameEvent); break;
                    case EventType.WinDeal: GeneratePlayerWinScript(response, gameEvent); break;
                    case EventType.LooseDeal: GeneratePlayerLooseScript(response, gameEvent); break;
                    case EventType.WinAndRematchOffer: GenerateWinAndRematchScript(response, gameEvent); break;
                    case EventType.LooseAndRematchOffer: GenerateLooseAndRematchScript(response, gameEvent); break;
                    case EventType.ContinueGame: GenerateContinueGameScript(response, gameEvent); break;
                    case EventType.TimeoutWin: GenerateTimeoutWinScript(response, gameEvent); break;
                    case EventType.TimeoutLoose: GenerateTimeoutLooseScript(response, gameEvent); break;
                }
            }

            // draw button controls (test)
            GenerateButtonsHtml(response);

            // draw game info
            GenerateGameInfoHtml(response, playerId);

            // draw taken cards 
            GenerateTakenCards(response, playerId);

            // add init script to update
            response.Script.Append("initBoard();");

            // draw player events
            GeneratePlayerEventsHtml(response, playerId);

            return response;
        }

        public BuraGame getBuraGame()
        {
            return (BuraGame)Game;
        }

        /*
         * Initialize Board
         * */
        private void Initialize(HtmlResponse response, int playerId)
        {
            response.Script.AppendFormat("StopTimer();");
        }

        /*
         * Initialize Timer for player
         * */
        private void GenerateTimerScript(HtmlResponse response, int playerId, GameEvent gameEvent)
        {
            string timerPosition = GameContext.GetCurrentPlayer().PlayerId == playerId ? "Bottom" : "Top";
            long elapsedTime = (DateTime.Now.Ticks - gameEvent.EventDate.Ticks) / TimeSpan.TicksPerSecond;
            long elapsedHeight = elapsedTime * 100 / GameEvent.EVENT_TIME;
            if (elapsedHeight > 100)
                elapsedHeight = 100;
            // add timer progress
            response.Html.AppendFormat(
                "<div class='PlayerTimer {1}' id='TimerPlayer{0}'><div class='ElapsedContent' style='height:{2}px;'></div></div>",
                playerId, timerPosition, elapsedHeight);
            
            // timer works only for current player
            if (GameContext.GetCurrentPlayer().PlayerId == playerId)
            {
                response.Script.AppendFormat("StartTimer({0}, {1}, {2});", playerId, elapsedHeight, GameEvent.EVENT_TIME * 1000 / 100);
            }
        }

        /*
         * Generates Events monitoring list for debug
         * */
        public void GeneratePlayerEventsHtml(HtmlResponse response, int playerId)
        {
            Player player = Game.Players[playerId];
            response.Html.AppendFormat("<div style='position:absolute; top:{0}px; left:{1}px; width:{2}px; height:{3}px; color:Maroon;'>",
                    eventsArea.Top, eventsArea.Left, eventsArea.Width, eventsArea.Height);
            foreach (int eventId in player.Events.Keys)
            {
                GameEvent playerEvent = player.Events[eventId];
                response.Html.AppendFormat("{0};<b>{1}</b>;{2};<br />", playerEvent.EventId, playerEvent.Type.ToString(), playerEvent.EventValue);
            }
            response.Html.AppendFormat("</div>");
        }


        /*
         * Generate Script for Dealing Cards
         * */
        protected void GenerateDealScript(HtmlResponse response, int playerId, GameEvent gameEvent)
        {
            int cardCount = int.Parse(gameEvent.EventValue.ToString());
            foreach (int currentPlayerId in Game.Players.Keys)
            {
                BuraPlayer player = (BuraPlayer)Game.Players[currentPlayerId];
                bool isMainPlayer = (player.PlayerId == playerId);
                HtmlArea area = isMainPlayer ? playerAreas[0] : playerAreas[1];

                int destinationTop = area.Top + 20;
                int destinationLeft = area.Left + (3 - cardCount) * (CARD_WIDTH + 8);

                int startingTop = trumpArea.Top;
                int startingLeft = trumpArea.Left;

                int currentIndex = (3 - cardCount);

                for (int index = player.PlayerCards.Count - cardCount; index < player.PlayerCards.Count; index++)
                {
                    Card card = player.PlayerCards[index];
                    string mainCardId = isMainPlayer ? string.Format("CD_{0}", card.Name) : string.Format("CD_{0}_{1}", player.PlayerId, currentIndex);
                    string dealCardId = isMainPlayer ? string.Format("CD_D{0}", card.Name) : string.Format("CD_D{0}_{1}", player.PlayerId, currentIndex);
                    string cardImage = GetHiddenCardImage();
                    string cardClass = "DealingCard";

                    response.Html.AppendFormat("<div id='{0}' class='{1}' style='top:{2}px; left:{3}px; height:{4}px; " +
                        "width:{5}px; position:absolute; z-index:2;'><img alt='' src='{6}' /></div>",
                        dealCardId, cardClass, startingTop, startingLeft + CARD_WIDTH / 2, CARD_HEIGHT, CARD_HEIGHT, GetHiddenCardImage());

                    string destination = "{'top':" + destinationTop.ToString() + ", 'left':" + destinationLeft.ToString() + "}";
                    string eventAction = "function() {" + string.Format("$('#{0}').css('display', 'block'); $('#{1}').css('display', 'none'); ", mainCardId, dealCardId) +
                        ((index == player.PlayerCards.Count - 1 && isMainPlayer) ? " " : string.Empty) + " }";

                    response.Script.AppendFormat(("$('#{0}').css('display', 'none'); $('#{1}').delay(500).animate({2}, '500', 'linear', {3});"),
                        mainCardId, dealCardId, destination, eventAction);

                    destinationLeft += CARD_WIDTH + 8;
                    currentIndex++;
                }
            }
            response.Script.AppendFormat("setTimeout(\"BoardEvent('EndEvent:{0}')\", 1000);", gameEvent.EventId);
        }

        protected void GeneratePlayerCards(HtmlResponse response, BuraPlayer player, List<Card> playerCards, HtmlArea area, bool isMainPlayer)
        {
            int top = area.Top + 20;
            //int left = area.Left;
            int left = area.Left + (area.Width - (area.Width / 5 * playerCards.Count)) / 2;

            int currentIndex = 0;
            foreach (Card card in playerCards)
            {
                string cardId = isMainPlayer ? string.Format("id='CD_{0}'", card.Name) : string.Format("id='CD_{0}_{1}'", player.PlayerId, currentIndex);
                string cardImage = isMainPlayer ? GetCardImage(card) : GetHiddenCardImage();
                string cardClass = isMainPlayer ? "Card" : "HiddenCard";
                if (card.Type != CardType.EmptyCard)
                {
                    response.Html.AppendFormat("<div {0} class='{1}' style='top:{2}px; left:{3}px; height:{4}px; width:{5}px; z-index:0; position:absolute;'><img alt='' src='{6}' /></div>",
                        cardId, cardClass, top, left, CARD_HEIGHT, CARD_WIDTH, cardImage);
                }
                else
                {
                    response.Html.AppendFormat("<div class='EmptyCard' style='top:{0}px; left:{1}px; height:{2}px; width:{3}px; z-index:0; position:absolute;'></div>",
                        top, left, CARD_HEIGHT, CARD_WIDTH);
                }

                left += CARD_WIDTH + 8;
                currentIndex++;
            }
        }

        protected void GeneratePlacedCards(HtmlResponse response, BuraPlayer player, List<Card> playerCards, HtmlArea area, bool isMainPlayer)
        {
            BuraPlayer oponent = getBuraGame().getOponentPlayer(player.PlayerId);
            if (oponent == null)
                return;

            int top = area.Top + 20;
            // center placed cards
            //int left = area.Left + (area.Width - (area.Width / 5 * playerCards.Count)) / 2;
            int left = area.Left;
            int zIndex = 0;
            if (getBuraGame().PreviousCardTakerPlayer != null)
            {
                zIndex = getBuraGame().PreviousCardTakerPlayer.PlayerId == player.PlayerId ? 0 : 2;
                left += getBuraGame().PreviousCardTakerPlayer.PlayerId == player.PlayerId ? 0 : 20;
            }
            int rotateAngle = Game.PlayerTurn == player.PlayerId ? -20 : 20;
            response.Html.AppendFormat("<div align='center' style='top:{0}px; left:{1}px; height:{2}px; width:{3}px; z-index:{4}; position:absolute;'>",
                top, left, area.Height, area.Width, zIndex);
            int currentIndex = 0;

            bool hideCards = false;
            // check if both players has played
            bool bothPlayerPlayed = player.PlacedCards.Count > 0 && oponent.PlacedCards.Count > 0;
            if (bothPlayerPlayed)
            {
                hideCards = (getBuraGame().LastCardTakerPlayer.PlayerId != player.PlayerId && getBuraGame().PreviousCardTakerPlayer.PlayerId != player.PlayerId)
                        && getBuraGame().PassHiddenCards;
            } else {
                hideCards = false;
            }
            foreach (Card card in playerCards)
            {
                string cardId = string.Format("PCD_{0}_{1}", player.PlayerId, currentIndex);
                string cardImage = card.Type == CardType.Hidden || hideCards ? GetHiddenCardImage() : GetCardImage(card);
                string cardClass = card.Type == CardType.Hidden || hideCards ? "PlacedHiddenCard" : "PlacedCard";

                response.Html.AppendFormat("<div id='{0}' class='{1}' style='margin-left:16px; height:{2}px; width:{3}px;'><img alt='' src='{4}' /></div>",
                    cardId, cardClass, CARD_HEIGHT, 1.8 * CARD_WIDTH, cardImage);

                // disable place card rotation
                //response.Script.AppendFormat("$('#{0} img').rotate({1});", cardId, rotateAngle);

                left += CARD_WIDTH + 8;
                rotateAngle *= -1;
                currentIndex++;
            }
            response.Html.AppendFormat("</div>");
        }

        protected void GeneratePlaceCardScript(HtmlResponse response, int viewerPlayerId, GameEvent gameEvent)
        {
            BuraPlayer player = (BuraPlayer)gameEvent.EventValue;

            int currentIndex = 0;
            foreach (Card card in player.PlacedCards)
            {
                string cardId = string.Format("PCD_{0}_{1}", player.PlayerId, currentIndex);
                response.Script.AppendFormat("$('#{0}').addClass('PlaceCardScript');", cardId);
                currentIndex++;
            }

            currentIndex = 0;
            HtmlArea area = viewerPlayerId == player.PlayerId ? playerAreas[0] : playerAreas[1];
            int top = area.Top + 20;
            int left = area.Left;
            
            for (int index = 0; index < player.PlayerCards.Count; index ++ )
            {
                Card card = player.PlayerCards[index];
                if (card.Type == CardType.EmptyCard)
                {
                    string cardId = string.Format("PCDS_{0}_{1}", player.PlayerId, currentIndex);
                    string cardImage = GetHiddenCardImage();
                    string cardClass = "PlacedHiddenCardScript";

                    response.Html.AppendFormat("<div {0} class='{1}' style='top:{2}px; left:{3}px; height:{4}px; width:{5}px; z-index:0; position:absolute;'><img alt='' src='{6}' /></div>",
                        cardId, cardClass, top, left, CARD_HEIGHT, CARD_WIDTH, cardImage);
                    currentIndex++;
                }
                left += CARD_WIDTH + 8;
            }

            response.Script.Append("setTimeout(\"$('.PlacedHiddenCardScript').css('display', 'none');$('.PlacedCard').removeClass('PlaceCardScript');\", 100);");
            response.Script.Append("setTimeout(\"BoardEvent('EndEvent:").Append(gameEvent.EventId).Append("')\", 200);");
        }

        protected void GenerateTrumpCard(HtmlResponse response, Card trump)
        {
            if (getBuraGame().DealingCards == null || trump == null)
                return;
            
            int top = trumpArea.Top;
            int left = trumpArea.Left;
            string cardId = "id='CD_Trump'";
            string cardClass = "Trump";

            if (getBuraGame().DealingCards.Count > 0)
            {
                string cardImage = GetCardImage(trump);
                response.Html.AppendFormat("<div {0} class='{1}' style='top:{2}px; left:{3}px; height:{4}px; width:{5}px; position:absolute; z-index:1;'><img alt='' src='{6}' /></div>",
                    cardId, cardClass, top, left, CARD_HEIGHT, CARD_HEIGHT, cardImage);
                response.Script.AppendFormat("$('.Trump img').rotate(-90);");
                response.Html.AppendFormat("<div {0} class='CD{1}' style='top:{2}px; left:{3}px; height:{4}px; width:{5}px; position:absolute; z-index:2;'><img alt='' src='{6}' /></div>",
                    "", "Cover", top, left + CARD_WIDTH / 2, CARD_HEIGHT, CARD_HEIGHT, GetHiddenCardImage());
            }
            else
            {
                string cardImage = GetCardImage(getBuraGame().Cards[trump.Type.ToString()]);
                response.Html.AppendFormat("<div {0} class='{1}' style='top:{2}px; left:{3}px; height:{4}px; width:{5}px; position:absolute; z-index:1;'><img alt='' src='{6}' /></div>",
                    cardId, cardClass, top, left, CARD_HEIGHT, CARD_HEIGHT, cardImage);
                response.Script.AppendFormat("$('.Trump img').rotate(-90);");
            }
        }

        protected void GenerateTakenCards(HtmlResponse response, int currentPlayerId)
        {
            foreach (int playerId in Game.Players.Keys)
            {
                BuraPlayer player = getBuraGame().getPlayer(playerId);
                HtmlArea area = (playerId == currentPlayerId) ? takenCardsAreas[0] : takenCardsAreas[1];
                int left = area.Left;
                int top = area.Top + 20;
                int zIndex = 15;
                string cardImage = GetHiddenCardImage();

                int remainingCards = player.TakenCards.Count;
                // cards on board must not be applied to taken cards
                if (getBuraGame().LastCardTakerPlayer != null && getBuraGame().LastCardTakerPlayer.PlayerId == playerId)
                {
                    remainingCards -= 2 * player.PlacedCards.Count;
                }
                bool alternativePack = currentPlayerId != playerId;
                while (remainingCards > 0)
                {
                    if (remainingCards <= 2)
                    {
                        response.Html.AppendFormat("<div class='TakenCard' style='top:{0}px; left:{1}px; height:{2}px; width:{3}px; z-index:{4}; position:absolute;'><img alt='' src='{5}' /></div>",
                            top, left + CARD_WIDTH / 2, CARD_HEIGHT, CARD_WIDTH, zIndex, cardImage);

                        remainingCards -= 1;
                    }
                    else
                    {
                        string cardName = alternativePack ? "Cards/TakenCards1.png" : "Cards/TakenCards2.png";
                        string takenCardsImage = GetImageUrl(cardName);
                        response.Html.AppendFormat("<div class='TakenCard' style='top:{0}px; left:{1}px; height:{2}px; width:{3}px; z-index:{4}; position:absolute;'><img alt='' src='{5}' /></div>",
                            top, left, CARD_HEIGHT, CARD_WIDTH, zIndex, takenCardsImage);
                        
                        remainingCards -= 3;
                    }
                    zIndex--;
                    left--;
                    top--;
                }
            }
        }

        protected void GenerateDoublingOfferScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = GameDoubling.Items[((GameDoubling) gameEvent.EventValue).DoublingValue];
            if (doubling.DoublingValue < GameDoubling.MAX_DOUBLING_VALUE)
            {
                response.Html.Append(
                    BuraMessage.GetMessage(
                        string.Format("Player offers Doubling: {0}", doubling.DoublingText),
                        new MessageOption[3]{
                            new MessageOption("Accept", string.Format("BoardEvent(\"DoublingAccept:{0}\")", gameEvent.EventId), "Red"),
                            new MessageOption(string.Format("{0}", GameDoubling.Items[doubling.DoublingValue].DoublingText), string.Format("BoardEvent(\"DoublingReDouble:{0}\")", gameEvent.EventId), "Blue"),
                            new MessageOption("Reject", string.Format("BoardEvent(\"DoublingReject:{0}\")", gameEvent.EventId), "Green")}));
            } else {
                response.Html.Append(
                    BuraMessage.GetMessage(
                        string.Format("Player offers Doubling: {0}", doubling.DoublingText),
                        new MessageOption[2]{
                            new MessageOption("Accept", string.Format("BoardEvent(\"DoublingAccept:{0}\")", gameEvent.EventId), "Red"),
                            new MessageOption("Reject", string.Format("BoardEvent(\"DoublingReject:{0}\")", gameEvent.EventId), "Blue")}));
            }
        }

        protected void GenerateContinueQuestionEventScript(HtmlResponse response, GameEvent gameEvent)
        {
            response.Html.Append(
                BuraMessage.GetMessage(
                    string.Format("Choose your Option"),
                    new MessageOption[2]{
                            new MessageOption("Continue", string.Format("BoardEvent(\"Continue:{0}\")", gameEvent.EventId), "Red"),
                            new MessageOption("Show Cards", string.Format("BoardEvent(\"ShowCards:{0}\")", gameEvent.EventId), "Green")}));
        }

        protected void GenerateShowCardsEventScript(HtmlResponse response, GameEvent gameEvent)
        {
            int playerId = int.Parse(gameEvent.EventValue.ToString());
            BuraPlayer player = getBuraGame().getPlayer(playerId);
            HtmlArea area = showCardsArea;
            response.Html.AppendFormat("<div class='ShowCards' style='position:absolute; top:{0}px; left:{1}px; width:{2}px; height:{3}px; z-index:50;'>",
                area.Top, area.Left, area.Width, area.Height)
                .AppendFormat("<span style='margin:auto;'>");
            int totalValue = 0;
            foreach (Card card in player.TakenCards)
            {
                string cardImage = GetCardImage(card);
                response.Html.AppendFormat("<span><img alt='' src='{0}' /></span>", cardImage);
                totalValue += card.Value;
            }
            response.Html.Append("</span>")
                .AppendFormat("<div>{0}</div>", totalValue)
                .Append("</div>");
            if (playerId != gameEvent.ViewerPlayerId) {
                response.Html.Append(
                    BuraMessage.GetMessage(string.Format("თქვენმა მოწინააღმდეგემ დააგროვა {0} ქულა", totalValue), 
                        new MessageOption[1]{new MessageOption("დახურვა", string.Format("BoardEvent(\"EndEvent:{0}\")", gameEvent.EventId), "Silver")})
                );
            } else {
                response.Html.Append(
                    BuraMessage.GetMessage(string.Format("თქვენ დააგროვეთ {0} ქულა", totalValue),
                        new MessageOption[1] { new MessageOption("დახურვა", string.Format("BoardEvent(\"EndEvent:{0}\")", gameEvent.EventId), "Silver") })
                );
            }
        }

        protected void GeneratePlayerTurnScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            if (doubling.DoublingValue < GameDoubling.MAX_DOUBLING_VALUE)
            {
                response.Html.Append(
                    BuraMessage.GetMessage(string.Empty,
                        new MessageOption[3] { 
                        new MessageOption("ჩასვლა", "placeCards(true)", "Red"),
                        new MessageOption("გატნევა", "placeCards(false)", "Blue"),
                        new MessageOption(GameDoubling.Items[doubling.DoublingValue].DoublingText, string.Format("BoardEvent(\"DoublingOffer\")"),"Silver")})
                );
            }
            else
            {
                response.Html.Append(
                    BuraMessage.GetMessage(string.Empty,
                        new MessageOption[2] { 
                        new MessageOption("ჩასვლა", "placeCards(true)", "Red"),
                        new MessageOption("გატნევა", "placeCards(false)", "Blue")})
                );
            }
        }


        protected void GeneratePlayerWinScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            response.Html.Append(
                BuraMessage.GetMessage("თქვენ მოიგეთ ეს დარიგება",
                    new MessageOption[1] { 
                        new MessageOption("დახურვა", string.Format("BoardEvent(\"EndEvent:{0}\")", gameEvent.EventId),"Silver")
                        })
            );
        }

        protected void GeneratePlayerLooseScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            response.Html.Append(
                BuraMessage.GetMessage("თქვენ წააგეთ ეს დარიგება",
                    new MessageOption[1] { 
                        new MessageOption("დახურვა", string.Format("BoardEvent(\"EndEvent:{0}\")", gameEvent.EventId),"Silver")
                        })
            );
        }

        protected void GenerateWinAndRematchScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            response.Html.Append(
                BuraMessage.GetMessage("თქვენ მოიგეთ პარტია",
                    new MessageOption[2] { 
                        new MessageOption("ახალი პარტია", string.Format("BoardEvent(\"RematchOffer:{0}\")", gameEvent.EventId),"Red"),
                        new MessageOption("მაგიდის დატოვება", string.Format("BoardEvent(\"LeaveGame\")", gameEvent.EventId),"Blue")
                        })
            );
        }

        protected void GenerateLooseAndRematchScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            response.Html.Append(
                BuraMessage.GetMessage("თქვენ წააგეთ პარტია",
                    new MessageOption[2] { 
                        new MessageOption("ახალი პარტია", string.Format("BoardEvent(\"RematchOffer:{0}\")", gameEvent.EventId),"Red"),
                        new MessageOption("მაგიდის დატოვება", string.Format("BoardEvent(\"LeaveGame\")", gameEvent.EventId),"Blue")
                        })
            );
        }

        protected void GenerateTimeoutWinScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            response.Html.Append(
                BuraMessage.GetMessage("თქვენი მოწინააღმდეგე გავარდა თამაშიდან",
                    new MessageOption[1] { 
                        new MessageOption("მაგიდის დატოვება", string.Format("BoardEvent(\"LeaveGame\")", gameEvent.EventId),"Red")
                        })
            );
        }

        protected void GenerateTimeoutLooseScript(HtmlResponse response, GameEvent gameEvent)
        {
            GameDoubling doubling = getBuraGame().Doubling;
            response.Html.Append(
                BuraMessage.GetMessage("თქვენ გავარდით თამაშიდან",
                    new MessageOption[1] { 
                        new MessageOption("მაგიდის დატოვება", string.Format("BoardEvent(\"LeaveGame\")", gameEvent.EventId),"Blue")
                        })
            );
        }

        protected void GenerateContinueGameScript(HtmlResponse response, GameEvent gameEvent)
        {
            response.Script.Append("BoardEvent('ContinueGame');");
        }

        protected void GenerateGameInfoHtml(HtmlResponse response, int playerId)
        {
            StringBuilder builder = new StringBuilder();
            builder
                .AppendFormat("თამაში <font>{0}</font>-მდე &nbsp;", getBuraGame().PlayingTill)
                .AppendFormat("<font>{0}</font> ლარზე<br />", getBuraGame().Amount)
                .AppendFormat("თამაშის ტიპი <font>{0}</font><br />", getBuraGame().PassHiddenCards ? "დახურული" : "ღია")
                .AppendFormat("მალიუტკა <font>{0}</font>", getBuraGame().StickAllowed ? "ურიგოთ" : "რიგით");
            response.Script
                .AppendFormat("$('.GameInfoDetail').html('{0}');", builder.ToString());
                //.AppendFormat("$('.DoublingInfo').html('{0}');", Game.Doubling.DoublingText);

            // draw players score and avatars
            BuraPlayer player = getBuraGame().getPlayer(playerId);
            response.Script
                .AppendFormat("$('.PlayerScore.Bottom').html('{0}');", player.Score)
                .AppendFormat("$('.PlayerAvatar.Bottom .PlayerName').html('{0}');", player.PlayerName);
            
            BuraPlayer oponent = getBuraGame().getOponentPlayer(playerId);
            if (oponent != null)
            {
                response.Script
                    .AppendFormat("$('.PlayerScore.Top').html('{0}');", oponent.Score)
                    .AppendFormat("$('.PlayerAvatar.Top .PlayerName').html('{0}');", oponent.PlayerName);
            }
        }


        protected void GenerateButtonsHtml(HtmlResponse response)
        {
            /*
            string waitingFor = "";
            bool found = false;
            foreach (int playerId in Game.Players.Keys)
            {
                if (Game.Players[playerId].Events.Count > 0)
                {
                    waitingFor += "  " + playerId;
                    found = true;
                }
            }
            if (!found)
                waitingFor += Game.PlayerTurn;

            HtmlArea area = buttonsArea;
            response.Html.AppendFormat("<div class='ButtonsDiv' style='position:absolute; top:{0}px; left:{1}px; height:{2}px; width:{3}px;'>", area.Top, area.Left, area.Height, area.Width)
                .AppendFormat("<a onclick='placeCards()'>Place Cards</a>")
                .AppendFormat("<a onclick='DoublingOffer()'>Doubling Offer</a><span>Current Game Value:{0}</span>", Game.Doubling.DoublingValue)
                .AppendFormat("<span>Waiting For Players:{0}</span>", waitingFor)
                .AppendFormat("</div>");
             * */
        }

    }

}