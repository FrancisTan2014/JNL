(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        $parsley: null,
        searchUrl: 'http://10.94.4.75/Rules/',

        init: function () {
            this.$parsley = $('#fileForm').parsley();

            $('#dtBox').DateTimePicker();
        },

        setFilePath: function (fileName, filePath) {
            $('input[name=FilePath]').val(filePath);
            $('input[name=FilePath]').next().val(fileName);
        },

        bindEvents: function () {
            var $filePath = $('input[name=FilePath]'),
                $fileUpload = $('.file-field'),
                _this = this;

            $('#download').on('click', function () {
                $filePath.val(_this.searchUrl);
                _this.setFilePath(_this.searchUrl, _this.searchUrl);
                $fileUpload.hide();
            });
            $('#uploadFile').on('click', function () {
                $fileUpload.show();
                _this.setFilePath('', '');
            });

            $('#fileForm').on('submit', function (e) {
                e.preventDefault();

                _this.$parsley.validate();
            });

            $('select[name=PublishLevel]').on('change', function () {
                var value = +$(this).val(),
                    $label = $('#download').next();

                if (value === 1) {
                    $label.text('去总公司网站查询');
                    _this.searchUrl = 'http://10.1.4.39/xxlr/ywxt/kejiaosi/gzwd/index.asp';
                } else {
                    $label.text('去铁路局网站查询');
                    _this.searchUrl = 'http://10.94.4.75/Rules/';
                }

                if ($('#download').is(':checked')) {
                    _this.setFilePath(_this.searchUrl, _this.searchUrl);
                }
            });

            $('.uploadify').uploadifive({
                'uploadScript': '/Common/FileUpload',
                'buttonText': '选择文件',
                'formData': { fileType: 1 },
                'onUploadComplete': _this.uploadCompleted
            });

            $('.uploadifive-button').addClass('btn waves-effect waves-light').css('width', '100%');

            $('#btnSave').on('click', function () {
                if (_this.$parsley.validate()) {
                    var fileInfo = common.formJsonfiy('#fileForm');
                    if (fileInfo.FileType < 0) {
                        Materialize.toast('请选择文件类型', 3000);
                        return;
                    }

                    if (fileInfo.PublishLevel < 0) {
                        Materialize.toast('请选择发布级别', 3000);
                        return;
                    }

                    if (!fileInfo.FilePath) {
                        Materialize.toast('请上传文件', 3000);
                        return;
                    }

                    _this.save(fileInfo);
                }
            });
        },

        uploadCompleted: function (file, data) {
            data = $.parseJSON(data);
            console.info(data);

            page.setFilePath(data.OriginalFileName, data.FileRelativePath);
            Materialize.toast('{0} 上传成功！'.format(data.OriginalFileName), 2000);

            $('.uploadifive-button .close').click();
        },

        save: function (fileInfo) {
            $('#btnSave').prop('disabled', true).text('正在保存...');

            common.ajax({
                url: '/Common/InsertData',
                data: {
                    target: 'BasicFile',
                    json: JSON.stringify(fileInfo)
                }
            }).done(function(res) {
                if (res.code == 100) {
                    var redictUrl = '/Basic/Files/{0}/{1}'.format(fileInfo.FileType, fileInfo.PublishLevel);
                    Materialize.toast('保存成功', 1000, '', function() {
                        location.href = redictUrl;
                    });

                    return;
                }

                Materialize.toast('保存失败，请稍后重试。', 2000);
                $('#btnSave').prop('disabled', false).text('保 存');
            });
        }
    };
})(window, jQuery);