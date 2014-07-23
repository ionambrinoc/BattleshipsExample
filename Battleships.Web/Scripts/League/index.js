window.battleships = window.battleships || {};
window.battleships.league = window.battleships.league || {};

window.battleships.league.index = (function($, undefined) {
    var resetGameButton = $('#resetGameButton');
    var leaderboardAccordion = $('#accordion');
    var runLeagueButton = $('#runLeagueButton');
    var runLeagueForm = $('#run-league');

    var setupUi = $('[data-ui-phase="setup"]');
    var inProgressUi = $('[data-ui-phase="in-progress"]');
    var resultsUi = $('[data-ui-phase="results"]');


    function resetGame() {
        setupUi.show();
        inProgressUi.hide();
        resultsUi.hide();

        leaderboardAccordion.remove('h3');
    }

    function startLeague() {
        setupUi.hide();
        inProgressUi.show();
    }

    return {
        init: function() {
            resetGame();
            runLeagueButton.click(function() {
                startLeague();
                runLeagueForm.ajaxSubmit(function(data) {
                    for (var i = 0; i < data.length; i++) {
                        var header = $('<h3>');
                        var span = $('<span>');
                        span.text(data[i].Name + '\t' + data[i].Wins);
                        var playerGameStatsDiv = $('<div>');
                        playerGameStatsDiv.attr('id', 'player' + data[i].Id);
                        header.append(span).append(playerGameStatsDiv);
                        leaderboardAccordion.append(header);
                    }
                    leaderboardAccordion.accordion();

                    inProgressUi.hide();
                    resultsUi.show();
                });
            });
            resetGameButton.click(resetGame);
        }
    };
})(jQuery);