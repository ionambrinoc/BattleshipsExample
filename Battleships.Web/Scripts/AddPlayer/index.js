window.battleships = window.battleships || {};
window.battleships.addPlayer = window.battleships.addPlayer || {};
window.battleships.addPlayer.index = (function($, undefined) {
    return {
        buttonOpen: function() {
            $('#uploadPlayerFileBtn').on('click', function() { $('input[id=File]').click(); });
            $('input[id=File]').change(function() {
                $('#textBoxCoverLeft').val($(this).val());
            });
            $('#uploadAvatarBtn').on('click', function() { $('input[id=Picture]').click(); });
            $('input[id=Picture]').change(function() {
                $('#textBoxCoverRight').val($(this).val());
            });
        }
    };
})(jQuery);