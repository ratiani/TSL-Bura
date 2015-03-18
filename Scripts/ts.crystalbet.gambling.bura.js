/*
*   TotalSoft JavaScript library for www.crystalbet.com Gambling
*   www.totalsoft.ge
*/

function BoardEvent(params) {
    __doPostBack('ctl00$ctl00$GamblingContentPlaceHolder$BuraContentPlaceHolder$BoardEvent', params);
}


var timer;
var timerPlayerId;
var timerTimeElapsed;

function StartTimer(playerId, startingElapsed, interval) {
    timerPlayerId = playerId;
    timerTimeElapsed = startingElapsed;
    StopTimer();
    timer = setInterval("PlayerTimer()", interval);
}

function PlayerTimer() {
    timerTimeElapsed++;
    if (timerTimeElapsed >= 100) {
        StopTimer();
        //alert(timerPlayerId);
        return;
    }
    $("#TimerPlayer" + timerPlayerId + " div.ElapsedContent").css("height", timerTimeElapsed.toString() + "px");
}

function StopTimer() {
    try {
        if (timer != null)
            clearInterval(timer);
    } catch (ex) {
        alert(ex);
    }
}


function initBoard() {
    $(".Card").click(
        function () {
            if ($(this).hasClass("Selected")) {
                $(this).animate({ 'top': '+=20' }, 20, 'linear', function () { $(this).removeClass("Selected"); });
            } else {
                $(this).animate({ 'top': '-=20' }, 50, 'linear', function () { $(this).addClass("Selected"); });
            }
        });

    $(".Card").mouseover(function () { if (!$(this).hasClass("Selected")) $(this).animate({ 'top': '-=10' }, 'fast', 'swing'); });
    $(".Card").mouseout(function () { if (!$(this).hasClass("Selected")) $(this).animate({ 'top': '+=10' }, 'fast', 'swing'); });
}


function getSelectedCards() {
    selectedCards = "";
    cards = $(".Card.Selected");
    for (index = 0; index < cards.length; index++) {
        selectedCards += $(cards[index]).attr("id") + ";";
    }
    return selectedCards;
}

function placeCards(takeCards) {
    if (takeCards) {
        BoardEvent("TakeCard:" + getSelectedCards());
    } else {
        BoardEvent("PassCard:" + getSelectedCards());
    }
}

function DoublingOffer() {
    BoardEvent("DoublingOffer");
}

// Lobby 

function InitCheckBox(id) {
    $('#ctl00_ctl00_GamblingContentPlaceHolder_BuraContentPlaceHolder_'+id).unbind('click');
    $('#ctl00_ctl00_GamblingContentPlaceHolder_BuraContentPlaceHolder_' + id).click(function () {

        if ($(this).hasClass('check_label active')) {
            $(this).removeClass('active');            
        } else {
            $(this).addClass('active');            
        }
    });
}

function InitRadioBox(objects) {

    for (i = 0; i < objects.length; i++) {
        id = '#ctl00_ctl00_GamblingContentPlaceHolder_BuraContentPlaceHolder_' + objects[i];
        
        $(id).unbind('click');
        $(id).click(function () {
                    
            for (j = 0; j < objects.length; j++) {
                id = '#ctl00_ctl00_GamblingContentPlaceHolder_BuraContentPlaceHolder_' + objects[j];
                $(id).removeClass('active');
            }
            if ($(this).hasClass('radio_label active')) {
                $(this).removeClass('active');
            } else {
                $(this).addClass('active'); 
            }

        }); 
    }
}