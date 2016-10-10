(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        init: function () {
            var _this = this;

            $('.uploadify').uploadifive({
                'uploadScript': '?',
                'buttonText': '选择文件',
                'formData': { fileType: 3 },
                'multi': true,
                'onUploadComplete': _this.uploadCompleted
            });

            $('.uploadifive-button').addClass('btn waves-effect waves-light').css('width', '100%').find('input[type=file]').css('width', '100%');
        },

        uploadCompleted: function (file, data) {
            try {
                $('.error-box').empty();

                data = $.parseJSON(data);

                if (data.code == 117) {
                    Materialize.toast('请选择excel文件上传！', 3000);
                } else if (data.code == 110) {
                    Materialize.toast('您上传的文件格式与模板格式不一致，上传失败！', 3000);
                } else if (data instanceof Array) {
                    Materialize.toast('上传成功！', 3000);
                    data.forEach(function(item) {
                        $('.error-box').append('<p class="error-msg center">{0}</p>'.format(item));
                    });
                }

                $('.uploadifive-button').find('input[type=file]').css('width', '100%');
                $('.uploadifive-queue').empty();
            } catch (e) {
                console.info(e);
            } 
        },

        bindEvents: function() {
            
        }
    };
})(window, jQuery);