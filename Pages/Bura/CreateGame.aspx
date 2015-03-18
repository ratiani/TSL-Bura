<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Bura/Bura.master" AutoEventWireup="true" CodeFile="CreateGame.aspx.cs" Inherits="Gambling_Bura_CreateGame" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BuraHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BuraContentPlaceHolder" Runat="Server">
    <div>
        gameId: 
        <asp:TextBox ID="TextBoxGameId" runat="server"></asp:TextBox>
        playerId:
        <asp:TextBox ID="TextBoxPlayerId" runat="server"></asp:TextBox>
        <hr />
        <asp:Button ID="ButtonJoin" runat="server" Text="Join" 
            onclick="ButtonJoin_Click" />

        <hr />
        playTill: 
        <asp:TextBox ID="TextBoxPlayTill" runat="server"></asp:TextBox>
        amount: 
        <asp:TextBox ID="TextBoxAmount" runat="server"></asp:TextBox>
        longStyle: 
        <asp:CheckBox ID="CheckBoxLongStyle" runat="server" />
        stickAllowed:
        <asp:CheckBox ID="CheckBoxStickAllowed" runat="server" />
        passHiddenCards: 
        <asp:CheckBox ID="CheckBoxPassHiddenCards" runat="server" />
        <br />
        <asp:Button ID="ButtonCreateGame" runat="server" Text="Create" 
            onclick="ButtonCreateGame_Click" />

    </div>
</asp:Content>

