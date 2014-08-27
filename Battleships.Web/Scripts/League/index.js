window.battleships = window.battleships || {};
window.battleships.league = window.battleships.league || {};

window.battleships.league.index = (function($, undefined) {
    var loadingSpinner = $('#loading-spinner');
    var gameSetup = $('#gameSetup');
    var leaderboard = $('#leaderboard');

    function resetGame() {
        loadingSpinner.hide();
        leaderboard.show();
        gameSetup.show();
    }

    function startLeague() {
        gameSetup.hide();
        leaderboard.hide();
        loadingSpinner.show();
    }

    function togglePlayerStats() {
        $(this).next().toggle();
    }

    return {
        init: function() {
            $('.player').click(togglePlayerStats);
            resetGame();
            $('#runLeagueButton').click(startLeague);
        }
    };
})(jQuery);