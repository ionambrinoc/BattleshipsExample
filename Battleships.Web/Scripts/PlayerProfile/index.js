window.battleships = window.battleships || {};
window.battleships.profile = window.battleships.profile || {};

window.battleships.profile.index = (function($, undefined) {

    function generatePlayerStatsHtml(playerStats) {
        return '<tbody class="player"><tr class="info">' +
            '<td>' + playerStats.Name + '</td>' +
            '<td>' + playerStats.Wins + '</td>' +
            '<td>' + playerStats.Losses + '</td>' +
            '<td>' + playerStats.TotalRoundWins + '</td>' +
            '</tr></tbody>';
    }

    return {
        init: function() {


        }
    };
})(jQuery);