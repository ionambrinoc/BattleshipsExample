window.battleships = window.battleships || {};
window.battleships.headToHead = window.battleships.headToHead || {};

window.battleships.headToHead.play = (function($, undefined) {
    function runGame() {
        $("#run-game-form").ajaxSubmit(function(data) {
            $("#result").text(data);
        });
    }

    return {
        init: function() {
            runGame();
        }
    };
})(jQuery);