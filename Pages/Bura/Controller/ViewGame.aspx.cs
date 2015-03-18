using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TS.Gambling.Web;
using GamblingModel;
using System.Text;

public partial class Pages_Bura_Controller_ViewGame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[SessionKey.VIEW_BURA_GAME_ID] == null)
            return;
        if (! IsPostBack) {
            int gameId = int.Parse(Session[SessionKey.VIEW_BURA_GAME_ID].ToString());
            GamblingModel.Entities entity = new GamblingModel.Entities();
            var buraGame = (from c in entity.BuraGames where c.GameId == gameId select c).First();
            Session[SessionKey.VIEW_BURA_GAME] = buraGame;
            Session[SessionKey.VIEW_BURA_GAME_VERSION] = buraGame.BuraGameVersions.First().VersionNumber;
        }
        DrawGameView();
    }

    protected void DrawGameView()
    {
        int currentVersion = GetCurrentVersion();
        BuraGame buraGame = (BuraGame) Session[SessionKey.VIEW_BURA_GAME];
        int eventCount = buraGame.BuraGameVersions.Count;
        LabelVersionCount.Text = eventCount.ToString();
        TextBoxGotoVersion.Text = currentVersion.ToString();
        
        if (currentVersion >= eventCount)
            return;
        // load current version
        var gameVersion = (from c in buraGame.BuraGameVersions where c.VersionNumber == currentVersion select c).First();
        if (gameVersion != null)
        {
            // draw game info
            StringBuilder gameInfo = new StringBuilder();
            gameInfo.AppendFormat("GameId: {0}<br />", buraGame.GameId)
                .AppendFormat("GameDate: {0}<br />", buraGame.StartDate.ToString())
                .AppendFormat("PlayingTill: {0}<br />", buraGame.PlayTill)
                .AppendFormat("Amount: {0}<br />", buraGame.Amount)
                .AppendFormat("IsStickAllowed: {0}<br />", buraGame.StickAllowed)
                .AppendFormat("PassHiddenCards: {0}<br />", buraGame.PassHiddenCards)
                .AppendFormat("IsLongStyle: {0}<br />", buraGame.LongStyle)
                .AppendFormat("DoublingValue: {0}<br />", gameVersion.DoublingValue);
            if (gameVersion.DoublingOfferer != null)
                gameInfo.AppendFormat("DoublingOfferer: {0}<br />", gameVersion.DoublingOfferer.PlayerName);
            PlaceHolderGameInfo.Controls.Clear();
            PlaceHolderGameInfo.Controls.Add(new LiteralControl(gameInfo.ToString()));

            // draw version info 
            ImageTrump.ImageUrl = GetCardImage(gameVersion.Trump);

            // draw player infos
            if (gameVersion.BuraGamePlayers.Count > 0)
            {
                BuraGamePlayer player1 = gameVersion.BuraGamePlayers.First();
                DrawPlayerInfo(player1, LabelPlayer1Name, LabelPlayer1Score, TextBoxPlayer1Events, PlaceHolderPlayer1Cards, PlaceHolderPlayer1PlacedCards);
                ImagePlayer1TakenCards.Visible = gameVersion.LastCardTakerPlayer == player1.Player.PlayerId;
            }
            if (gameVersion.BuraGamePlayers.Count > 1)
            {
                BuraGamePlayer player2 = gameVersion.BuraGamePlayers.Skip(1).First();
                DrawPlayerInfo(player2, LabelPlayer2Name, LabelPlayer2Score, TextBoxPlayer2Events, PlaceHolderPlayer2Cards, PlaceHolderPlayer2PlacedCards);
                ImagePlayer2TakenCards.Visible = gameVersion.LastCardTakerPlayer == player2.Player.PlayerId;
            }
        }
    }

    protected string GetCardImage(string cardName)
    {
        if (cardName == null)
            return string.Empty;
        return string.Format("{0}{1}.png", VirtualPathUtility.ToAbsolute("~/Skins/Default/Images/Cards/"), cardName);
    }

    protected string GetHiddenCardImage()
    {
        return string.Format("{0}Cover.png", VirtualPathUtility.ToAbsolute("~/Skins/Default/Images/Cards/"));
    }

    protected void DrawPlayerInfo(BuraGamePlayer player, Label nameLabel, Label scoreLabel, TextBox eventsArea, PlaceHolder cardsHolder, PlaceHolder placedCardHolder)
    {
        nameLabel.Text = player.Player.PlayerName;
        scoreLabel.Text = player.PlayerScore.ToString();
        // draw player events 
        eventsArea.Text = string.Empty;
        foreach (BuraGameEvent playerEvent in player.Events)
        {
            eventsArea.Text += string.Format("{0};{1};{2};\n", playerEvent.EventId, playerEvent.EventType, playerEvent.EventValue);
        }

        // draw player cards 
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("<div style='position:relative; height:111px;'>");
        int left = 0;
        foreach (BuraPlayerCard card in player.PlayerCards)
        {
            string cardImage = GetCardImage(card.CardName);
            builder.AppendFormat("<div style='left:{0}px; top:0px; widht:81px; height:111px; position:absolute;'><img alt='' src='{1}' /></div>", left, cardImage);
            left += 90;
        }
        builder.AppendFormat("</div>");
        cardsHolder.Controls.Clear();
        cardsHolder.Controls.Add(new LiteralControl(builder.ToString()));

        // draw placed cards 
        builder = new StringBuilder();
        builder.AppendFormat("<div style='position:relative; height:111px;'>");
        left = 0;
        foreach (BuraPlayerPlacedCard card in player.PlacedCards)
        {
            string cardImage = GetCardImage(card.CardName);
            builder.AppendFormat("<div style='left:{0}px; top:0px; widht:81px; height:111px; position:absolute;'><img alt='' src='{1}' /></div>", left, cardImage);
            left += 90;
        }
        builder.AppendFormat("</div>");
        placedCardHolder.Controls.Clear();
        placedCardHolder.Controls.Add(new LiteralControl(builder.ToString()));
    }


    private int GetCurrentVersion()
    {
        int currentVersion = -1;
        if (Session[SessionKey.VIEW_BURA_GAME_VERSION] != null)
        {
            currentVersion = int.Parse(Session[SessionKey.VIEW_BURA_GAME_VERSION].ToString());
        }
        return currentVersion;
    }

    private void SetGameVersionNumber(int versionNumber)
    {
        Session[SessionKey.VIEW_BURA_GAME_VERSION] = versionNumber;
        DrawGameView();
    }

    protected void ButtonGoto_Click(object sender, EventArgs e)
    {
        SetGameVersionNumber(int.Parse(TextBoxGotoVersion.Text));
    }

    protected void ButtonPrevVersion_Click(object sender, EventArgs e)
    {
        SetGameVersionNumber(GetCurrentVersion() - 1);
    }
    protected void ButtonNextVersion_Click(object sender, EventArgs e)
    {
        SetGameVersionNumber(GetCurrentVersion() + 1);
    }
}