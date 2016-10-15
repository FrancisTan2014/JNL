(function(window, $) {
    $(function() {

        page.init();

        page.bindEvents();

    });
    
    window.page = {
        
        // 初始化页面上的组件
        init: function () {

            // 时间选择器
            common.pickdate();

            // 初始化部门选择下拉列表
            var selectedDepartId = $('#DepartmentId').data('default') || -1;
            $.initSelect({
                selectors: ['#DepartmentId'],
                ajaxUrl: '/Common/GetList',
                getAjaxParams: function () {
                    return { TableName: 'Department' };
                },
                textField: 'Name',
                valueField: 'Id',
                selectedValue: selectedDepartId,
                afterBuilt: function ($select) {
                    $select.material_select();
                }
            });

            // 初始化字典型下拉列表
            var defaultFlag = $('#WorkFlagId').data('default') || -1;
            var defaultType = $('#WorkTypeId').data('default') || -1;
            var defaultPosition = $('#PositionId').data('default') || -1;
            var defaultPolitical = $('#PoliticalStatusId').data('default') || -1;
            common.loadDictionaries([
                { type: 1, selector: '#WorkFlagId', value: 'Id', selected: defaultFlag },
                { type: 3, selector: '#WorkTypeId', value: 'Id', selected: defaultType },
                { type: 6, selector: '#PositionId', value: 'Id', selected: defaultPosition },
                { type: 4, selector: '#PoliticalStatusId', value: 'Id', selected: defaultPolitical }
            ]);
            
            // jquery.validation配置
            $('.form-validate').validate({
                rules: {
                    Name: { required: true },
                    SalaryId: { required: true },
                    WorkId: { required: true },
                    HireDate: { required: true },
                    Identity: { checkIdentity: true }
                },
                messages: {
                    Name: { required: '姓名不能为空' },
                    SalaryId: { required: '工号不能为空' },
                    WorkId: { required: '工作证号不能为空' },
                    HireDate: { required: '入职日期不能为空' }
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

        }, // end init

        // 验证表单中的select选项是否有效
        selectIsValid: function() {

            var valid = true;
            $('.form-validate select.initialized').each(function() {
                var $select = $(this),
                    value = $select.val();

                if (value <= 0) {
                    valid = false;

                    var name = $select.parent().parent().siblings('label').text().replace(/\s/g, '');
                    var msg = '请选择{0}:(='.format(name);
                    Materialize.toast(msg, 3000);

                    return false;
                }
            });

            return valid;
        },

        // 为页面上特定元素绑定事件
        bindEvents: function() {
            
            var _this = this;

            // 限制身份证号码最多只能输入18位
            // 同时自动填充生日
            $('#Identity').on('input', function() {
                var value = $(this).val();

                if (value) {
                    if (value.length > 18) {
                        value = value.substr(0, 18);
                        $(this).val(value);
                    }

                    var birthDate = common.matchBirthDate(value);
                    $('#BirthDate').val(birthDate).prev().val(birthDate)
                                   // 修复生日上方的label在生日改变后不上去的BUG
                                   .end().change();
                }
            });

            // 返回按钮点击事件
            $('#btnBack').on('click', function() {
                _this.back();
            });

            // 表单提交事件
            $('.form-validate').on('submit', function(e) {

                e.preventDefault();

                if ($('.form-validate').valid() && _this.selectIsValid()) {
                    _this.save();
                }

            });

        },

        // 返回列表页面
        back: function() {

            location.href = '/Admin/StaffList';
        },

        // 保存
        save: function() {

            var _this = this,
                model = common.formJsonfiy('.form-validate'),
                ajaxUrl = model.Id > 0 ? '/Common/UpdateData' : '/Admin/AddStaff';

            _this.btnToggleDisable();
            common.ajax({
                url: ajaxUrl,
                data: {
                    target: 'Staff',
                    json: JSON.stringify(model)
                }
            }).done(function(res) {
                if (res.code == 100) {
                    Materialize.toast('保存成功:(=', 1000, '', function() {
                        _this.back();
                    });
                } else {
                    Materialize.toast('保存失败，请稍后重试 (>﹏<)', 3000);
                    _this.btnToggleDisable();
                }
            });
        },

        // 禁用button
        btnToggleDisable: function() {

            var $btns = $('#btnSave,#btnBack'),
                disabled = $btns.prop('disabled');

            $btns.prop('disabled', !disabled);
        }

    };

})(window, jQuery);