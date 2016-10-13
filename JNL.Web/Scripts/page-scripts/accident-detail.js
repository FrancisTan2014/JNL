(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        $mainForm: $('.edit-form'),
        $submitBtn: $('#btnSubmit'),

        selectConfig: [
            { type: 7, selector: '#AccidentType', value: 'Name' },
            { type: 8, selector: '#LocoType', value: 'Name' },
            { type: 9, selector: '#WeatherLike', value: 'Name' },
            { type: 15, selector: '#ResponseDepot', value: 'Name' },
            { type: 16, selector: '#ResponseBureau', value: 'Name' },
            { type: 17, selector: '#Place', value: 'Name' }
        ],

        init: function () {
            common.loadDictionaries(this.selectConfig);

            common.pickdate();

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
                    Summary: textAreaRules,
                    Reason: textAreaRules,
                    Help: textAreaRules,
                    Responsibility: textAreaRules,
                    Lesson: textAreaRules
                },
                //For custom messages
                messages: {
                    OccurrenceTime: { required: '事故发生时间不能为空' },
                    Summary: textAreaMsgs,
                    Reason: textAreaMsgs,
                    Help: textAreaMsgs,
                    Responsibility: textAreaMsgs,
                    Lesson: textAreaMsgs,
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

        isSelectsValidate: function () {
            var valid = true;

            this.$mainForm.find('select').each(function () {
                var value = $(this).val();
                valid = !!value;

                if (!valid) {
                    var errormsg = $(this).find('option:first').text();
                    Materialize.toast(errormsg, 3000);
                    return false;
                }
            });

            return valid;
        },

        bindEvents: function () {
            var _this = this;
            _this.$submitBtn.on('click', function (e) {
                if (_this.$mainForm.valid() && _this.isSelectsValidate()) {
                    _this.save();
                }
            });

            $('[name=OccurrenceTime]').on('focus', function () {
                $(this).siblings('.error').hide();
            });
        },

        save: function () {
            var accident = common.formJsonfiy('.edit-form'),
                _this = this,
                forbidden = common.submitForbidden(_this.$submitBtn, '正在保存');

            common.ajax({
                url: '/Common/InsertData',
                data: {
                    target: 'Accident',
                    json: JSON.stringify(accident)
                }
            }).done(function (res) {
                if (res.code == 100) {
                    Materialize.toast('保存成功', 1000, '', function() {
                        _this.jump();
                    });
                    return;
                }

                forbidden.enabled();
                Materialize.toast('保存失败，请稍后重试', 3000);
            });
        },

        jump: function() {
            location.href = '/Basic/Accidents';
        }
    };
})(window, jQuery);