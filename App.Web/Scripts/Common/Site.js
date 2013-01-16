
(function (jQuery) {

    $(function () {

        $('#more').bind('click', function () {

            var url = $('#more').data('url');
            var page = $('#more').data('nextpage');

            $('#more').hide();
            $('div.progress').show();

            $.get(url, { page: page }, function (data) {

                if ($.trim(data).length < 1) {
                    $('#more').hide();
                    $('div.progress').hide();
                    return;
                }

                $('#more').data('nextpage', parseInt($('#more').data('nextpage')) + 1);
                $('#statuses').append(data);

                $('#more').show();
                $('div.progress').hide();
            });

        });
    });

})(jQuery);