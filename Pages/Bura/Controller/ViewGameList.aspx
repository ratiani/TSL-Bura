<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Bura/Bura.master" AutoEventWireup="true" CodeFile="ViewGameList.aspx.cs" Inherits="Pages_Bura_Controller_ViewGameList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BuraHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BuraContentPlaceHolder" Runat="Server">
    <div style="width:100%;">
        <div style="width:800px; margin:auto;">
            <asp:TextBox ID="TextBoxGameId" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonSetGame" runat="server" Text="View Game" 
                onclick="ButtonSetGame_Click" />
            <hr />
            <asp:GridView ID="GridViewGameList" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                CellPadding="4" DataKeyNames="GameId" DataSourceID="BuraGamesDataSource" 
                ForeColor="Black" GridLines="Vertical" 
                onrowcommand="GridViewGameList_RowCommand" Width="800px">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="GameId" HeaderText="GameId" 
                        SortExpression="GameId" />
                    <asp:BoundField DataField="Player1Name" HeaderText="Player1Name" 
                        SortExpression="Player1Name" />
                    <asp:BoundField DataField="Player2Name" HeaderText="Player2Name" 
                        SortExpression="Player2Name" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" 
                        SortExpression="Amount" />
                    <asp:BoundField DataField="Score" HeaderText="Score" SortExpression="Score" />
                    <asp:BoundField DataField="PlayingTill" HeaderText="PlayingTill" 
                        SortExpression="PlayingTill" />
                    <asp:ButtonField CommandName="View Game" Text="View Game" />
                </Columns>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <asp:ObjectDataSource ID="BuraGamesDataSource" runat="server" 
                SelectMethod="GetBuraGamesList" 
                TypeName="TS.Gambling.DataProviders.BuraGameListProvider">
            </asp:ObjectDataSource>
            
        </div>
    </div>
</asp:Content>

