var dateRangePicker;
var dateStart = moment().format("YYYY-MM-DD");
var dateEnd = moment().format("YYYY-MM-DD");
function DateRangePickerInitial() {
    dateRangePicker = $("#outputReportDateRangePickerCustom").daterangepicker(
        {
            showDropdowns: true,
            ranges: {
                今天: [moment(), moment()],
                昨天: [moment().subtract(1, "days"), moment().subtract(1, "days")],
                "7天前": [moment().subtract(6, "days"), moment()],
                "30天前": [moment().subtract(29, "days"), moment()],
                "1年前": [moment().subtract(365, "days"), moment()],
                當月: [moment().startOf("month"), moment().endOf("month")],
                上個月: [moment().subtract(1, "month").startOf("month"), moment().subtract(1, "month").endOf("month")],
            },
            drops: "auto",
            showCustomRangeLabel: true,
            cancelClass: "btn-danger",
            startDate: "2005-01-01",
            endDate: moment(),
            minDate: "2005-01-01",
            maxDate: moment(),
            alwaysShowCalendars: true,
            locale: {
                format: "YYYY-MM-DD",
                separator: " ~ ",
                applyLabel: "確定",
                cancelLabel: "清除",
                fromLabel: "自",
                toLabel: "到",
                customRangeLabel: "自定義",
                weekLabel: "週",
                daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
                monthNames: [
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
                    "十二月",
                ],
            },
        },
        function (start, end, label) {
            dateStart = start.format("YYYY-MM-DD");
            dateEnd = end.format("YYYY-MM-DD");
        }
    );
}

/**
 * 修改DateRangePicker，Range之日期區間分成兩個TextBox，對所有事件取代行為
 */
function CustomDateRangePicker() {
    dateRangePicker.on("apply.daterangepicker", function (ev, picker) {
        dateStart = picker.startDate.format("YYYY-MM-DD");
        dateEnd = picker.endDate.format("YYYY-MM-DD");
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd);
    });

    dateRangePicker.on("cancel.daterangepicker", function (ev, picker) {
        dateStart = "2005-01-01";
        dateEnd = moment().format("YYYY-MM-DD");
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd);
    });

    dateRangePicker.on("hide.daterangepicker", function (ev, picker) {
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd);
    });

    dateRangePicker.on("show.daterangepicker", function (ev, picker) {
        $("#outputReportDateRangePickerStart").val(dateStart);
        $("#outputReportDateRangePickerEnd").val(dateEnd);
    });

    $("#outputReportDateRangePickerStart").val(moment().format("YYYY-MM-DD"));
    $("#outputReportDateRangePickerEnd").val(moment().format("YYYY-MM-DD"));
}

/**
 * 選擇欲搜尋之欄位資料
 */
function FormSelect_Select() {
    $("#generateOutputReport_ReportTypeSeelct").change(function () {
        $("#generateOutputReport_DepartmentSelect").val("");
        $("#generateOutputReport_UserSelect").empty().append(`<option value>${"全部"}</option>`);
        if ($(this).val().indexOf("dept") >= 0) {
            $("#generateOutputReport_UserSelect").prop("disabled", true);
        } else {
            $("#generateOutputReport_UserSelect").prop("disabled", false);
        }
    });

    $("#generateOutputReport_DepartmentSelect").change(function () {
        $.ajax({
            url: "/Admin/OutputReport/GetAllUserByDepartmentId",
            type: "GET",
            data: { departmentId: $(this).val() },
            success: function (data) {
                $("#generateOutputReport_UserSelect").empty().append(`<option value>${"全部"}</option>`);
                $.each(data.data, function (index, element) {
                    $("#generateOutputReport_UserSelect").append(`<option value= ${element.Value}>${element.Text}</option>`);
                });
            },
        });
    });

    $("#outputReportPeriodSelect").change(function () {
        $("#outputReportDateRangePickerCustom").css({ display: "none" });
        switch ($(this).find("option:selected").val()) {
            //今天
            case "0":
                $("#outputReportDateRangePickerStart").val(moment().format("YYYY-MM-DD"));
                $("#outputReportDateRangePickerEnd").val(moment().format("YYYY-MM-DD"));
                break;
            //一週
            case "1":
                $("#outputReportDateRangePickerStart").val(moment().subtract(7, "days").format("YYYY-MM-DD"));
                $("#outputReportDateRangePickerEnd").val(moment().format("YYYY-MM-DD"));
                break;
            //一個月
            case "2":
                $("#outputReportDateRangePickerStart").val(moment().subtract(1, "months").format("YYYY-MM-DD"));
                $("#outputReportDateRangePickerEnd").val(moment().format("YYYY-MM-DD"));
                break;
            //全部
            case "3":
                $("#outputReportDateRangePickerStart").val("2005-01-01");
                $("#outputReportDateRangePickerEnd").val(moment().format("YYYY-MM-DD"));
                break;
            //自訂
            case "4":
                $("#outputReportDateRangePickerCustom").css({ display: "block" });
                break;
        }
    });
}

/**
 * 產生使用量報表
 */
function GenerateUsage() {
    $("#btnGenerateUsageReport").on("click", function () {
        const reportType = $("#generateOutputReport_ReportTypeSeelct").find("option:selected").val();
        const reportColor = $("#generateOutputReport_ColorSelect").find("option:selected").val();
        const deptId = $("#generateOutputReport_DepartmentSelect").find("option:selected").val();
        const userId = $("#generateOutputReport_UserSelect").find("option:selected").val();
        const mfpIp = $("#generateOutputReport_MachineIPSelect").find("option:selected").val();
        const dateStart = $("#outputReportDateRangePickerStart").val();
        const dateEnd = $("#outputReportDateRangePickerEnd").val();

        /**
         * 開始產生報表，關閉按鈕顯示Spinner
         */
        $("#btnGenerateUsageReport").attr("disabled", true);
        $("#btnUsageSpinner").show();

        $.ajax({
            url: "/Admin/OutputReport/GenerateUsageReport",
            data: {
                reportType: reportType,
                reportColor: reportColor,
                deptId: deptId,
                userId: userId,
                mfpIp: mfpIp,
                date: dateStart + "~" + dateEnd,
            },
            type: "GET",
            success: function (data) {
                $("#outputReportDataTableDiv").html(data);
                UsageDataTableInitial();
            },
        }).done(function (response) {
            /**
             * 報表產生完成後啟用按鈕並關閉Spinner
             */
            $("#btnGenerateUsageReport").attr("disabled", false);
            $("#btnUsageSpinner").hide();
        });
    });
}

/**
 * 產生紀錄報表
 */
function GenerateRecord() {
    $("#btnGenerateRecordReport").on("click", function () {
        const reportType = $("#generateOutputReport_ReportTypeSeelct").find("option:selected").val();
        const reportColor = $("#generateOutputReport_ColorSelect").find("option:selected").val();
        const deptId = $("#generateOutputReport_DepartmentSelect").find("option:selected").val();
        const userId = $("#generateOutputReport_UserSelect").find("option:selected").val();
        const mfpIp = $("#generateOutputReport_MachineIPSelect").find("option:selected").val();
        const dateStart = $("#outputReportDateRangePickerStart").val();
        const dateEnd = $("#outputReportDateRangePickerEnd").val();

        /**
         * 開始產生報表，關閉按鈕顯示Spinner
         */
        $("#btnGenerateRecordReport").attr("disabled", true);
        $("#btnRecordSpinner").show();

        $.ajax({
            url: "/Admin/OutputReport/GenerateRecordReport",
            data: {
                reportType: reportType,
                reportColor: reportColor,
                deptId: deptId,
                userId: userId,
                mfpIp: mfpIp,
                date: dateStart + "~" + dateEnd,
            },
            type: "GET",
            success: function (data) {
                $("#outputReportDataTableDiv").html(data);
                RecordDataTableInitial();
            },
        }).done(function (response) {
            /**
             * 報表產生完成後啟用按鈕並關閉Spinner
             */
            $("#btnGenerateRecordReport").attr("disabled", false);
            $("#btnRecordSpinner").hide();
        });
    });
}

/**
 * 使用量報表DataTable初始化，設定報表
 */
function UsageDataTableInitial() {
    const previewReportURL = "/Admin/OutputReport/GenerateUsageReport";

    var usageDataTable = $("#outputReportUsageDataTable").DataTable({
        ajax: {
            url: previewReportURL,
            type: "POST",
        },
        columns: [{ data: "Name" }, { data: "SubTotal", name: "使用量" }],
        columnDefs: [
            { width: "50%", targets: 0, className: "text-start" },
            { width: "50%", target: 1, className: "text-end" },
        ],
        dom:
            "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { text: "輸出：", className: "btn btn-secondary disabled" },
            { extend: "excel", className: "btn btn-warning buttons-excel buttons-html5" },
            { extend: "csv", bom: true, className: "btn btn-warning buttons-csv buttons-html5" },
            { extend: "print", className: "btn btn-warning buttons-print buttons-html5" },
            { extend: "pageLength", attr: { id: "btnUsagePageLength" }, className: "btn btn-info buttons-html5" },
        ],
        lengthMenu: [
            [10, 25, 50, 1000, 2000],
            ["10筆", "25筆", "50筆", "1000筆", "2000筆"],
        ],
        deferRender: true,
        serverSide: true,
        processing: true,
        paging: true,
        ordering: false,
        language: {
            processing: "資料載入中...請稍後",
            paginate: {
                first: "首頁",
                last: "尾頁",
                previous: "上一頁",
                next: "下一頁",
            },
            info: "<b style='color:black;'>顯示第 _START_ 至 _END_ 筆資料，共 _TOTAL_ 筆</b>",
            zeroRecords: "找不到相符資料",
            infoFiltered: "",
        },
        initComplete: function (settings, json) {
            $("#outputReportUsageDataTable tr").last().addClass("bg-danger bg-opacity-10");
            $("#btnUsagePageLength").text("顯示 " + $("#outputReportUsageDataTable").DataTable().page.len() + " 筆");
        },
    });

    usageDataTable.on("length.dt", function (e, settings, len) {
        $("#btnUsagePageLength").text("顯示 " + $("#outputReportUsageDataTable").DataTable().page.len() + " 筆");
    });
}

/**
 * 紀錄報表DataTable初始化，設定報表
 */
function RecordDataTableInitial() {
    const previewReportURL = "/Admin/OutputReport/GenerateRecordReport";

    var recordDataTable = $("#outputReportRecordDataTable").DataTable({
        ajax: {
            url: previewReportURL,
            type: "POST",
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
            { data: "document_name", name: "文件名稱" },
        ],
        dom:
            "<'row'<'col-sm-12 col-md-12 text-start'B><'col-sm col-md'p>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { text: "輸出：", className: "btn btn-secondary disabled" },
            { extend: "excel", className: "btn btn-warning buttons-excel buttons-html5" },
            { extend: "csv", bom: true, className: "btn btn-warning buttons-csv buttons-html5" },
            {
                extend: "print",
                exportOptions: { columns: ":visible" },
                className: "btn btn-warning buttons-print buttons-html5",
            },
            { text: "選項：", className: "btn btn-secondary disabled" },
            { extend: "pageLength", attr: { id: "btnRecordPageLength" }, className: "btn btn-info buttons-html5" },
            { extend: "colvis", text: "顯示欄位", className: "btn btn-info buttons-html5" },
        ],
        lengthMenu: [
            [10, 25, 50, 100, 1000, 2000],
            ["10筆", "25筆", "50筆", "100筆", "1000筆", "2000筆"],
        ],
        deferRender: true,
        serverSide: true,
        processing: true,
        paging: true,
        ordering: false,
        language: {
            processing: "資料載入中...請稍後",
            paginate: {
                first: "首頁",
                last: "尾頁",
                previous: "上一頁",
                next: "下一頁",
            },
            info: "<b style='color:black;'>顯示第 _START_ 至 _END_ 筆資料，共 _TOTAL_ 筆</b>",
            zeroRecords: "找不到相符資料",
            infoFiltered: "",
        },
        initComplete: function (settings, json) {
            $("#btnRecordPageLength").text("顯示 " + $("#outputReportRecordDataTable").DataTable().page.len() + " 筆");
        },
    });

    recordDataTable.on("length.dt", function (e, settings, len) {
        $("#btnRecordPageLength").text("顯示 " + $("#outputReportRecordDataTable").DataTable().page.len() + " 筆");
    });
}

$(function () {
    FormSelect_Select();
    DateRangePickerInitial();
    CustomDateRangePicker();
    GenerateUsage();
    GenerateRecord();
});
