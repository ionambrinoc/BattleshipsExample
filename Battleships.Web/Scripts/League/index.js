window.battleships = window.battleships || {};
window.battleships.league = window.battleships.league || {};

window.battleships.league.index = (function($, undefined) {
    var loadingSpinner = $('#loading-spinner');
    var gameSetup = $('#gameSetup');
    var resetButton = $('#resetGameButton');
    var leaderboard = $('#leaderboard');
    var noLeagueBefore = $('#noLeagueBefore');
    var latestResults = $('#latestResults');
    var noLeagueRun = $('#noLeagueRun');

    function resetGame() {
        gameSetup.show();
        loadingSpinner.hide();
        resetButton.hide();
        noLeagueRun.hide();
        latestLeaderboard();
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


        $("#leaderboard tr").remove();
        noLeagueBefore.hide();
        for (var i = 0; i < data.length; i++) {
            var playerStats = data[i];
            leaderboard.append(generatePlayerStatsHtml(playerStats));
            leaderboard.append(generateRoundStatsHtml(playerStats.RoundStats));
        }
        $('.player').on('click', togglePlayerStats);

        latestResults.show();
        leaderboard.show();
    }

    function latestLeaderboard() {
        $('#latest-league').ajaxSubmit(function (data) {
            if (data.length === 0) {
                latestResults.hide();
                leaderboard.hide();
                noLeagueBefore.show();
            } else {
                makeLeaderboard(data);
            }
        });
    }

    function runLeague() {
        $('#runLeagueButton').click(function () {
            startLeague();
            $('#run-league').ajaxSubmit(function (data) {

                loadingSpinner.hide();
                if (data.length !== 0) {
                    makeLeaderboard(data);
                } else {
                    noLeagueRun.show();
                }
                resetButton.show();
            });
        });
    }


    return {
        init: function () {

            noLeagueRun.hide();

            latestLeaderboard();

            resetGame();
            resetButton.on('click', resetGame);

            runLeague();
        }
    };
})(jQuery);