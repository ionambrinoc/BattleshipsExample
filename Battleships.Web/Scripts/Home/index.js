window.battleships = window.battleships || {};
window.battleships.index = (function ($, undefined) {
    return {
        init: function () {
            $("#accordion").accordion({ collapsible: true, active: false, heightStyle: "content" });
        }
    }
})(jQuery);
