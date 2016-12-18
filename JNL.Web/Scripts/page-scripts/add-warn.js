(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        departs: [],

        init: function () {
            var _this = this;

            var defalutDeparts = $('#defaultDeparts').val();
            if (defalutDeparts) {
                _this.departs = defalutDeparts.split(',');
            }

            _this.loadDeparts();

            var defaultSource = $('#defaultSource').val();
            common.loadDictionaries([
                { type: 18, selector: '#WarningSource', value: 'Id', selected: defaultSource }
            ]);

            $('#dtBox').DateTimePicker();

            var textAreaRules = {
                required: true,
                maxlength: 8000
            };
            var textAreaMsgs = {
                required: '此项不能为空',
                minlength: '请输入不少于十个字',
                maxlength: '不能超过八千字'
            };
            $(".form-validate").validate({
                rules: {
                    WarningTitle: textAreaRules,
                    WarningContent: textAreaRules,
                    ChangeRequirement: textAreaRules
                },
                //For custom messages
                messages: {
                    WarningTitle: textAreaMsgs,
                    WarningContent: textAreaMsgs,
                    ChangeRequirement: textAreaMsgs
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

        loadDeparts: function() {
            var _this = this;
            common.ajax({
                url: '/Common/GetList',
                data: { TableName: 'Department' }
            }).done(function(res) {
                if (res.code == 108) {
                    var $departs = $('#departs');
                    res.data.forEach(function(depart) {
                        var $btn = $('<a class="btn waves-effect waves-teal btn-flat depart" data-id="{0}" title="点击选中">{1}</a>'.format(depart.Id, depart.Name));

                        if (_this.departs.contains(depart.Id)) {
                            $btn.addClass('active');
                        }

                        $btn.on('click', _this.departClick);

                        $departs.append($btn);
                    });
                }
            });
        },

        departClick: function() {
            var id = $(this).data('id');
            if ($(this).hasClass('active')) {
                $(this).removeClass('active').prop('title', '点击选中');
                page.departs.remove(id);
            } else {
                page.departs.push(id);
                $(this).addClass('active').prop('title', '点击取消');
            }
        },

        staffItelligenceConfig: {
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function (input) {
                return {
                    TableName: 'ViewStaff',
                    Fields: 'Id###WorkId###SalaryId###Name###Department',
                    PageIndex: 1,
                    PageSize: 50,
                    Conditions: '(SalaryId LIKE \'{0}%\' OR WorkId LIKE \'{0}%\' OR Name LIKE \'{0}%\')'.format(input)
                };
            },
            buildDropdownItem: function (data) {
                return '{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId);
            },
            afterSelected: function (data, $input) {
                $input.prev().val(data.Id);
            }
        },

        bindEvents: function () {
            var _this = this;

            $('#Switch').on('change', function () {
                $('#Visible').val($(this).prop('checked'));
            });

            // 保 存
            $('#btnSave').on('click', function () {
                var $form = $('.edit-form'),
                    $btn = $(this);


                if ($form.valid()) {
                    var selectValued = true;
                    $('select.initialized').each(function () {
                        var value = $(this).val();
                        if (value < 0) {
                            var msg = '请选择' + $(this).find('option:first').text();
                            selectValued = false;
                            Materialize.toast(msg, 3000);
                            return false;
                        }
                    });

                    if (!selectValued) {
                        return;
                    }

                    if (_this.departs.length === 0) {
                        Materialize.toast('请选择落实部门', 3000);
                        return;
                    }

                    var model = common.formJsonfiy('.edit-form');
                    model.ImplementDeparts = _this.departs.join();

                    var url = '/Common/InsertData';
                    if (model.Id > 0) {
                        url = '/Common/UpdateData';
                    }

                    $btn.prop('disabled', true);
                    common.ajax({
                        url: url,
                        data: {
                            target: 'Warning',
                            json: JSON.stringify(model)
                        }
                    }).done(function (res) {
                        if (res.code == 100) {
                            Materialize.toast('保存成功', 1000, '', function () {
                                if (model.Id > 0) {
                                    history.back();
                                } else {
                                    location.reload();
                                }
                            });
                            return;
                        }

                        Materialize.toast('保存失败，请稍后重试!', 3000);
                    });

                } else {
                    Materialize.toast('请完善信息', 3000);
                }
            });
        }
    };

})(window, jQuery);