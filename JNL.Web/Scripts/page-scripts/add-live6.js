(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        init: function () {
            var _this = this;

            $.initSelect({
                selectors: '#engineType',
                ajaxUrl: '/Common/GetList',
                textField: 'Name',
                valueField: 'Id',
                getAjaxParams: function () {
                    return {
                        TableName: 'Dictionaries',
                        Conditions: 'Type=12'
                    };
                },
                afterBuilt: function ($select) {
                    $select.material_select();
                }
            });

            common.loadDictionaries([
                { type: 10, selector: '#LivingItemId', value: 'Id' },
                { type: 11, selector: '#RepairProcessId', value: 'Id' }
            ]);

            $('#dtBox').DateTimePicker({ defaultDate: new Date() });

            $.inputIntelligence('#reportStaff', _this.staffItelligenceConfig);

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
                    ReportTime: { required: true },
                    staff: { required: true },
                    StartRepair: { required: true },
                    EndRepair: { required: true },
                    BrokenPlace: textAreaRules,
                    RepairMethod: textAreaRules,
                    RepairDesc: textAreaRules,

                    RepairTeam: textAreaRules,
                    LiveItem: textAreaRules
                },
                //For custom messages
                messages: {
                    ReportTime: textAreaMsgs,
                    staff: textAreaMsgs,
                    EndRepair: textAreaMsgs,
                    StartRepair: textAreaMsgs,
                    BrokenPlace: textAreaMsgs,
                    RepairMethod: textAreaMsgs,
                    RepairDesc: textAreaMsgs,
                    
                    RepairTeam: textAreaMsgs,
                    LiveItem: textAreaMsgs
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

        bindEvents: function() {
            $('#engineType').on('change', function () {
                var parentId = $(this).val();
                $.initSelect({
                    selectors: '#modelType',
                    ajaxUrl: '/Common/GetList',
                    textField: 'Name',
                    valueField: 'Id',
                    getAjaxParams: function () {
                        return {
                            TableName: 'Dictionaries',
                            Conditions: 'Type=13 AND ParentId=' + parentId
                        };
                    },
                    beforeBuilt: function($select) {
                        $select.find('option:not(:first)').remove();
                    },
                    afterBuilt: function ($select) {
                        $select.material_select();
                    }
                });
            });

            $('#modelType').on('change', function () {
                var parentId = $(this).val();
                $.initSelect({
                    selectors: '#locoNumber',
                    ajaxUrl: '/Common/GetList',
                    textField: 'Name',
                    valueField: 'Id',
                    getAjaxParams: function () {
                        return {
                            TableName: 'Dictionaries',
                            Conditions: 'Type=14 AND ParentId=' + parentId
                        };
                    },
                    beforeBuilt: function ($select) {
                        $select.find('option:not(:first)').remove();
                    },
                    afterBuilt: function ($select) {
                        $select.material_select();
                    }
                });

                $('#locoNumber').on('change', function() {
                    $('input[name=LocomotiveId]').val($(this).val());
                });
            });

            $('#btnSave').on('click', function() {
                var $form = $('.edit-form'), 
                    $btn = $(this), 
                    isLive28 = $form.data('type') == 28;


                if ($form.valid()) {
                    var selectValued = true;
                    $('select.initialized').each(function() {
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

                    var model = common.formJsonfiy('.edit-form');
                    if (!isLive28 && !model.RepairStaffId) {
                        Materialize.toast('施修人必须从下拉提示框中选择，因此无法提交！', 3000);
                        return;
                    }

                    var target = 'LocoQuality6';
                    if (isLive28) {
                        target = 'LocoQuality28';
                    }

                    $btn.prop('disabled', true);
                    common.ajax({
                        url: '/Common/InsertData',
                        data: {
                            target: target,
                            json: JSON.stringify(model)
                        }
                    }).done(function(res) {
                        if (res.code == 100) {
                            Materialize.toast('保存成功', 1000, '', function() {
                                location.reload();
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