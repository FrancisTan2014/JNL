(function() {
    $(function() {
        page.bindEvents();
    });

    window.page = {
        $loginBtn: $('[type=submit]'),

        bindEvents: function () {
            var _this = this;

            $('#WorkNo,#Password').on('keyup', function() {
                var workNo = $('#WorkNo').val(),
                    password = $('#Password').val();

                if (workNo && password) {
                    _this.enableLogin();
                } else {
                    _this.disableLogin();
                }
            });

            var $switch = $('#Switch');
            $switch.next().on('click', function () {
                // 此处取非是因为点击事件会在checkbox选中之前发生
                $('input[name=Remember]').val(!$switch.prop('checked'));
            });

            $('form').on('submit', function(e) {
                e.preventDefault();

                _this.login();
            });
        },

        disableLogin: function() {
            this.$loginBtn.prop('disabled', true);
        },

        enableLogin: function() {
            this.$loginBtn.prop('disabled', false);
        },

        jump: function() {
            var backUrl = common.getQueryParam('backUrl');
            if (backUrl) {
                location.href = backUrl;
            } else {
                location.href = '/Home/Index';
            }
        },

        login: function() {
            var user = common.formJsonfiy('form'),
                _this = this;

            _this.disableLogin();
            _this.$loginBtn.text('正在登录...');
            common.ajax({
                url: '/Home/Login',
                data: user
            }).done(function (res) {
                if (res.code == 102) {
                    Materialize.toast(res.msg, 1000, '', function() {
                        _this.jump();
                    });
                    return;
                }

                _this.enableLogin();
                _this.$loginBtn.text('登 录');
                Materialize.toast(res.msg, 3000);
            });
        }
    };
})(window, jQuery);