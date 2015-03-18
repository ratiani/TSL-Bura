using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TS.Gambling.Bura;
using TS.Gambling.Core;
using TS.Gambling.Web;

public partial class Pages_Bura_Board : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // fill static content
            BuraGame game = (BuraGame)GameContext.GetCurrentGame();
            BuraPlayer player = (BuraPlayer)GameContext.GetCurrentPlayer();
            if (game != null && player != null)
            {
                ImageBottomPlayerAvatar.ImageUrl = VirtualPathUtility.ToAbsolute(
                        string.Format("~/Pages/Bura/ViewAvatar.aspx?gameId={0}&playerId={1}", game.GameId, player.PlayerId));
                BuraPlayer oponent = game.getOponentPlayer(player.PlayerId);
                if (oponent != null)
                {
                    ImageTopPlayerAvatar.ImageUrl = VirtualPathUtility.ToAbsolute(
                            string.Format("~/Pages/Bura/ViewAvatar.aspx?gameId={0}&playerId={1}", game.GameId, oponent.PlayerId));
                }
            }
            
            DrawBoard();
        }
    }
    private void DrawBoard()
    {
        if (GameContext.GetCurrentPlayer() == null)
            return;

        if (GameContext.GetCurrentPlayer().Events.Count > 0)
        {
            if (!GameContext.GetCurrentPlayer().Events.First().Value.EventPlayed)
            {
                GameContext.GetCurrentPlayer().Events.First().Value.EventPlayed = true;
            }
        }

        Player player = GameContext.GetCurrentPlayer();
        HtmlResponse response = ((BuraBoard)GameContext.GetCurrentGame().Board).GetBoardHtml(player.PlayerId);
        BoardPlaceHolder.Controls.Add(new LiteralControl(response.Html.ToString()));
        BoardUpdatePanel.Update();

        String script = string.Format("<script>{0}</script>", response.Script.ToString());
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "initBura", script, false);
    }

    protected void BoardEvent_Click(object sender, EventArgs e)
    {
        string eventArgument = Request["__EVENTARGUMENT"];
        if (string.IsNullOrEmpty(eventArgument))
            return;
        Player player = GameContext.GetCurrentPlayer();
        CardGame game = GameContext.GetCurrentGame();

        if (player == null || game == null)
            return;

        if (eventArgument.StartsWith("Continue:"))
        {
            int eventId = int.Parse(eventArgument.Substring("Continue:".Length));
            GameContext.GetCurrentGame().EndEvent(player.PlayerId, eventId);
            ((BuraGame)GameContext.GetCurrentGame()).ContinueGame(player.PlayerId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("TakeCard:"))
        {
            string selectedCards = eventArgument.Substring("TakeCard:".Length);
            bool result = ((BuraGame)GameContext.GetCurrentGame()).PlaceCards(player.PlayerId, selectedCards, true);
            //if (!string.IsNullOrEmpty(selectedCards) && result)
            DrawBoard();
        }
        if (eventArgument.StartsWith("PassCard:"))
        {
            string selectedCards = eventArgument.Substring("PassCard:".Length);
            bool result = ((BuraGame)GameContext.GetCurrentGame()).PlaceCards(player.PlayerId, selectedCards, false);
            //if (!string.IsNullOrEmpty(selectedCards) && result)
            DrawBoard();
        }
        if (eventArgument.StartsWith("EndEvent:"))
        {
            int eventId = int.Parse(eventArgument.Substring("EndEvent:".Length));
            GameContext.GetCurrentGame().EndEvent(player.PlayerId, eventId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("PlayerTurn:"))
        {
            int eventId = int.Parse(eventArgument.Substring("PlayerTurn:".Length));
            GameContext.GetCurrentGame().EndEvent(player.PlayerId, eventId);
            ((BuraGame)GameContext.GetCurrentGame()).PreparePlayerTurn(player.PlayerId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("DoublingOffer"))
        {
            GameContext.GetCurrentGame().DoublingOffer(player.PlayerId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("DoublingAccept:"))
        {
            int eventId = int.Parse(eventArgument.Substring("DoublingAccept:".Length));
            GameContext.GetCurrentGame().AcceptDoubling(player.PlayerId, eventId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("DoublingReDouble:"))
        {
            int eventId = int.Parse(eventArgument.Substring("DoublingReDouble:".Length));
            GameContext.GetCurrentGame().RedoubleOffer(player.PlayerId, eventId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("DoublingReject:"))
        {
            int eventId = int.Parse(eventArgument.Substring("DoublingReject:".Length));
            GameContext.GetCurrentGame().RejectDoubling(player.PlayerId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("ShowCards:"))
        {
            int eventId = int.Parse(eventArgument.Substring("ShowCards:".Length));
            GameContext.GetCurrentGame().EndEvent(player.PlayerId, eventId);
            ((BuraGame)GameContext.GetCurrentGame()).ShowPlayerCards(player.PlayerId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("RematchOffer:"))
        {
            int eventId = int.Parse(eventArgument.Substring("RematchOffer:".Length));
            ((BuraGame)GameContext.GetCurrentGame()).RematchOffer(player.PlayerId, eventId);
            DrawBoard();
        }
        if (eventArgument.StartsWith("LeaveGame"))
        {
            GameContext.SetCurrentGame(null);
            RedirectToPage("~/Pages/Bura/GameList.aspx");
        }
        if (eventArgument.StartsWith("ContinueGame"))
        {
            ((BuraGame)GameContext.GetCurrentGame()).StartGame();
            DrawBoard();
        }
    }

    protected void TimerUpdateLiveData_Tick(object sender, EventArgs e)
    {
        bool updateBoard = false;
        if (GameContext.MustUpdateGame())
        {
            updateBoard = true;
            GameContext.SetCurrentGameVersion(GameContext.GetCurrentGameVersion() + 1);
        }
        if (GameContext.GetCurrentPlayer() == null)
            return;
        if (GameContext.GetCurrentPlayer().Events.Count == 0 || !GameContext.GetCurrentPlayer().Events.First().Value.EventPlayed)
        {
            updateBoard = true;
        }
        if (updateBoard)
        {
            DrawBoard();
        }
    }

    protected void RedirectToPage(string virtualPath)
    {
        String TransferPage = string.Format(
            "<script>window.open('{0}','_self');</script>",
            VirtualPathUtility.ToAbsolute(virtualPath));
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "tempRedirect", TransferPage, false);
    }

}