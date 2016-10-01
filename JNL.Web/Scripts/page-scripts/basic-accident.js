(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        columns: ['OccurrenceTime', 'Place', 'ResponseBureau', 'ResponseDepot', 'AccidentType', 'Summary', 'Id'],
        builds: [
        {
            targets: [5],
            onCreateCell: function (columnValue) {
                columnValue = columnValue + '';
                if (columnValue && columnValue.length > 30) {
                    columnValue = columnValue.substr(0, 30) + '...';
                }

                //return columnValue;
                return '<td style="justify-content:space-between">{0}</td>'.format(columnValue);
            }
        },
        {
            targets: [6],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Basic/AccidentDetail/{0}" title="点击查看详情"><i class="small mdi-content-link"></i></a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'AddTime',
            Desending: true
        },

        getConditions: function () {
            var conditions = [];

            var month = +$('#searchMonth').val();
            if (month > 0) {
                conditions.push('DATEPART(MONTH, OccurrenceTime)=' + month);
            }

            var searchStart = $('#searchStar').val();
            if (searchStart) {
                var startDate = new Date(searchStart).format('yyyy-MM-dd');
                conditions.push('AddTime > \'{0}\''.format(startDate));
            }
            var searchEnd = $('#searchEnd').val();
            if (searchEnd) {
                var endDate = new Date(searchEnd);
                endDate.setHours(23);
                endDate.setMinutes(59);
                endDate.setSeconds(59);
                conditions.push('AddTime < \'{0}\''.format(endDate.format('yyyy-MM-dd HH:mm:ss')));
            }

            var place = $('#searchPlace').val();
            if (place) {
                conditions.push('Place LIKE \'%{0}%\''.format(place));
            }
            var reason = $('#searchReason').val();
            if (reason) {
                conditions.push('Reason LIKE \'%{0}%\''.format(reason));
            }

            $('.same-condition').each(function() {
                var value = $(this).val(),
                    field = $(this).data('field');
                if (value) {
                    conditions.push('{0} LIKE \'%{1}%\''.format(field, value));
                }
            });

            return conditions;
        },

        selectConfigs: [
            { type: 7, selector: '#searchType', value: 'Name' },
            { type: 8, selector: '#searchLocoType', value: 'Name' },
            { type: 9, selector: '#searchWeather', value: 'Name' },
            { type: 15, selector: '#searchDepot', value: 'Name' },
            { type: 16, selector: '#searchBereau', value: 'Name' },
            { type: 17, selector: '#searchPlace', value: 'Name' }
        ],

        init: function () {

            this.ajaxParams.TableName = 'Accident';
            this.ajaxParams.PageIndex = 1;
            this.ajaxParams.PageSize = 20;

            var _this = this;
            _this.commonTable = $.commonTable('.accident-table', {
                columns: _this.columns,
                builds: _this.builds,
                ajaxParams: _this.ajaxParams,
                getConditions: _this.getConditions
            });

            $('.datepicker').pickadate({
                selectMonths: true, // Creates a dropdown to control month
                selectYears: 15, // Creates a dropdown of 15 years to control year
                format: 'yyyy-mm-dd'
            });

            common.loadDictionaries(_this.selectConfigs);
        },

        bindEvents: function () {
            var _this = this;
            $('#btnSearch').on('click', function () {
                _this.commonTable.setPageIndex(1);
                _this.commonTable.loadData();
            });
        }
    };
})(window, jQuery);