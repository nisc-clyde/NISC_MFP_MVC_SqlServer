import { DataTableTemplate } from "./Shared.js"

//Global Variable - Start
var dataTable;
var dateStart;
var dateEnd;
//Global Variable - End

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

dateStart = "2005/01/01";
dateEnd = formatDate(new Date());

function DateRangePicker_Initial() {
    var dateRangePicker = $('#dateRangePicker').daterangepicker({
        "showDropdowns": true,
        ranges: {
            '今天': [moment(), moment()],
            '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            '7天前': [moment().subtract(6, 'days'), moment()],
            '30天前': [moment().subtract(29, 'days'), moment()],
            '1年前': [moment().subtract(365, 'days'), moment()],
            '當月': [moment().startOf('month'), moment().endOf('month')],
            '上個月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
        },
        "drops": "auto",
        "showCustomRangeLabel": true,
        "cancelClass": "btn-danger",
        "startDate": "2005/01/01",
        "endDate": moment(),
        "maxDate": moment(),
        "alwaysShowCalendars": true,
        "locale": {
            "format": "YYYY-MM-DD",
            "separator": " ~ ",
            "applyLabel": "確定",
            "cancelLabel": "清除",
            "fromLabel": "自",
            "toLabel": "到",
            "customRangeLabel": "自定義",
            "weekLabel": "週",
            "daysOfWeek": [
                "日",
                "一",
                "二",
                "三",
                "四",
                "五",
                "六"
            ],
            "monthNames": [
                "一月",
                "二月",
                "三月",
                "四月",
                "五月",
                "六月",
                "七月",
                "八月",
                "九月",
                "十月",
                "十一月",
                "十二月"
            ]
        }
    }, function (start, end, label) {
        dateStart = start.format('YYYY-MM-DD');
        dateEnd = end.format('YYYY-MM-DD');
    });

    dateRangePicker.on('apply.daterangepicker', function (ev, picker) {
        dataTable.columns(9).search(dateStart + "~" + dateEnd).draw();
    });

    dateRangePicker.on('cancel.daterangepicker', function (ev, picker) {
        dateRangePicker.data('daterangepicker').setStartDate('2005/01/01');
        dateRangePicker.data('daterangepicker').setEndDate(moment());
        dataTable.columns(9).search("").draw();
    });
}

function FormSelect_Move(orign, dest) {
    var selectedItem = $(orign + " option:selected");
    $(dest).append($(selectedItem).clone());
    $(selectedItem).remove();
}

function FormSelect_UnSelect() {
    $("#searchPrint_OperationUnSelect").click(function () {
        FormSelect_Move("#searchPrint_OperationUnSelect", "#searchPrint_OperationSelect");

        var operationString = [];
        $("#searchPrint_OperationSelect option:not(:first)").each(function () {
            operationString.push($(this).text());
        })

        if (operationString.length == 0) {
            dataTable.columns(5).search("AdvancedEmpty").draw();
        } else {
            dataTable.columns(5).search(operationString.join(",")).draw();
        }
    })

    $("#searchPrint_DepartmentUnSelect").click(function () {
        FormSelect_Move("#searchPrint_DepartmentUnSelect", "#searchPrint_DepartmentSelect");

        var departmentString = [];
        $("#searchPrint_DepartmentSelect option:not(:first)").each(function () {
            departmentString.push($(this).text());
        })

        if (departmentString.length == 0) {
            dataTable.columns(2).search("AdvancedEmpty").draw();
        } else {
            dataTable.columns(2).search(departmentString.join(",")).draw();
        }
    })
}

function FormSelect_Select() {
    $("#searchPrint_OperationSelect").click(function () {
        FormSelect_Move("#searchPrint_OperationSelect", "#searchPrint_OperationUnSelect");

        var operationString = [];
        $("#searchPrint_OperationSelect option:not(:first)").each(function () {
            operationString.push($(this).text());
        })

        if (operationString.length == 0) {
            dataTable.columns(5).search("AdvancedEmpty").draw();
        } else {
            dataTable.columns(5).search(operationString.join(",")).draw();
        }
    })

    $("#searchPrint_DepartmentSelect").click(function () {
        FormSelect_Move("#searchPrint_DepartmentSelect", "#searchPrint_DepartmentUnSelect");

        var departmentString = [];
        $("#searchPrint_DepartmentSelect option:not(:first)").each(function () {
            departmentString.push($(this).text());
        })

        if (departmentString.length == 0) {
            dataTable.columns(2).search("AdvancedEmpty").draw();
        } else {
            dataTable.columns(2).search(departmentString.join(",")).draw();
        }
    })
}

/**
 * Visualization the data from Database
 * @param {Element} table - The table which you want populate.
 * @param {string} url - The table resource request from.
 * @param {string} page - Identify columns index when  request to controller.
 * @param {string[]} columns - Declare all columns will exists.
 * @param {string[]} columnDefs - Custom column definition your self.
 * @param {string[]} order - Specify default orderable by column when data table already.
 * @param {function} rowCallback - Do things between response from backend and render to element, 
 */
function SearchPrintDataTableInitial() {
    const table = $("#searchPrintDataTable");
    const url = "/Admin/Print/InitialDataTable";
    const page = "print";
    const columns = [
        { data: "mfp_name", name: "事務機" },
        { data: "user_name", name: "使用人員" },
        { data: "dept_name", name: "部門" },
        { data: "card_id", name: "卡號" },
        { data: "card_type", name: "屬性" },
        { data: "usage_type", name: "動作" },
        { data: "page_color", name: "顏色" },
        { data: "page", name: "張數" },
        { data: "value", name: "使用點數" },
        { data: "print_date", name: "列印時間" },
        { data: "document_name", name: "文件名稱" }
    ];
    const columnDefs = [];
    const order = [9, "desc"];
    const rowCallback = function (row, data) {
        (data.card_type == "遞增") ? $('td:eq(4)', row).html("<b class='text-success'>遞增</b>") : $('td:eq(4)', row).html("<b class='text-danger'>遞減</b>");
        (data.page_color == "C(彩色)") ? $('td:eq(6)', row).html("<b class='rainbow-text'>C(彩色)</b>") : $('td:eq(6)', row).html("<b>M(單色)</b>");
    }

    dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, columnDefs, order, rowCallback);
};

function ColumnSearch() {
    $("#searchPrint_Printer").keyup(function () {
        dataTable.columns(0).search($("#searchPrint_Printer").val()).draw();

    });

    $("#searchPrint_User").keyup(function () {
        dataTable.columns(1).search($("#searchPrint_User").val()).draw();

    });

    $("#searchPrint_Department").keyup(function () {
        dataTable.columns(2).search($("#searchPrint_Department").val()).draw();

    });

    $("#searchPrint_Card").keyup(function () {
        dataTable.columns(3).search($("#searchPrint_Card").val()).draw();

    });

    $("#searchPrint_AttributeSelect").change(function () {
        if ($("#searchPrint_AttributeSelect").val() != "") {
            dataTable.columns(4).search($("#searchPrint_AttributeSelect :selected").text()).draw();
        } else {
            dataTable.columns(4).search("").draw();
        }
    });

    $("#searchPrint_ActionSelect").change(function () {
        if ($("#searchPrint_ActionSelect").val() != "") {
            dataTable.columns(5).search($("#searchPrint_ActionSelect :selected").text()).draw();
        } else {
            dataTable.columns(5).search("").draw();
        }
    });

    $("#searchPrint_ColorSelect").change(function () {
        if ($("#searchPrint_ColorSelect").val() != "") {
            dataTable.columns(6).search($("#searchPrint_ColorSelect :selected").text()).draw();
        } else {
            dataTable.columns(6).search("").draw();
        }
    });

    $("#searchPrint_Count").keyup(function () {
        dataTable.columns(7).search($("#searchPrint_Count").val()).draw();

    });

    $("#searchPrint_Point").keyup(function () {
        dataTable.columns(8).search($("#searchPrint_Point").val()).draw();

    });

    $("#searchPrint_PrintTime").keyup(function () {
        dataTable.columns(9).search($("#searchPrint_PrintTime").val()).draw();

    });
    $("#searchPrint_DocumentName").keyup(function () {
        dataTable.columns(10).search($("#searchPrint_DocumentName").val()).draw();
    });
}

function DateRangePickerColumnHeight() {
    $("#dateRangePickerRow").css("height", $("#operationRow").outerHeight());
}

$(function () {
    SearchPrintDataTableInitial();
    DateRangePicker_Initial();
    DateRangePickerColumnHeight();
    FormSelect_Select();
    FormSelect_UnSelect();
    ColumnSearch();
});