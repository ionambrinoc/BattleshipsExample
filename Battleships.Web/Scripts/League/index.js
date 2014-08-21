window.battleships = window.battleships || {};
window.battleships.league = window.battleships.league || {};

window.battleships.league.index = (function($, undefined) {
    var loadingSpinner = $('#loading-spinner');
    var gameSetup = $('#gameSetup');
    var resetButton = $('#resetGameButton');
    var leaderboard = $('#leaderboard');
    var noLeagueBefore = $('#noLeagueBefore');
    var latestResults = $('#latestResults');

    function resetGame() {
        gameSetup.show();
        loadingSpinner.hide();
   //     leaderboard.find('.player, .round-stats').remove();

        resetButton.hide();
    }

    function startLeague() {
        gameSetup.hide();
        leaderboard.hide();
        noLeagueBefore.hide();
        loadingSpinner.show();
        latestResults.hide();
    }

    function togglePlayerStats() {
        $(this).next().toggle();
    }

    function generatePlayerStatsHtml(playerStats) {
        return '<tbody class="player"><tr class="info">' +
            '<td>' + playerStats.Name + '</td>' +
            '<td>' + playerStats.Wins + '</td>' +
            '<td>' + playerStats.Losses + '</td>' +
            '<td>' + playerStats.TotalRoundWins + '</td>' +
            '</tr></tbody>';
    }

    function generateRoundStatsHtml(roundStats) {
        var roundStatsHtml = '<tbody class="round-stats">';
        for (var j = 0; j < roundStats.length; j++) {
            roundStatsHtml +=
                '<tr>' +
                '<td>vs. ' + roundStats[j].OpponentName + ' - ' + roundStats[j].Wins + ':' + roundStats[j].Losses + '</td>' +
                '<td></td><td></td><td></td>' +
                '</tr>';
        }
        roundStatsHtml += '</tbody>';
        return roundStatsHtml;
    }

    function makeLeaderboard(data) {
        for (var i = 0; i < data.length; i++) {
            var playerStats = data[i];
            leaderboard.append(generatePlayerStatsHtml(playerStats));
            leaderboard.append(generateRoundStatsHtml(playerStats.RoundStats));
        }
        $('.player').on('click', togglePlayerStats);
        noLeagueBefore.hide();
        latestResults.show();
        leaderboard.show();
    }


    return {
        init: function () {

            $('#latest-league').ajaxSubmit(function (data) {
                if (data.length === 0) {
                    noLeagueBefore.show();
                    latestResults.hide();
                    leaderboard.hide();
                } else {
                    makeLeaderboard(data);
                }
            });

            resetGame();
            resetButton.on('click', resetGame);

            $('#runLeagueButton').click(function() {
                startLeague();
                $('#run-league').ajaxSubmit(function(data) {
                    loadingSpinner.hide();
                    makeLeaderboard(data);
                    resetButton.show();
                });
            });
        }
    };
})(jQuery);