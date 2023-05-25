import { CustomSweetAlert2 } from "./Shared.js";

var dataTable;
var currentOperation;
const uploadFileURL = "/Admin/System/UploadFile";
const previewEmployeeURL = "/Admin/System/PreviewEmployee";
const modalForm = "employeePreviewModal";

/**
 * 重置人事資料並預覽資料
 */
function ResetEmployeeFromFile() {
    $("#btnResetFromFile").on("click", function () {
        CustomSweetAlert2.SweetAlertTemplateHome()
            .fire({
                title: "重置",
                text: "此操作將會覆蓋所有人事資料，包含所有部門、使用者、卡片將會全部清除，屆時只存有匯入之資料，且經重置後資料無法恢復。",
            })
            .then((result) => {
                if (result.isConfirmed) {
                    bootstrap.Tooltip.getOrCreateInstance($(this)).hide(); // Hide Tooltips

                    /**
                     * 取得上傳之檔案
                     */
                    let formData = new FormData();
                    let files = $("#fileResource").get(0).files;
                    if (files.length > 0) formData.append("EmployeeSource", files[0]);

                    currentOperation = $(this).val();//操作為重置或是匯入而已

                    $.ajax({
                        url: uploadFileURL,
                        type: "POST",
                        contentType: false, // Not to set any content header
                        processData: false, // Not to process data
                        data: formData,
                        success: function (response) {
                            if (response.success) {
                                $.get(previewEmployeeURL, { formTitle: "資料預覽" }, function (data) {
                                    $("#" + modalForm).html(data);
                                    $("#" + modalForm).modal("show");
                                    ImportEmployee();
                                    EmployeePreviewDataTableInitial();
                                });
                            } else {
                                CustomSweetAlert2.SweetAlertTemplateError(response.message).fire();
                            }
                        },
                    });
                }
            });
    });
}

/**
 * 匯入人事資料並預覽資料
 */
function ImportEmployeeFromFile() {
    $("#btnImportFromFile").on("click", function () {
        bootstrap.Tooltip.getOrCreateInstance($(this)).hide(); // Hide Tooltips

        let formData = new FormData();
        let files = $("#fileResource").get(0).files;
        if (files.length > 0) formData.append("EmployeeSource", files[0]);
        currentOperation = $(this).val();

        $.ajax({
            url: uploadFileURL,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: formData,
            success: function (response) {
                if (response.success) {
                    $.get(previewEmployeeURL, { formTitle: "資料預覽" }, function (data) {
                        $("#" + modalForm).html(data);
                        $("#" + modalForm).modal("show");
                        ImportEmployee();
                        EmployeePreviewDataTableInitial();
                    });
                } else {
                    CustomSweetAlert2.SweetAlertTemplateError(response.message).fire();
                }
            },
        });
    });
}

/**
 * 預覽資料之DataTable初始化
 */
function EmployeePreviewDataTableInitial() {
    const previewEmployeeURL = "/Admin/System/PreviewEmployee";

    dataTable = $("#employeePreviewDataTable").DataTable({
        ajax: {
            url: previewEmployeeURL,
            type: "POST",
            datatype: "json",
        },
        columns: [
            { data: "card_id", name: "卡片編號" },
            { data: "dept_id", name: "部門編號" },
            { data: "dept_name", name: "部門名稱" },
            { data: "user_name", name: "姓名" },
            { data: "user_id", name: "帳號" },
            { data: "work_id", name: "工號" },
            { data: "card_type", name: "屬性" },
            { data: "enable", name: "使用狀態" },
            { data: "e_mail", name: "信箱" },
        ],
        dom: "<'row'<'col-sm-12'tr>>" + "<'row'<'col text-start'i><'col'p>>",
        paging: true,
        pagingType: "full_numbers",
        deferRender: true,
        serverSide: true,
        processing: true,
        ordering: false,
        searhcing: false,
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
        rowCallback: function (row, data) {
            data.card_type == "1"
                ? $("td:eq(6)", row).html("<b class='text-success'>遞增</b>")
                : $("td:eq(6)", row).html("<b class='text-danger'>遞減</b>");
            data.enable == "1"
                ? $("td:eq(7)", row).html("<b class='text-success'>可用</b>")
                : $("td:eq(7)", row).html("<b class='text-danger'>停用</b>");
        },
    });
    dataTable.columns.adjust().draw();
}

/**
 * 提交匯入之資料，檔案資料由後端Session暫存
 */
function ImportEmployee() {
    $("#employeePreviewForm").on("submit", function (e) {
        e.preventDefault();

        /**
         * 提交後關閉按鈕並開啟Spinner
         */
        $("#btnSubmmit").attr("disabled", true);
        $("#btnSubmmitSpinner").show();

        $.ajax({
            type: "POST",
            dataType: "JSON",
            url: "/Admin/System/ImportEmployeeFromFile",
            data: { currentOperation: currentOperation },
            success: function (data) {
                if (data.success) {
                    CustomSweetAlert2.SweetAlertTemplateSuccess("匯入成功").fire();
                }
            },
            error: function (response) {
                //出現格式等錯誤時，提示使用者出現錯誤的第一筆資料
                CustomSweetAlert2.SweetAlertTemplateError(response).fire();
            },
        }).done(function (response) {
            /**
             * 提交完成後開啟按鈕並關閉Spinner
             */
            $("#btnSubmmit").attr("disabled", false);
            $("#btnSubmmitSpinner").hide();
        });
        return false;
    });
}

$(function () {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = [...tooltipTriggerList].map((tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl));
    ResetEmployeeFromFile();
    ImportEmployeeFromFile();
});