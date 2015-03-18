using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GamblingModel;
using TS.Gambling.Bura;

public partial class Pages_Bura_GameList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //BuraGame game = new BuraGame();
        //game.BuraGameVersions.Add();
    }     

    protected void HyperLinkAll_Click(object sender, EventArgs e)
    {
        HyperLinkAll.CssClass = "menu_a1 active";
        HyperLink10.CssClass = "menu_a2";
        HyperLink100.CssClass = "menu_a3";
        HyperLink1000.CssClass = "menu_a4";
    }
    protected void HyperLink10_Click(object sender, EventArgs e)
    {
        HyperLinkAll.CssClass = "menu_a1";
        HyperLink10.CssClass = "menu_a2 active";
        HyperLink100.CssClass = "menu_a3";
        HyperLink1000.CssClass = "menu_a4";
    }
    protected void HyperLink100_Click(object sender, EventArgs e)
    {
        HyperLinkAll.CssClass = "menu_a1";
        HyperLink10.CssClass = "menu_a2";
        HyperLink100.CssClass = "menu_a3 active";
        HyperLink1000.CssClass = "menu_a4";
    }
    protected void HyperLink1000_Click(object sender, EventArgs e)
    {
        HyperLinkAll.CssClass = "menu_a1";
        HyperLink10.CssClass = "menu_a2";
        HyperLink100.CssClass = "menu_a3";
        HyperLink1000.CssClass = "menu_a4 active";
    }
    protected void ButtonCreateTable_Click(object sender, EventArgs e)
    {
        CheckBoxCards3.Text = CheckBoxFreeTables.Attributes["class"].ToString();
        /*
        int playerId = ;
        int gameId = ;
        int playingTill = ;
        double amount = ;
        bool stickAllowed = ;
        bool longGameStyle = ;
        bool passHiddenCards = ;

        BuraGameController.CurrentInstanse.CreateGame(gameId, playerId, playingTill, amount, longGameStyle, stickAllowed, passHiddenCards);
         */ 
    }
    protected void ButtonShow_Click(object sender, EventArgs e)
    {
        // TODO: Show filter
        CheckBoxCards3.Text = CheckBoxFreeTables.Attributes["class"].ToString();
    }
}