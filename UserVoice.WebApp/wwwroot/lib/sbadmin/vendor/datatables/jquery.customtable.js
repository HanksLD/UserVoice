(function ($) {
    $.fn.CustomTable = function (option, value) {
        var result,
		element = this.each(function () {
		    var $this = $(this);
		    if ($($this).find('thead').find('tr').first().find(".rowNum").length == 0) {
		        var html = '<th data-name="_sort" style="width:50px;" class="rowNum">序号</th>';
		        $($this).find('thead').find('tr').first().prepend(html);
		    }

		    var options = {};
		    //dataTable默认参数
		    options = $.extend({}, $.fn.CustomTable.defaults);
		    if (typeof option === 'object') {
		        options = $.extend(options, option);
		    }
		    else if (typeof option == 'string') {
		        var data = new CustomTable($this, options);
		        result = data[option](value);
		        return;
		    }
		    if (!options.columns) {
		        var columns = [];
		        if (options.columnsStart) {
		            $.extend(columns, options.columnsStart);
		        }
		        columns.push({ data: "_sort", orderable: false });
		        $('thead>tr>th[data-name]', $this).each(function (index) {
		            columns.push({ "data": $(this).data('name'), "orderable": false });
		        });
		        if (options.columnsEnd) {
		            columns = columns.concat(options.columnsEnd);
		        }
		        options.columns = columns;
		    } else {
		        var newColomns = [];
		        newColomns.push({
		            data: "_sort",
		            render: function (data, type, row) {
		                return data;
		            }
		        });
		        $(options.columns).each(function (index, item) {
		            newColomns.push(item);
		        });
		        options.columns = newColomns;
		    }

		    //表格数据全选或反选
            $('.group-checkable', $this).on('change', function () {
		        $('.checkboxes', $this).prop('checked', $(this).prop('checked'));
		        $.uniform.update();
		        $('.checkboxes', $this).parents('tr').toggleClass('selected', $(this).prop('checked'));
		    });

		    //是否单选
		    if ($this.attr('single') || options.single) {
		        $('.group-checkable', $this).remove();
		        $($this).on('click.checkboxes', '.checkboxes', function () {
		            if ($(this)[0].checked) {
		                $('.checkboxes').not(this).prop('checked', false).parents('tr').toggleClass('selected', false);
		                $.uniform.update();
		            }
		        });
		    }

		    options.ajax = ajaxBuilder(options.ajax);

		    result = $this.DataTable(options);
		    if (options.searchInput) {
		        $(options.searchInput).on('focusout', function () {
		            var searchValue = $(this).val();
		            if (searchValue) {
		                result.search(searchValue).draw(false);
		            }
		        });
		    }
		    if (options.searchButton) {
		        $(options.searchButton).on('click', function () {
		            result.ajax.reload();
		        });
		    }

		    $this.find('tbody').on('click', 'input', function (e) {
		        if (!$(e.target).hasClass('select-ignore') && $(e.target).parents('.select-ignore').length <= 0) {
		            if (!$(e.target).is('input[type="checkbox"]')) {
		                $('.checkboxes', $(this)).trigger('click');
		                $.uniform.update();
		            }
		            $(this).parents('tr').toggleClass('selected', $('.checkboxes', $(this)).prop('checked'));
		        }
		    });
		});
        return result;
    };

    var ajaxBuilder = function (opts) {
        var conf = $.extend({
            url: '',
            data: null,
            method: opts.type || 'GET'
        }, opts);

        var dataFunction = null;
        if ($.isFunction(conf.data)) {
            dataFunction = conf.data;
        }
        return function (request, drawCallback, settings) {
            if (dataFunction != null && $.isFunction(dataFunction)) {
                dataFunction.call(this, request);
                conf.data = request;
            }

            settings.conf = request;
            settings.jqXHR = $.ajax($.extend(conf,
            {
                "success": function (json) {
                    if (json.total != undefined) {
                        json = { data: json.rows, iTotalRecords: json.rows.length, iTotalDisplayRecords: json.total };
                    }
                    var error = json.error || json.sError;
                    if (error) {
                        settings.oApi._fnLog(settings, 0, error);
                    }
                    var page = settings.conf.page;
                    var pageSize = settings.conf.pageSize;
                    if (!json.data) {
                        json.data = [];
                    }
                    if (typeof (json.data) != undefined && json.data.length > 0) {
                        $(json.data).each(function (index, item) {
                            item._sort = (index + 1) + (page - 1) * pageSize;
                        });
                    }
                    settings.oApi._fnCallbackFire(settings, null, 'xhr', [settings, json]);
                    settings.json = json;
                    drawCallback(json);
                    $('input[type="checkbox"').uniform();
                }
            }));
        }
    };

    $.fn.CustomTable.defaults = {
        "processing": true,//加载数据时候是否显示进度条
        "serverSide": true,//是否从服务加载数据 
        "filter": false, //过滤功能1
        "sort": false,
        "pageLength": 15, // default records per page，
        "language": {
            "aria": {
                "sortAscending": ":升序",
                "sortDescending": ":降序"
            },
            "processing": "读取中",
            "emptyTable": "空",
            "info": "第 _START_ 到 _END_ 共 _TOTAL_ 条",
            "infoEmpty": "无数据",
            "infoFiltered": "",//"(从 _MAX_ 条数)",
            "lengthMenu": "显示 _MENU_ 条目",
            "search": "搜素:",
            "zeroRecords": "找不到结果",
            "lengthMenu": "  _MENU_ 条",
            "paginate": {
                "previous": "<",
                "next": ">",
                "last": "<<",
                "first": ">>"
            }
        },
        "orderCellsTop": false,
        "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12"l><"col-md-3 col-sm-12"i><"col-md-7 col-sm-12"p>>',
        "stateSave": false,
        "pagingType": "bootstrap_full_number",
        //自定义
        "single": false,
        "checkbox": false,
        "searchInput": null,
        "searchButton": null
    };

    var CustomTable = function (element, options) {
        this.options = options;
        this.$element = element;
    }

    CustomTable.prototype = {
        reload: function () {
            if ($.fn.DataTable.isDataTable(this.$element)) {
                this.$element.DataTable().ajax.reload();
            }
        },
        getSelected: function (ele) {
            if ($.fn.DataTable.isDataTable(this.$element)) {
                var selectedRow;
                if (ele) {
                    if (!(ele instanceof jQuery)) {
                        ele = $(ele);
                    }
                    selectedRow = ele.eq(0).parents('tr');
                }
                else {
                    selectedRow = this.$element.find('tbody .checker span.checked').eq(0).parents('tr');
                }
                if (selectedRow.length) {
                    return {
                        element: selectedRow[0],
                        data: this.$element.DataTable().row(selectedRow).data()
                    };
                }
                else {
                    return undefined;
                }
            }
        },
        getSelection: function () {
            if ($.fn.DataTable.isDataTable(this.$element)) {
                var selectedRows = this.$element.find('tbody .checker span.checked').parents('tr');
                var selection = new Array();
                var table = this.$element.DataTable();
                if (selectedRows.length) {
                    selectedRows.each(function () {
                        var data = table.row(this).data();
                        selection.push({
                            element: this,
                            data: data
                        });
                    });
                    return selection;
                }
                else {
                    return undefined;
                }
            }
        }
    }
})(window.jQuery);