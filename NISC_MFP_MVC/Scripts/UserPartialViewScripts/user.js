import { CustomSweetAlert2, RequestDelete, DataTableTemplate } from "../AdminPartialViewScripts/Shared.js";

// 選擇使用者卡片後代入已使用點數之值
function UsedValue() {
    $("#userCardUsedValue").text($("#userCardSelect option:selected").val());
    $("#userCardSelect").on("change", function () {
        $("#userCardUsedValue").text($("#userCardSelect option:selected").val());
    })
}

function PrintJobsDataTableInitial() {
    const table = $("#printJobsDataTable");
    const url = "/User/User/PrintJobsInitial";
    const page = $("#user_idDiv").text();
    const columns = [
        { data: "date", name: "時間" },
        { data: "document", name: "文件名稱" },
        { data: "pages", name: "張數" },
        { data: "size", name: "紙張尺寸" },
        { data: "color", name: "顏色" },
        { data: "value", name: "點數" },
        {
            data: null,
            // "data.file" is property of PrintJobsModel from backend
            render: function (data, type, row) {
                return (
                    "<div class='row row-cols-sm row-cols-md row-cols-lg row-cols-xl row-cols-xxl g-2'>" +
                    "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='" +
                    data.file +
                    "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>"
                );
            },
            orderable: false,
        },
    ];
    const order = [0, "desc"];
    let dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, null, order, null);
    DeleteAlertPopUp(dataTable);
    dataTable.draw();
}

function DeleteAlertPopUp(dataTable) {

    dataTable.on("click", ".btn-delete", function (e) {
        e.preventDefault();

        const uniqueIdProperty = "file";
        const uniqueId = $(this).data("id");
        const url = "/User/User/DeleteJob";
        const currentRow = $(this).closest("tr");
        const rowData = dataTable.row(currentRow).data();

        $.get(url, { documentUid: uniqueId }, function (data) {
            var dataTableAsFormSerialize = uniqueIdProperty + "=" + uniqueId;
            dataTableAsFormSerialize += "&card_id=" + $("#userCardSelect :selected").text();
            const sweetalertHtml = $.parseHTML(data);
            const dataTableHtml = $("#deleteRowData", sweetalertHtml);
            dataTableHtml.find("th").each(function () {
                dataTableAsFormSerialize += "&" + this.id + "=" + $(this).text();
            });

            CustomSweetAlert2.SweetAlertTemplateHome().fire({
                html: data,
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: dataTableAsFormSerialize,
                        success: function (data) {
                            if (data.success) {
                                dataTable.row(currentRow).remove().draw();
                                CustomSweetAlert2.SweetAlertTemplateSuccess().fire();
                            }
                        },
                    });
                }
            });
        });
    });
}

// 載入Edit Partial View並更新User資料
function PopupFormForEdit() {
    const modalForm = "generalUserForm";
    const url = $("#" + modalForm).data("url");

    $("#btnEditUserPassword").on("click", function (e) {
        e.preventDefault();

        /**
         * 取得Partial View
         */
        $.get(url, { user_id: $("#user_idDiv").text() }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");

            /**
             * 提交更新
             */
            $("#userEditForm").on("submit", function () {
                $.validator.unobtrusive.parse($("#userEditForm"));
                if ($("#userEditForm").valid()) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: $(this).serialize(),
                        success: function (data) {
                            /**
                             * 更新成功後自動登出，否則提示告知錯誤
                             */
                            if (data.success) {
                                $("#" + modalForm).modal("hide");
                                CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire()
                                    .then((result) => {
                                        if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
                                            $.get("/User/LogOut/LogOutForJavaScript");
                                        }
                                    });
                            } else {
                                CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                            }
                        },
                    });
                }
                return false;
            });
        });
    });
}

// Render Recently Record Partial View
function PopupFormForRecentlyRecordPreview() {
    const modalForm = "generalUserForm";
    $("#btnRecentlyPrintRecord").on("click", function (e) {
        e.preventDefault();
        /**
         * 取得Partial View
         */
        $.get("/User/User/RecentlyPrintRecord", { user_id: $("#user_idDiv").text(), formTitle: $(this).text() }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");
            RecentlyPrintRecordPreviewDataTableInitial();
        });
    });

    $("#btnRecentlyDepositRecord").on("click", function (e) {
        e.preventDefault();
        /**
         * 取得Partial View
         */
        $.get("/User/User/RecentlyDepositRecord", { user_id: $("#user_idDiv").text(), formTitle: $(this).text() }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");
            RecentlyDepositRecordPreviewDataTableInitial();
        });
    });
}

// 預覽資料之PrintRecordPreviewDataTable初始化
function RecentlyPrintRecordPreviewDataTableInitial() {
    const table = $("#recentlyPrintRecordDataTable");
    const url = "/User/User/RecentlyPrintRecordDataTableInitial";
    const page = $("#user_idDiv").text() // This page variable is temporary for passing the user_id
    const columns = [
        { data: "mfp_name", name: "事務機" },
        { data: "usage_type", name: "影列印" },
        { data: "page_color", name: "顏色" },
        { data: "value", name: "使用點數" },
        { data: "document_name", name: "文件名稱" },
        { data: "print_date", name: "列印時間" },
        { data: "serial", name: "serial" },
    ];
    const columnsDefs = [
        { visible: false, target: 6 },
    ]
    const rowCallback = function (row, data) {
        (data.page_color == "C") ? $('td:eq(2)', row).html("<b class='rainbow-text'>C(彩色)</b>") : $('td:eq(2)', row).html("<b>M(單色)</b>");

        $('td:eq(5)', row).html(moment(data.print_date).format("YYYY-MM-DD hh:mm:ss"));

        (data.file_path != null && data.file_name != null) ?
            $('td:eq(10)', row).html('<a href="' + data.file_path + data.file_name + '">' + data.document_name + '</a>') :
            data.document_name;
    };
    const order = [5, "desc"];

    let dataTable = DataTableTemplate.DataTableInitial(
        table,
        url,
        page,
        columns,
        columnsDefs,
        order,
        rowCallback
    );

    // 移除GlobalSearch Html輸入
    $("#recentlyPrintRecordDataTable_filter").parent().remove();
    dataTable.draw();
}

// 預覽資料之DepositRecordPreviewDataTable初始化
function RecentlyDepositRecordPreviewDataTableInitial() {
    const table = $("#recentlyDepositRecordDataTable");
    const url = "/User/User/RecentlyDepositRecordDataTableInitial";
    const page = $("#user_idDiv").text() // This page variable is temporary for passing the user_id
    const columns = [
        { data: "user_id", name: "儲值人員帳號" },
        { data: "user_name", name: "儲值人員姓名" },
        { data: "pbalance", name: "儲值前點數" },
        { data: "deposit_value", name: "儲值點數" },
        { data: "final_value", name: "儲值後點數" },
        { data: "deposit_date", name: "儲值時間" },
    ];

    const order = [5, "desc"];

    let dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, null, order, null);

    // 移除GlobalSearch Html輸入
    $("#recentlyDepositRecordDataTable_filter").parent().remove();
    dataTable.draw();
}


$(function () {
    UsedValue();
    PrintJobsDataTableInitial();
    PopupFormForEdit();
    PopupFormForRecentlyRecordPreview();
})