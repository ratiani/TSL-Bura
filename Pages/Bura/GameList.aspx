<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Bura/Bura.master" AutoEventWireup="true"
    CodeFile="GameList.aspx.cs" Inherits="Pages_Bura_GameList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BuraHead" runat="Server">
    <link href="../../Skins/Default/Styles/BuraLobbyStyleSheet.css" rel="stylesheet"
        type="text/css" />
    <script src="../../Scripts/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.scroll.js" type="text/javascript"></script>
    <script src="../../Scripts/ts.crystalbet.gambling.bura.js" type="text/javascript"></script>
    <script type="text/javascript">
        var p_html = '';
        $(document).ready(function () {
            $('.scrollbar').scrollbar();

            //ev.preventDefault();
            //$('#scroll_container').scrollbar('repaint');


            $('#CreateNewTable').click(function () {
                var m_width = $('.main1').width() + 8;
                var m_height = $('.main1').height() + 8;

                var html = '<div id="popup"><div id="popup_bg"></div><div id="popup_resul"></div></div>';

                $('.main1').append(html);

                $('#popup').css({ 'position': 'absolute', 'left': '0', 'top': '0', 'width': m_width, 'height': m_height, 'display': 'none', 'z-index': 100 });
                $('#popup_bg').css({ 'position': 'absolute', 'left': '0', 'top': '0', 'width': m_width, 'height': m_height, 'background': '#000', 'opacity': 0.7 });

                if (p_html == '') p_html = $('#for_popup').html();
                $('#for_popup').html('');
                $('#popup_resul').html(p_html);
                var p_width = 395;
                var p_left = (m_width - p_width) / 2;
                $('#popup_resul').css({ 'position': 'absolute', 'left': p_left, 'top': '100px' });

                $('#popup').fadeIn(200);

                InitRadioBox(new Array("NewRadioBoxGameType3", "NewRadioBoxGameType5"));
                InitRadioBox(new Array("NewRadioBoxRound3", "NewRadioBoxRound7", "NewRadioBoxRound11"));
                InitRadioBox(new Array("NewRadioBoxMalutka1", "NewRadioBoxMalutka2"));

                $('#CloseNewTable').unbind('click');
                $('#CloseNewTable').click(function () {
                    $('#popup').fadeOut(200, function () {
                        $('#popup').remove();
                    });
                });
            });

            InitCheckBox("CheckBoxFreeTables");
            InitCheckBox("CheckBoxCards3");
            InitCheckBox("CheckBoxCards5");
            InitCheckBox("CheckBoxRound3");
            InitCheckBox("CheckBoxRound7");
            InitCheckBox("CheckBoxRound11");

            InitRadioBox(new Array("RadioBoxMalutka1", "RadioBoxMalutka2"));
        });       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BuraContentPlaceHolder" runat="Server">
    <div class="main1">
        <div class="main2">
            <div class="main3">
                <div class="head">
                    <div class="head1">
                    </div>
                    <div class="head2">
                        <asp:HyperLink runat="server" ID="HyperLink1"><img alt="" src="../../Skins/Default/Images/Lobby/head2.png" /></asp:HyperLink></div>
                </div>
                <div class="c_left">
                    <div class="menu">
                        <asp:LinkButton runat="server" ID="HyperLinkAll" CssClass="menu_a1 active" OnClick="HyperLinkAll_Click">ყველა</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="HyperLink10" CssClass="menu_a2" OnClick="HyperLink10_Click">10 ლარამდე</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="HyperLink100" CssClass="menu_a3" OnClick="HyperLink100_Click">100 ლარამდე</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="HyperLink1000" CssClass="menu_a4" OnClick="HyperLink1000_Click">1000 ლარამდე</asp:LinkButton>
                    </div>
                    <div class="c_left_div">
                        <table class="c_left_div_title" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td width="94" height="25" align="center" valign="middle">
                                    პინ კოდი
                                </td>
                                <td width="200" height="25" align="center" valign="middle">
                                    მოთამაშე
                                </td>
                                <td width="102" height="25" align="center" valign="middle">
                                    ქულა
                                </td>
                                <td width="98" height="25" align="center" valign="middle">
                                    თანხა
                                </td>
                                <td width="132" height="25" align="center" valign="middle">
                                    სტატუსი
                                </td>
                            </tr>
                        </table>
                        <div class="scroll_content">
                            <div id="scroll_container" class="scrollbar simple">
                                <%
                                    for (int i = 0; i < 100; i++)
                                    {
                                %>
                                <table class="row<%=(i%2)+1 %>" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="94" height="21" align="center" valign="middle">
                                            პინ კოდი
                                        </td>
                                        <td width="200" align="center" valign="middle">
                                            მოთამაშე
                                        </td>
                                        <td width="102" align="center" valign="middle">
                                            ქულა
                                        </td>
                                        <td width="98" align="center" valign="middle">
                                            თანხა
                                        </td>
                                        <td width="132" align="center" valign="middle" class="row_r_active">
                                            სტატუსი
                                        </td>
                                    </tr>
                                </table>
                                <%
                                    }
                                %>
                            </div>
                        </div>
                        <div class="scroll_div" style="display: none;">
                            <img alt="" src="../../Skins/Default/Images/Lobby/scroll_up.png" style="position: absolute;
                                left: 0px; top: 1px;" />
                            <img alt="" src="../../Skins/Default/Images/Lobby/scroll_dragger.png" style="position: absolute;
                                left: 0px; top: 100px;" />
                            <img alt="" src="../../Skins/Default/Images/Lobby/scroll_down.png" style="position: absolute;
                                left: 0px; bottom: 1px;" />
                        </div>
                    </div>
                </div>
                <div class="c_right">
                    <div class="balansi_div">
                        <img alt="" src="../../Skins/Default/Images/Lobby/avatar.png" class="avatar_img" />
                        <div class="balansi_text1">
                            სახელი: tess</div>
                        <div class="balansi_text2">
                            ბალანსი: 1234.56 ლარი</div>
                        <asp:Button runat="server" Text="ავატარის გამოცვლა" ID="ButtonChangeAvatar" CssClass="change_avatar_bt">
                        </asp:Button>
                    </div>
                    <div class="filtri_div">
                        <div class="filter_title">
                            ფილტრი</div>
                        <asp:Button runat="server" Text="ჩვენება" ID="ButtonShow" 
                            CssClass="filter_show" onclick="ButtonShow_Click">
                        </asp:Button>
                        <asp:Button runat="server" Text="გასუფთავება" ID="ButtonClear" CssClass="filter_clear">
                        </asp:Button>
                        <div style="padding: 41px 0 0 60px; height: 16px; line-height: 16px;">
                            <asp:Label ID="CheckBoxFreeTables" runat="server" CssClass="check_label">თავისუფალი მაგიდები</asp:Label>
                        </div>
                        <div style="padding: 17px 0 0 38px; height: 16px; line-height: 16px; font-size: 12px;
                            font-weight: bold; color: #FFF;">
                            კარტები&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="CheckBoxCards3" runat="server" CssClass="check_label" >3 კარტა</asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="CheckBoxCards5" runat="server" CssClass="check_label" >5 კარტა</asp:Label>
                        </div>
                        <div style="padding: 20px 0 0 37px; height: 16px; line-height: 16px; font-size: 12px;
                            font-weight: bold; color: #FFF;">
                            რაუნდი&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="CheckBoxRound3" runat="server" CssClass="check_label" >3</asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="CheckBoxRound7" runat="server" CssClass="check_label" >7</asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="CheckBoxRound11" runat="server" CssClass="check_label" >11</asp:Label>
                        </div>
                        <div style="padding: 20px 0 0 37px; height: 17px; line-height: 17px; font-size: 12px;
                            font-weight: bold; color: #FFF;">
                            მალიუტკა&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="RadioBoxMalutka1" runat="server" CssClass="radio_label" >ურიგოთ</asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="RadioBoxMalutka2" runat="server" CssClass="radio_label" >რიგით</asp:Label>
                        </div>
                    </div>
                    <div class="new_table_div">
                        <div class="new_table_title">
                            მაგიდის შექმნა</div>
                        <div id="CreateNewTable" class="new_table_bt">
                            ახალი მაგიდა</div>
                    </div>
                    <div>
                        <asp:HyperLink runat="server" ID="HyperLink2"><img alt="" src="../../Skins/Default/Images/Lobby/b_ruletka.png" /></asp:HyperLink></div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div id="for_popup" style="display: none;">
        <div class="popup_div">
            <div id="CloseNewTable" class="popup_close">
            </div>
            <div style="position: absolute; left: 181px; top: 70px; font-size: 12px; color: #FFF;
                font-weight: bold;">
                <asp:DropDownList runat="server" ID="DropDownList1">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;ლარი
            </div>
            <div style="position: absolute; left: 175px; top: 104px;">
                <asp:Label ID="NewRadioBoxGameType3" runat="server" CssClass="radio_label" >3 კარტა</asp:Label>
            </div>
            <div style="position: absolute; left: 281px; top: 104px;">
                <asp:Label ID="NewRadioBoxGameType5" runat="server" CssClass="radio_label" >5 კარტა</asp:Label>
            </div>
            <div style="position: absolute; left: 175px; top: 135px;">
                <asp:Label ID="NewRadioBoxRound3" runat="server" CssClass="radio_label" >3</asp:Label>
            </div>
            <div style="position: absolute; left: 245px; top: 135px;">
                <asp:Label ID="NewRadioBoxRound7" runat="server" CssClass="radio_label" >7</asp:Label>
            </div>
            <div style="position: absolute; left: 315px; top: 135px;">
                <asp:Label ID="NewRadioBoxRound11" runat="server" CssClass="radio_label" >11</asp:Label>
            </div>
            <div style="position: absolute; left: 175px; top: 169px;">
                <asp:Label ID="NewRadioBoxMalutka1" runat="server" CssClass="radio_label" >ურიგოთ</asp:Label>
            </div>
            <div style="position: absolute; left: 285px; top: 169px;">
                <asp:Label ID="NewRadioBoxMalutka2" runat="server" CssClass="radio_label" >რიგით</asp:Label>
            </div>
            <asp:Button runat="server" Text="მაგიდის შექმნა" ID="ButtonCreateTable" 
                CssClass="popup_bt" onclick="ButtonCreateTable_Click">
            </asp:Button>
        </div>
    </div>
</asp:Content>
