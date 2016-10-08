(function(window, $) {
    $(function() {
        $('.verify').on('click', function() {
            var $btn = $(this),
                implementId = $btn.data('id'),
                text = $btn.data('text'),
                $status = $($btn.data('target')),
                operate = $btn.data('operate'),
                isPassing = operate === 'pass';

            function toggleDisable() {
                var diabled = !$btn.prop('disabled');
                $btn.prop('disabled', diabled).siblings('.verify').prop('disabled', diabled);
            }

            toggleDisable();
            common.ajax({
                url: '/Warn/DoVerify',
                data: { id: implementId, status: isPassing ? 2 : 3 }
            }).done(function (res) {
                if (res.code == 100) {
                    Materialize.toast('操作成功！', 1000, '', function () {
                        toggleDisable();
                        $status.text(text);
                        if (isPassing) {
                            $btn.siblings('.verify').remove();
                            $btn.remove();
                        } else {
                            $btn.remove();
                        }
                    });
                } else if (res.code == 121) {
                    Materialize.toast(res.msg, 3000);
                } else {
                    Materialize.toast('操作失败，请稍后重试！', 3000);
                }
            });
        });
    });
})(window, jQuery);