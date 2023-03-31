function DateRangePickerInitial_Start() {
    $('input[id="dateRangePickerStart"]').daterangepicker({
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
    $('input[id="dateRangePickerEnd"]').daterangepicker({
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

    $("#searchPrint_AttributeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchPrint_ActionSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchPrint_ColorSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}


$(function () {
    $(document).ready(DateRangePickerInitial_Start);
    $(document).ready(DateRangePickerInitial_End);
    $(document).ready(FormSelect_Select);
    $(document).ready(FormSelect_UnSelect);
});