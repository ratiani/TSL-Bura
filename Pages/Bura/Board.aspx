<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Bura/Bura.master" AutoEventWireup="true" CodeFile="Board.aspx.cs" Inherits="Pages_Bura_Board" %>

<asp:Content ID="ContentBuraHead" ContentPlaceHolderID="BuraHead" Runat="Server">
    <link href="../../Skins/NewDesign/Styles/BuraStyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jQueryRotate.js" type="text/javascript"></script>
    <script src="../../Scripts/ts.crystalbet.gambling.bura.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="ContentBuraBody" ContentPlaceHolderID="BuraContentPlaceHolder" Runat="Server">
    <asp:ScriptManager ID="BuraScriptManager" runat="server">
    </asp:ScriptManager>

    <div id="BuraBoard" class="Board">
        <!-- fixed items -->
        <div class="Table"></div>
        <div class="BuraLogo"></div>
        <div class="PlayerScore Top">0</div>
        <div class="PlayerNameHolder Top"></div>
        <div class="PlayerName Top">Avatarich</div>
        
        <div class="PlayerScore Bottom">0</div>
        
        <div class="CardArea Top"></div>
        <div class="CardArea Bottom"></div>
        <div class="CardArea Center"></div>

        <div class="PlayerNameHolder Bottom"></div>
        <div class="PlayerName Bottom">Testovich</div>

        <div class="PlayerAvatar Top">
            <div class="Avatar">
                <asp:Image ID="ImageTopPlayerAvatar" runat="server" ImageUrl="~/Skins/Default/Images/Common/img.png" />
            </div>
        </div>
        <div class="PlayerAvatar Bottom">
            <div class="Avatar">
                <asp:Image ID="ImageBottomPlayerAvatar" runat="server" ImageUrl="~/Skins/Default/Images/Common/img1.png" />
            </div>
        </div>
        
        <div class="GameInfo"></div>
        <div class="MessageBox" id="MessageBox"></div>
        <div class="GameInfoDetail"></div>
        <div class="DoublingInfo"></div>
        <asp:UpdatePanel ID="BoardUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelTimer" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer ID="TimerUpdateLiveData" runat="server" Interval="1500" Enabled="True" 
                    ontick="TimerUpdateLiveData_Tick">
                </asp:Timer>
            </ContentTemplate>
            </asp:UpdatePanel>
            <asp:LinkButton ID="BoardEvent" runat="server" style="display:none;" onclick="BoardEvent_Click">BoardEvent</asp:LinkButton>
            <asp:PlaceHolder ID="BoardPlaceHolder" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

