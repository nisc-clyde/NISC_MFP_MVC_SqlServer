function DateRangePickerInitial_Start() {
    $("#generateOutputReport_PeriodCustomStart").daterangepicker({
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
    $("#generateOutputReport_PeriodCustomEnd").daterangepicker({
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

function FormSelect_Select() {

    $("#generateOutputReport_ReportTypeSeelct").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#generateOutputReport_ColorSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#generateOutputReport_DepartmentSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#generateOutputReport_User").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#generateOutputReport_MachineIPSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#generateOutputReport_PeriodSelect").change(function () {
        console.log($(this).find("option:selected").val());
        if ($(this).find("option:selected").val() == 4) {
            $("#generateOutputReport_PeriodCustomBlock").css({ display: "block" });
        } else {
            $("#generateOutputReport_PeriodCustomBlock").css({ display: "none" });
        }
    })
}


$(function () {
    $(document).ready(DateRangePickerInitial_Start);
    $(document).ready(DateRangePickerInitial_End);
    $(document).ready(FormSelect_Select);

});