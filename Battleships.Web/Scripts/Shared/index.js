﻿window.battleships = window.battleships || {};
window.battleships.common = window.battleships.common || {};

window.battleships.common.index = (function($, undefined) {
    return {
        fade: function() {
            $('#PopupBanner').fadeOut(10000);
        }
    };
})(jQuery);