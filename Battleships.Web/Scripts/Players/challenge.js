window.battleships = window.battleships || {};
window.battleships.players = window.battleships.players || {};
var firstPlayerExists = false;
var secondPlayerExists = false;
var firstPlayerId;
var secondPlayerId;

function AddToGame(id, name) {
    if (!firstPlayerExists) {
        firstPlayerId = id;
        $("firstPlayer").innerHTML = name;
        firstPlayerExists = true;
    } else if (!secondPlayerExists) {
        secondPlayerId = id;
        $("secondPlayer").innerHTML = name;
        secondPlayerExists = true;
    } else window.alert("There are already two players added to the game!");
}

function DeleteFirstPlayer() {
    $("firstPlayer").innerHTML = "First player";
    firstPlayerExists = false;
    firstPlayerId = null;
}

function DeleteSecondPlayer() {
    $("secondPlayer").innerHTML = "Second player";
    secondPlayerExists = false;
    secondPlayerId = null;
}

$('#play').click(function() {
    var url = "/Players/Challenge?playerOneId=" + firstPlayerId.toString() + "&playerTwoId=" + secondPlayerId.toString();
    window.location.href = url;
});

window.battleships.players.challenge = (function($, undefined) {
    function runGame() {
        $("#run-game-form").ajaxSubmit(function(data) {
            $("#result").text(data);
        });
    }

    return{
        init: function() {
            runGame();
        }
    };
})(jQuery);