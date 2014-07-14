window.battleships = window.battleships || {};
window.battleships.players = window.battleships.players || {};


window.battleships.players.challenge = (function($, undefined) {
    function runGame() {
        $("#run-game-form").ajaxSubmit(function(data) {
            $("#result").text(data);
        });
    }

    return{
        init: function() {
            runGame();
        }
    };
})(jQuery);