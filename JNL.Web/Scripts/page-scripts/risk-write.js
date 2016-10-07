(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        init: function() {
            var textAreaRules = {
                required: true,
                minlength: 10,
                maxlength: 8000
            };
            var textAreaMsgs = {
                required: '此项不能为空',
                minlength: '请输入不少于十个字',
                maxlength: '不能超过八千字'
            };
            $(".form-validate").validate({
                rules: {
                    RiskReason: textAreaRules,
                    RiskFix: textAreaRules
                },
                //For custom messages
                messages: {
                    RiskReason: textAreaMsgs,
                    RiskFix: textAreaMsgs,
                },
                errorElement: 'div',
                errorPlacement: function (error, element) {
                    var placement = $(element).data('error');
                    if (placement) {
                        $(placement).append(error);
                    } else {
                        error.insertAfter(element);
                    }
                }
            });
        },

        bindEvents: function() {
            $('#btnSubmit').on('click', function () {
                var $form = $('.edit-form'),
                    $btn = $(this);

                if ($form.valid()) {
                    var model = common.formJsonfiy('.edit-form');

                    $btn.prop('disabled', true);
                    common.ajax({
                        url: '/Risk/Write',
                        data: { json: JSON.stringify(model) }
                    }).done(function(res) {
                        if (res.code == 100) {
                            Materialize.toast('提交成功', 1000, '', function() {
                                history.back();
                            });
                            return;
                        }

                        $btn.prop('disabled', false);
                        Materialize.toast('提交失败，请稍后重试', 3000);
                    });
                }
            });
        }
    };

})(window, jQuery);