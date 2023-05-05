
var dateRangePicker;
var dateStart = "2005-01-01";
var dateEnd = moment().format("YYYY-MM-DD");
function DateRangePickerInitial() {
    dateRangePicker = $("#outputReportDateRangePickerCustom").daterangepicker({
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
        "startDate": "2005-01-01",
        "endDate": moment(),
        "minDate": "2005-01-01",
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
}

function CustomDateRangePicker() {
    dateRangePicker.on("apply.daterangepicker", function (ev, picker) {
        dateStart = picker.startDate.format('YYYY-MM-DD');
        dateEnd = picker.endDate.format('YYYY-MM-DD');
        $("#outputReportDateRangePickerStart").val(picker.startDate.format('YYYY-MM-DD'));
        $("#outputReportDateRangePickerEnd").val(picker.endDate.format('YYYY-MM-DD'))
    })

    dateRangePicker.on("cancel.daterangepicker", function (ev, picker) {
        dateStart = "2005-01-01";
        dateEnd = moment().format("YYYY-MM-DD");
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd);
    })

    dateRangePicker.on("hide.daterangepicker", function (ev, picker) {
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd);
    })

    dateRangePicker.on("show.daterangepicker", function (ev, picker) {
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd)
    })

    $("#outputReportDateRangePickerStart").val("2005-01-01");
    $("#outputReportDateRangePickerEnd").val(moment().format("YYYY-MM-DD"));
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

    $("#outputReportPeriodSelect").change(function () {
        console.log($(this).find("option:selected").val());
        if ($(this).find("option:selected").val() == 4) {
            $("#outputReportDateRangePickerCustom").css({ display: "block" });
        } else {
            $("#outputReportDateRangePickerCustom").css({ display: "none" });
        }
    })
}


$(function () {
    FormSelect_Select();
    DateRangePickerInitial();
    CustomDateRangePicker();
});