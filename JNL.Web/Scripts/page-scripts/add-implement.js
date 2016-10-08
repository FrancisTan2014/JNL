(function(window, $) {
    $(function() {
        $('#btnSave').on('click', function() {
            var detail = $('#ImplementDetail').val();
            if (!detail) {
                Materialize.toast('请填写预警内容', 3000);
                return;
            }
            
            var $btn = $(this);
            $btn.prop('disabled', true);

            var implement = common.formJsonfiy('.edit-form');
            common.ajax({
                url: '/Warn/UpdateImplement',
                data: {
                    json: JSON.stringify(implement)
                }
            }).done(function(res) {
                if (res.code == 100) {
                    Materialize.toast('保存成功！', 1000, '', function() {
                        history.back();
                    });
                    return;
                }

                $btn.prop('disabled', false);
                Materialize.toast('保存失败，请稍后重试！', 3000);
            });
        });
    });
})(window, jQuery);