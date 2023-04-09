function DateRangePickerInitial_Start() {
    $("#dateRangePickerStart").daterangepicker({
        "singleDatePicker": true,
        "minYear": 2015,
        "maxYear": 2023,
        "showDropdowns": true,
        "drops": "auto",
        "locale": {
            "format": "YYYY/MM/DD",
            "separator": " - ",
            "applyLabel": "確定",
            "cancelLabel": "取消",
            "fromLabel": "自",
            "toLabel": "到",
            "customRangeLabel": "Custom",
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
            ],
            "firstDay": 1
        },
        "showCustomRangeLabel": false,
        "startDate": "2023/03/30"
    }, function (start, end, label) {
        console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
    });
}

function DateRangePickerInitial_End() {
    $("#dateRangePickerEnd").daterangepicker({
        "singleDatePicker": true,
        "minYear": 2015,
        "maxYear": 2023,
        "showDropdowns": true,
        "drops": "auto",
        "locale": {
            "format": "YYYY/MM/DD",
            "separator": " - ",
            "applyLabel": "確定",
            "cancelLabel": "取消",
            "fromLabel": "自",
            "toLabel": "到",
            "customRangeLabel": "Custom",
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
            ],
            "firstDay": 1
        },
        "showCustomRangeLabel": false,
        "startDate": "2023/03/31"
    }, function (start, end, label) {
        console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
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
    })

    $("#searchPrint_DepartmentUnSelect").click(function () {
        FormSelect_Move("#departmentUnSelect", "#searchPrint_DepartmentSelect");
    })
}

function FormSelect_Select() {
    $("#searchPrint_OperationSelect").click(function () {
        FormSelect_Move("#searchPrint_OperationSelect", "#searchPrint_OperationUnSelect");
        $("#searchPrint_OperationSelect option").each(function () {
            console.log($(this).val());
        })
    })

    $("#searchPrint_DepartmentSelect").click(function () {
        FormSelect_Move("#searchPrint_DepartmentSelect", "#searchPrint_DepartmentUnSelect");
        $("#searchPrint_DepartmentSelect option").each(function () {
            console.log($(this).val());
        })

    })
}

function SearchPrintDataTableInitial() {
    var printDataTable = $("#searchPrintDataTable").DataTable({
        ajax: {
            url: "/Admin/Print",
            type: "POST",
            dataType: "json",
        },
        columns: [
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
        ],
        dom: "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { text: "輸出：", className: 'btn btn-secondary disabled' },
            { extend: "excel", className: "btn btn-success buttons-excel buttons-html5" },
            { extend: "csv", className: "btn btn-success buttons-csv buttons-html5" },
            { extend: "print", className: "btn btn-success buttons-print buttons-html5" }
        ],
        order: [9, "desc"],
        paging: true,
        deferRender: true,
        serverSide: true,
        processing: true,
        pagingType: 'full_numbers',
        language: {
            processing: "資料載入中...請稍後",
            paginate: {
                first: "首頁",
                last: "尾頁",
                previous: "上一頁",
                next: "下一頁"
            },
            info: "顯示第 _START_ 至 _END_ 筆資料，共 _TOTAL_ 筆"
        }
    });
};

function ColumnSearchPrint() {
    $("#searchPrint_Printer").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(0).search($("#searchPrint_Printer").val()).draw();

    });

    $("#searchPrint_User").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(1).search($("#searchPrint_User").val()).draw();

    });

    $("#searchPrint_Department").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(2).search($("#searchPrint_Department").val()).draw();

    });

    $("#searchPrint_Card").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(3).search($("#searchPrint_Card").val()).draw();

    });

    $("#searchPrint_AttributeSelect").change(function () {
        if ($("#searchPrint_AttributeSelect").val() != "0") {
            $("#searchPrintDataTable").DataTable().columns(4).search($("#searchPrint_AttributeSelect :selected").text()).draw();
        } else {
            $("#searchPrintDataTable").DataTable().columns(4).search("").draw();
        }
    });

    $("#searchPrint_ActionSelect").change(function () {
        if ($("#searchPrint_ActionSelect").val() != "0") {
            $("#searchPrintDataTable").DataTable().columns(5).search($("#searchPrint_ActionSelect :selected").text()).draw();
        } else {
            $("#searchPrintDataTable").DataTable().columns(5).search("").draw();
        }
    });

    $("#searchPrint_ColorSelect").change(function () {
        if ($("#searchPrint_ColorSelect").val() != "0") {
            $("#searchPrintDataTable").DataTable().columns(6).search($("#searchPrint_ColorSelect :selected").text()).draw();
        } else {
            $("#searchPrintDataTable").DataTable().columns(6).search("").draw();
        }
    });

    $("#searchPrint_Count").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(7).search($("#searchPrint_Count").val()).draw();

    });

    $("#searchPrint_Point").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(8).search($("#searchPrint_Point").val()).draw();

    });

    $("#searchPrint_PrintTime").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(9).search($("#searchPrint_PrintTime").val()).draw();

    });
    $("#searchPrint_DocumentName").keyup(function () {
        $("#searchPrintDataTable").DataTable().columns(10).search($("#searchPrint_DocumentName").val()).draw();
    });
}




$(function () {
    SearchPrintDataTableInitial();
    DateRangePickerInitial_Start();
    DateRangePickerInitial_End();
    FormSelect_Select();
    FormSelect_UnSelect();
    ColumnSearchPrint();
});