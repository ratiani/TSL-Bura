using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TS.Gambling.Bura;

public partial class Gambling_Bura_CreateGame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonCreateGame_Click(object sender, EventArgs e)
    {
        int playerId = int.Parse(TextBoxPlayerId.Text);
        int gameId = int.Parse(TextBoxGameId.Text);
        int playingTill = int.Parse(TextBoxPlayTill.Text);
        double amount = double.Parse(TextBoxAmount.Text);
        bool stickAllowed = CheckBoxStickAllowed.Checked;
        bool longGameStyle = CheckBoxLongStyle.Checked;
        bool passHiddenCards = CheckBoxPassHiddenCards.Checked;

        BuraGameController.CurrentInstanse.CreateGame(gameId, playerId, playingTill, amount, longGameStyle, stickAllowed, passHiddenCards);
    }
    protected void ButtonJoin_Click(object sender, EventArgs e)
    {
        BuraGameController.CurrentInstanse.JoinGame(int.Parse(TextBoxGameId.Text), int.Parse(TextBoxPlayerId.Text));
    }
}