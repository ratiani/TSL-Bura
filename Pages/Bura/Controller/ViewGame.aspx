<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Bura/Bura.master" AutoEventWireup="true"
    CodeFile="ViewGame.aspx.cs" Inherits="Pages_Bura_Controller_ViewGame" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BuraHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BuraContentPlaceHolder" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%;">
                <div style="width: 1000px; margin: auto;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="width: 200px; background-color:Silver;" valign="top">
                                            <asp:Label ID="LabelPlayer1Name" runat="server" Text=""></asp:Label>
                                            &nbsp;-&nbsp;
                                            <asp:Label ID="LabelPlayer1Score" runat="server" Text=""></asp:Label>
                                            <br />
                                            <asp:TextBox ID="TextBoxPlayer1Events" runat="server" TextMode="MultiLine" Rows="12"
                                                Width="200px" Style="border: none; background-color: Gray; color: Maroon; font-weight: bold;
                                                font-size: small;"></asp:TextBox>
                                        </td>
                                        <td valign="top" style="width:500px;">
                                            <asp:PlaceHolder ID="PlaceHolderPlayer1Cards" runat="server"></asp:PlaceHolder>
                                        </td>
                                        <td style="width: 82px">
                                            <asp:Image ID="ImagePlayer1TakenCards" runat="server" ImageUrl="~/Skins/Default/Images/Cards/Cover.png"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="2" style="background-color:Gray; font-weight:bold; color:Black; padding-left:8px;">
                                            <asp:PlaceHolder ID="PlaceHolderGameInfo" runat="server"></asp:PlaceHolder>
                                        </td>
                                        <td>
                                            <asp:PlaceHolder ID="PlaceHolderPlayer1PlacedCards" runat="server"></asp:PlaceHolder>
                                        </td>
                                        <td rowspan="2">
                                            <asp:Image ID="ImageTrump" runat="server" ImageUrl="~/Skins/Default/Images/Cards/EmptyCard.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:PlaceHolder ID="PlaceHolderPlayer2PlacedCards" runat="server"></asp:PlaceHolder>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color:Silver;" valign="bottom">
                                            <asp:Label ID="LabelPlayer2Name" runat="server" Text=""></asp:Label>
                                            &nbsp;-&nbsp;
                                            <asp:Label ID="LabelPlayer2Score" runat="server" Text=""></asp:Label>
                                            <br />
                                            <asp:TextBox ID="TextBoxPlayer2Events" runat="server" TextMode="MultiLine" Rows="12"
                                                Width="200px" Style="border: none; background-color: Gray; color: Maroon; font-weight: bold;
                                                font-size: small;"></asp:TextBox>
                                        </td>
                                        <td valign="bottom">
                                            <asp:PlaceHolder ID="PlaceHolderPlayer2Cards" runat="server"></asp:PlaceHolder>
                                        </td>
                                        <td>
                                            <asp:Image ID="ImagePlayer2TakenCards" runat="server" ImageUrl="~/Skins/Default/Images/Cards/Cover.png"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 150px; background-color: Gray; padding-left: 10px;">
                                <asp:Label ID="LabelGameVersion" runat="server" Text="Versions"></asp:Label>
                                <br />
                                1 -
                                <asp:Label ID="LabelVersionCount" runat="server" Text=""></asp:Label>
                                <br />
                                Go to :
                                <asp:TextBox ID="TextBoxGotoVersion" runat="server" Columns="3"></asp:TextBox>
                                <asp:Button ID="ButtonGoto" runat="server" Text="Ok" OnClick="ButtonGoto_Click" />
                                <br />
                                <asp:Button ID="ButtonPrevVersion" runat="server" Text="<<" OnClick="ButtonPrevVersion_Click" />
                                <asp:Button ID="ButtonNextVersion" runat="server" Text=">>" OnClick="ButtonNextVersion_Click" />
                                <hr />
                                <asp:HyperLink ID="HyperLinkGotoList" runat="server" NavigateUrl="~/Pages/Bura/Controller/ViewGameList.aspx">Game List</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
