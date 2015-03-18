<%@ Page Language="C#" %>
<%@ Import Namespace="System" %> 
<%@ Import Namespace="System.IO" %> 
<%@ Import Namespace="System.Text" %> 
<%@ Import Namespace="System.Drawing" %> 
<%@ Import Namespace="System.Drawing.Imaging" %> 
<%@ Import Namespace="System.Drawing.Text" %> 
<%@ Import Namespace="System.Drawing.Drawing2D" %> 
<%@ Import Namespace="TS.Gambling.Bura" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">


    private void Page_Load(object sender, System.EventArgs e)
    {
        int gameId = int.Parse(Request["gameId"].ToString());
        int playerId = int.Parse(Request["playerId"].ToString());
        BuraGame game = BuraGameController.CurrentInstanse.GetGame(gameId);
        
        if (game != null && game.getPlayer(playerId) != null)
        {
            byte[] b = game.getPlayer(playerId).Avatar;
            Response.Clear();
            Response.ContentType = "image/png";
            Response.BinaryWrite(b);
            Response.Flush();
        }
    }
 
</script>