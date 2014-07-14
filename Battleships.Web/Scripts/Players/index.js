window.battleships = window.battleships || {};
window.battleships.players = window.battleships.players || {};

window.battleships.players.index = (function($, undefined) {

    var firstPlayerId;
    var secondPlayerId;

    function addToGame(id, name) {
        if (!firstPlayerId) {
            firstPlayerId = id;
            $("#firstPlayer").text(name);
        } else if (!secondPlayerId) {
            secondPlayerId = id;
            $("#secondPlayer").text(name);
        } else {
            alert("There are already two players added to the game.");
        }
    }

    function resetFirstPlayer() {
        $("#firstPlayer").text("First player");
        firstPlayerId = null;
    }

    function resetSecondPlayer() {
        $("#secondPlayer").text("Second player");
        secondPlayerId = null;
    }

    function setUpButton() {
        addToGame($(this).data('player-id'), $(this).data('player-name'));
    }

    return {
        init: function() {

            $("#firstPlayerReset").click(resetFirstPlayer);
            $("#secondPlayerReset").click(resetSecondPlayer);

            $('#playGame').click(function() {
                if (firstPlayerId && secondPlayerId) {
                    $('#playerOneId').val(firstPlayerId);
                    $('#playerTwoId').val(secondPlayerId);

                    $('#run-game-form').ajaxSubmit(function(data) {
                        $("#winner").text(data);
                    });
                } else {
                    alert("You need two players to start the game.");
                }
            });

            $('#playerTable button').each(function() { $(this).on('click', setUpButton); });
        }
    };
})(jQuery);