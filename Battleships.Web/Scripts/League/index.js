window.battleships = window.battleships || {};
window.battleships.league = window.battleships.league || {};

window.battleships.league.index = (function($, undefined) {
    var loadingSpinner = $('#loading-spinner');
    var gameSetup = $('#gameSetup');
    var leaderboard = $('#leaderboard');
    var noLeagueBeforeMessage = $('#noLeagueBeforeMessage');
    var leagueRunFailureMessage = $('#leagueRunFailureMessage');

    function resetGame() {
        loadingSpinner.hide();
        leagueRunFailureMessage.hide();
        gameSetup.show();
        latestLeaderboard();
    }

    function startLeague() {
        gameSetup.hide();
        leaderboard.hide();
        noLeagueBeforeMessage.hide();
        loadingSpinner.show();
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
        $("#leaderboard tbody tr").remove();
        noLeagueBeforeMessage.hide();

        for (var i = 0; i < data.length; i++) {
            var playerStats = data[i];
            leaderboard.append(generatePlayerStatsHtml(playerStats));
            leaderboard.append(generateRoundStatsHtml(playerStats.RoundStats));
        }
        $('.player').on('click', togglePlayerStats);

        leaderboard.show();
    }

    function postLeaderboard(data) {
        if (data.isUpdated) {
            makeLeaderboard(data.leaderboard);
            leaderboard.show();
        } else {
            if (data.leaderboard.length > 0) {
                leaderboard.show();
            } else {
                leaderboard.hide();
            }
            leagueRunFailureMessage.show();
        }

    }

    function latestLeaderboard() {
        $('#latest-league').ajaxSubmit(function(data) {
            if (data.length === 0) {
                leaderboard.hide();
                noLeagueBeforeMessage.show();
            } else {
                makeLeaderboard(data);
            }
        });
    }



    function runLeagueButtonSetup() {
        $('#runLeagueButton').click(function() {
            startLeague();
            $('#run-league').ajaxSubmit(function(data) {
                loadingSpinner.hide();
                postLeaderboard(data);
                gameSetup.show();
            });
        });
    }


    return {
        init: function() {
            leagueRunFailureMessage.hide();
            resetGame();
            runLeagueButtonSetup();
        }
    };
})(jQuery);