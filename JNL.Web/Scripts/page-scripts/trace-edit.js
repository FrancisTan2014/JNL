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

            // 追踪类别默认值选中
            var defaultType = $('#defaultType').val();
            $('#TraceType').find('option').each(function() {
                var $this = $(this);
                if ($this.val() == defaultType) {
                    $this.prop('selected', true);
                }
            });

            // 初始化上传文件组件
            $('.uploadify').uploadifive({
                'uploadScript': '/Common/FileUpload',
                'buttonText': '上传附件',
                'formData': { fileType: 4 },
                'onUploadComplete': _this.uploadCompleted
            });

            $('.uploadifive-button').addClass('btn waves-effect waves-light').css('width', '100%').find('input[type=file]').css('width', '100%');

            // 初始化时间选择器
            common.pickdate();

            // 加载选中部门
            _this.loadDeparts();
        },

        uploadCompleted: function (file, data) {
            try {
                data = $.parseJSON(data);

                $('#fileName').text(data.OriginalFileName);
                $('[name=FileName]').val(data.OriginalFileName);
                $('[name=FilePath]').val(data.FileRelativePath);

                $('.uploadifive-button').find('input[type=file]').css('width', '100%');
                $('.uploadifive-queue').empty();

                Materialize.toast('上传成功:(-', 3000);
            } catch (e) {
                console.info(e);
            }
        },

        loadDeparts: function () {
            var _this = this;
            common.ajax({
                url: '/Common/GetList',
                data: { TableName: 'Department' }
            }).done(function (res) {
                if (res.code == 108) {
                    var $departs = $('#departs');
                    res.data.forEach(function (depart) {
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

        departClick: function () {
            var id = $(this).data('id');
            if ($(this).hasClass('active')) {
                $(this).removeClass('active').prop('title', '点击选中');
                page.departs.remove(id);
            } else {
                page.departs.push(id);
                $(this).addClass('active').prop('title', '点击取消');
            }
        },

        back: function() {
            history.back();
        },

        btnToggle: function() {
            var $btns = $('.edit-form button'),
                disabled = $btns.prop('disabled');

            $btns.prop('disabled', !disabled);
        },

        bindEvents: function () {
            var _this = this;

            $('#btnBack').on('click', function () {
                _this.back();
            });

            // 保 存
            $('#btnSave').on('click', function () {
                var $form = $('.edit-form'),
                    $btn = $(this);

                var model = common.formJsonfiy($form);
                if (!model.AddTime) {
                    Materialize.toast('追踪时间不能为空:(=', 3000);
                    return;
                }

                if (model.TraceType < 0) {
                    Materialize.toast('请选择追踪类别:(=', 3000);
                    return;
                }

                if (_this.departs.length === 0) {
                    Materialize.toast('请选择责任部门:(=', 3000);
                    return;
                } else {
                    model.ResponseDepartmentIds = _this.departs.join();
                }

                if (!model.TraceContent) {
                    Materialize.toast('追踪信息内容不能为空:(=', 3000);
                    return;
                }

                var url = model.Id > 0 ? '/Common/UpdateData' : '/Common/InsertData';

                _this.btnToggle();
                common.ajax({
                    url: url,
                    data: {
                        target: 'TraceInfo',
                        json: JSON.stringify(model)
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('保存成功:(=', 1000, '', function() {
                            _this.back();
                        });

                        return;
                    }

                    _this.btnToggle();
                    Materialize.toast('保存失败，请稍后重试！', 3000);
                });
            });
        }
    };

})(window, jQuery);