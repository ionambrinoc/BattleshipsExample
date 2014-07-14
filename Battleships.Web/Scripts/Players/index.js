window.battleships = window.battleships || {};
window.battleships.players = window.battleships.players || {};

window.battleships.players.index = (function($, undefined) {

    var firstPlayerExists = false;
    var secondPlayerExists = false;
    var firstPlayerId;
    var secondPlayerId;

    function addToGame(id, name) {
        if (!firstPlayerExists) {
            firstPlayerId = id;
            $("#firstPlayer").text(name);
            firstPlayerExists = true;
        } else if (!secondPlayerExists) {
            secondPlayerId = id;
            $("#secondPlayer").text(name);
            secondPlayerExists = true;
        } else window.alert("There are already two players added to the game!");
    }

    function resetFirstPlayer() {
        $("#firstPlayer").text("First player");
        firstPlayerExists = false;
        firstPlayerId = null;
    }

    function resetSecondPlayer() {
        $("#secondPlayer").text("Second player");
        secondPlayerExists = false;
        secondPlayerId = null;
    }

    function setUpButton() {
        addToGame($(this).data('player-id'), $(this).data('player-name'));
    }

    return {
        init: function() {

            $("#firstPlayerReset").click(resetFirstPlayer);
            $("#secondPlayerReset").click(resetSecondPlayer);

            $('#play').click(function() {
                if (firstPlayerExists && secondPlayerExists) {
                    var url = "/Players/Challenge?playerOneId=" + firstPlayerId.toString() + "&playerTwoId=" + secondPlayerId.toString();
                    window.location.href = url;
                } else window.alert("You need two players to start the game!");
            });


            $('#playerTable button').each(function() { $(this).on('click', setUpButton); });
        }
    };
})(jQuery);