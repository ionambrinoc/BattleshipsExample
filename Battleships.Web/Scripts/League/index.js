window.battleships = window.battleships || {};
window.battleships.league = window.battleships.league || {};

window.battleships.league.index = (function($, undefined) {
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

            $('#runLeagueButton').click(function() {
                startLeague();
                $('#run-league').ajaxSubmit(function(data) {
                    $('#loading-spinner').hide();
                    $('#winner').text("League Results").show();
                    for (var i = 0; i < data.length; i++) {
                        $("#leaderboard").append('<tr class="text-left"><td>' + data[i].Name + '</td><td>' +
                            data[i].Wins + '</td><td>' + data[i].Losses + '</td><td>' + data[i].RoundWins + '</td></tr>');
                    }
                    $("#leaderboard").show();
                    $('#resetGameButton').show();
                });
            });
            $('#resetGameButton').on('click', resetGame);
        }
    };
})(jQuery);