(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        columns: ['Id', 'FileName', 'FileNumber', 'PublishTime', 'FilePath'],
        builds: [
        {
            targets: [4],
            onCreateCell: function(columnValue) {
                return '<td><a href="{0}" class="btn waves-effect waves-light"><i class="mdi-editor-vertical-align-bottom"></i></a></td>'.format(columnValue);
            }
        }],
        ajaxParams: {
            OrderField: 'AddTime',
            Desending: true
        },
        getConditions: function() {
            var fileType = $('input[name=FileType]').val();
            var level = $('input[name=PublishLevel]').val();
            var conditions = ["FileType=" + fileType, "PublishLevel=" + level];

            var searchName = $('#searchName').val();
            if (searchName) {
                conditions.push('FileName LIKE \'%{0}%\''.format(searchName));
            }
            var searchNumber = $('#searchNumber').val();
            if (searchNumber) {
                conditions.push('FileNumber LIKE \'%{0}%\''.format(searchNumber));
            }
            var searchStart = $('#searchStar').val();
            if (searchStart) {
                var startDate = new Date(searchStart).format('yyyy-MM-dd');
                conditions.push('PublishTime > \'{0}\''.format(startDate));
            }
            var searchEnd = $('#searchEnd').val();
            if (searchEnd) {
                var endDate = new Date(searchEnd);
                endDate.setHours(23);
                endDate.setMinutes(59);
                endDate.setSeconds(59);
                conditions.push('PublishTime < \'{0}\''.format(endDate.format('yyyy-MM-dd HH:mm:ss')));
            }

            return conditions;
        },
        
        tips: [
            { name: '总公司', url: 'http://10.1.4.39/xxlr/ywxt/kejiaosi/gzwd/index.asp' },
            { name: '铁路局', url: 'http://10.94.4.75/Rules/' },
            { name: '机务段', url: 'http://10.94.4.75/Rules/' }
        ],

        init: function () {

            var level = $('[name=PublishLevel]').val() - 0,
                urlTips = this.tips[level - 1];
            $('#levelTips').text(urlTips.name).next()
                .text(urlTips.url).prop('href', urlTips.url);

            this.ajaxParams.TableName = 'BasicFile';
            this.ajaxParams.PageIndex = 1;
            this.ajaxParams.PageSize = 20;

            var _this = this;
            _this.commonTable = $.commonTable('#filesTable', {
                columns: _this.columns,
                builds: _this.builds,
                ajaxParams: _this.ajaxParams,
                getConditions: _this.getConditions
            });

            common.pickdate();
        },

        bindEvents: function () {
            var _this = this;
            $('#btnSearch').on('click', function() {
                _this.commonTable.setPageIndex(1);
                _this.commonTable.loadData();
            });
        }
    };
})(window, jQuery);