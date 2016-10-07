(function(window, $) {
    $(function() {
        page.editor = common.createWangEditor('Content');

        $('#btnSave').on('click', function() {
            var model = common.formJsonfiy('.edit-form');
            if (!model.Title) {
                Materialize.toast('请填写标题!', 3000);
                return;
            }

            if (model.CategoryId < 0) {
                Materialize.toast('请选择分类！', 3000);
                return;
            }

            if (!model.Content) {
                Materialize.toast('请填写内容！', 3000);
                return;
            }

            var $btn = $(this);
            $btn.prop('disabled', true);

            common.ajax({
                url: '/Common/InsertData',
                data: {
                    target: 'Article',
                    json: JSON.stringify(model)
                }
            }).done(function(res) {
                if (res.code == 100) {
                    Materialize.toast('保存成功！', 1000, '', function() {
                        location.reload();
                    });
                    return;
                }

                $btn.prop('disabled', false);
                Materialize.toast('保存失败，请稍后重试！', 3000);
            });
        });
    });

    window.page = {
        editor: null
    };
})(window, jQuery);