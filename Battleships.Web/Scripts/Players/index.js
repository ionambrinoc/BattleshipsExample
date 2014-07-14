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

    function deleteFirstPlayer() {
        $("#firstPlayer").text("First player");
        firstPlayerExists = false;
        firstPlayerId = null;
    }

    function deleteSecondPlayer() {
        $("#secondPlayer").text("Second player");
        secondPlayerExists = false;
        secondPlayerId = null;
    }

    function setUpButton() {
        addToGame($(this).data('player-id'), $(this).data('player-name'));
    }

    return {
        init: function() {

            $("#firstPlayerDelete").click(deleteFirstPlayer);
            $("#secondPlayerDelete").click(deleteSecondPlayer);

            $('#play').click(function() {
                var url = "/Players/Challenge?playerOneId=" + firstPlayerId.toString() + "&playerTwoId=" + secondPlayerId.toString();
                window.location.href = url;
            });


            $('#playerTable button').each(function() { $(this).on('click', setUpButton); });
        }
    };
})(jQuery);