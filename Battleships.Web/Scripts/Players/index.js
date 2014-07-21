window.battleships = window.battleships || {};
window.battleships.players = window.battleships.players || {};

window.battleships.players.index = (function($, undefined) {
    var firstPlayer = $('#firstPlayer');
    var secondPlayer = $('#secondPlayer');


    function addToGame(id, name) {
        if (!firstPlayer.data("id")) {
            firstPlayer.data("id", id);
            addPlayerNameToForm(firstPlayer, name);
        } else if (!secondPlayer.data("id")) {
            secondPlayer.data("id", id);
            addPlayerNameToForm(secondPlayer, name);
        } else {
            alert("There are already two players added to the game.");
        }
    }

    function addPlayerNameToForm(element, name) {
        element.text(name);
        element.addClass("filled");
        element.addClass("x");
    }

    function resetPlayer(element) {
        element = $(element);
        element.text(element.data("default-text"));
        element.data("id", null);
        element.removeClass("filled x onX");
    }

    function setUpButton() {
        addToGame($(this).data('player-id'), $(this).data('player-name'));
    }

    function tog(v) { return v ? 'addClass' : 'removeClass'; }

    function startGame() {
        $('#playerOneId').val(firstPlayer.data('id'));
        $('#playerTwoId').val(secondPlayer.data('id'));
        $('#gameSetup').hide();
        $('#loading-spinner').show();
    }

    function resetGame() {
        $('#resetGameButton').hide();
        $('#gameSetup').show();
        $('#loading-spinner').hide();
        $('#winner').hide();
        $('#leaderboard td').remove();
        $('#leaderboard').hide();
    }

    function startLeague() {
        $('#gameSetup').hide();
        $('#loading-spinner').show();
    }

    return {
        init: function() {
            resetGame();
            $('#runGameButton').click(function() {
                if (firstPlayer.data('id') && secondPlayer.data('id')) {
                    startGame();
                    $('#run-game-form').ajaxSubmit(function(data) {
                        $('#loading-spinner').hide();
                        $("#winner").text(data + " wins!").show();
                        $('#resetGameButton').show();
                    });
                } else {
                    alert("You need two players to start the game.");
                }
            });

            $('#runLeagueButton').click(function() {
                startLeague();
                $('#run-league').ajaxSubmit(function(data) {
                    $('#loading-spinner').hide();
                    $('#winner').text("League Results").show();
                    for (var i = 0; i < data.length; i++) {
                        $("#leaderboard").append('<tr class="text-left"><td>' + data[i].Key.Name + '</td><td>' +
                            data[i].Value.Wins + '</td><td>' + data[i].Value.Losses + '</td><td>' + data[i].Value.RoundWins + '</td></tr>');
                    }
                    $("#leaderboard").show();
                    $('#resetGameButton').show();
                });
            });

            $('#resetGameButton').on('click', resetGame);

            $('#playerTable button').each(function() { $(this).on('click', setUpButton); });

            $(document).on('mousemove', '.x', function(e) {
                $(this)[tog(this.offsetWidth - 100 < e.clientX - this.getBoundingClientRect().left)]('onX');
            }).on('click', '.onX', function() {
                resetPlayer(this);
            });
        }
    };
})(jQuery);