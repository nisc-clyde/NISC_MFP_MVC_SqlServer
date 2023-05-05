import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete } from "./Shared.js"

var dataTable;
function SearchCardDataTableInitial() {
    dataTable = $("#searchCardDataTable").DataTable({
        ajax: {
            url: "/Admin/Card/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "card" }
        },
        columns: [
            { data: "card_id", name: "卡片編號" },
            { data: "value", name: "點數" },
            { data: "freevalue", name: "免費點數" },
            { data: "user_id", name: "使用者帳號" },
            { data: "user_name", name: "使用者姓名" },
            { data: "card_type", name: "屬性" },
            { data: "enable", name: "使用狀態" },
            {
                data: null,
                render: function (data, type, row) {
                    return "<div class='row g-2'><div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-4'><button type='button' class='btn btn-primary btn-sm btn-deposit' data-id='" + data.serial + "'><i class='fa-solid fa-circle-info me-1'></i><div style='display: inline-block; white-space: nowrap;'>儲值</div></button></div>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-4'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" + data.serial + "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-4'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='" + data.serial + "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>";
                },
                orderable: false
            },
            { data: "serial", name: "serial" }
        ],
        columnDefs: [
            { width: "220px", targets: 7 },
            { visible: false, target: 8 }
        ],
        dom: "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { text: "輸出：", className: 'btn btn-secondary disabled' },
            { extend: "excel", className: "btn btn-warning buttons-excel buttons-html5" },
            { extend: "csv", bom: true, className: "btn btn-warning buttons-csv buttons-html5" },
            { extend: "print", className: "btn btn-warning buttons-print buttons-html5" }
        ],
        order: [0, "desc"],
        paging: true,
        pagingType: 'full_numbers',
        deferRender: true,
        serverSide: true,
        processing: true,
        responsive: true,
        language: {
            processing: "資料載入中...請稍後",
            paginate: {
                first: "首頁",
                last: "尾頁",
                previous: "上一頁",
                next: "下一頁"
            },
            info: "顯示第 _START_ 至 _END_ 筆資料，共 _TOTAL_ 筆",
            zeroRecords: "找不到相符資料",
            search: "全部欄位搜尋：",
            infoFiltered: ""
        },
        rowCallback: function (row, data) {
            if (data.card_type == "遞增") {
                $('td:eq(5)', row).html("<b class='text-success'>遞增</b>");
            } else {
                $('td:eq(5)', row).html("<b class='text-danger'>遞減</b>");
            }

            if (data.enable == "可用") {
                $('td:eq(6)', row).html("<b class='text-success'>可用</b>");
            } else {
                $('td:eq(6)', row).html("<b class='text-danger'>停用</b>");
            }
        }
    });
}

function ColumnSearch() {
    $("#searchCard_CardID").keyup(function () {
        dataTable.columns(0).search($("#searchCard_CardID").val()).draw();

    });

    $("#searchCard_Point").keyup(function () {
        dataTable.columns(1).search($("#searchCard_Point").val()).draw();

    });

    $("#searchCard_FreePort").keyup(function () {
        dataTable.columns(2).search($("#searchCard_FreePort").val()).draw();

    });

    $("#searchCard_Account").keyup(function () {
        dataTable.columns(3).search($("#searchCard_Account").val()).draw();

    });

    $("#searchCard_Name").keyup(function () {
        dataTable.columns(4).search($("#searchCard_Name").val()).draw();

    });

    $("#searchCard_AttributeSelect").change(function () {
        if ($("#searchCard_AttributeSelect").val() != "") {
            dataTable.columns(5).search($("#searchCard_AttributeSelect :selected").text()).draw();
        } else {
            dataTable.columns(5).search("").draw();
        }
    });

    $("#searchCard_StatusSelect").change(function () {
        if ($("#searchCard_StatusSelect").val() != "") {
            dataTable.columns(6).search($("#searchCard_StatusSelect :selected").text()).draw();
        } else {
            dataTable.columns(6).search("").draw();
        }
    });
}

/**
 * Popup the modal from bootstrap library for adding or updating the data
 * @param {string} btnAdd - click which button topopup modal for adding.
 * @param {string} modalForm - modal's id.
 * @param {jQuery DataTable} dataTable - the data of container.
 * @param {string} uniqueIdProperty - identification each row data of property.
 */
function PopupFormForAddOrEdit() {
    const btnAdd = "btnAddCard";
    const modalForm = "cardForm";
    const title = "卡片";
    RequestAddOrEdit.GetAddOrEditTemplate(btnAdd, modalForm, dataTable, title);
}

/**
 * Popup the modal from bootstrap library for deleting the data
 * @param {jQuery DataTable} dataTable - The data of container.
 * @param {string} uniqueIdProperty - Identification each row data of property.
 * @param {string} getURL - Send request of get destination
 * @param {string} postURL - Send request of post destination
 */
function DeleteAlertPopUp() {
    const uniqueIdProperty = "serial";
    const getURL = "/Admin/Card/DeleteCard";
    const postURL = "/Admin/Card/ReadyDeleteCard"
    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, getURL, postURL);
}

function ResetFreePoint() {
    $("#btnResetCardFreePoint").on("click", function () {
        const url = "/Admin/Card/ResetCardFreePoint";
        const modalForm = "cardForm";
        const sweetAlertSuccess = CustomSweetAlert2.SweetAlertTemplateSuccess();

        $.get(
            url,
            { formTitle: $(this).text() },
            function (data) {
                $("#" + modalForm).html(data);
                $("#" + modalForm).modal("show");
                $("#resetFreeValueForm").on("submit", function () {
                    $.validator.unobtrusive.parse(this);
                    if ($(this).valid()) {
                        $.ajax({
                            type: "POST",
                            url: url,
                            data: $(this).serialize(),
                            success: function (data) {
                                if (data.success) {
                                    $("#" + modalForm).modal("hide");
                                    sweetAlertSuccess.fire({
                                        text: "免費點數已重設"
                                    })
                                        .then((result) => {
                                            if (result.isConfirmed || result.dismiss == Swal.DismissReason.timer) {
                                                dataTable.ajax.reload();
                                            }
                                        })
                                }
                            }
                        });
                    }
                    return false;
                })
            }
        )
    });
}

var addedValue = 0;
var originalValue
function DepositCardValue() {
    const url = "/Admin/Card/DepositCard";
    const modalForm = "cardForm";
    const sweetAlertHome = CustomSweetAlert2.SweetAlertTemplateHome();
    const sweetAlertSuccess = CustomSweetAlert2.SweetAlertTemplateSuccess();

    dataTable.on("click", ".btn-deposit", function (e) {
        e.preventDefault();
        const currentRow = $(this).closest("tr");
        const serial = $(this).data("id");

        $.get(
            url,
            { formTitle: "卡片儲值", serial: serial },
            function (data) {
                $("#" + modalForm).html(data);
                $("#" + modalForm).modal("show");
                addedValue = 0;
                CustomDeposit();

                $("#depositcardForm").on("submit", function () {
                    if (addedValue != 0) {
                        sweetAlertHome.fire({
                            title: "確定儲值" + addedValue + "點嗎？",
                            text: ""
                        }).then((result) => {
                            if (result.isConfirmed) {
                                $.ajax({
                                    type: "POST",
                                    url: "/Admin/Card/DepositCard",
                                    data: { value: addedValue, serial: serial },
                                    success: function (data) {
                                        $("#" + modalForm).modal("hide");
                                        if (data.success) {
                                            sweetAlertSuccess.fire({
                                                text: "儲值成功"
                                            }).then((result) => {
                                                dataTable.ajax.reload();
                                                //if (result.isConfirmed || result.dismiss == Swal.DismissReason.timer) {
                                                //    currentRow.addClass("animate__animated animate__flash animate__fast animate__repeat-2");
                                                //    currentRow.on("animationend", function () {
                                                //        currentRow.removeClass("animate__animated animate__flash animate__fast animate__repeat-2");
                                                //    })
                                                //}
                                            })
                                        }
                                    }
                                })
                            }
                        })
                    }
                    return false;
                })
            }
        )
    });
}

function CustomDeposit() {
    originalValue = $("tr[id=depositCardRowData] th[id=value]").text();
    $("#btnCustomDeposit").on("click", function () {
        addedValue += parseInt($("#customDeposit").val());
        if (addedValue > 0) {
            $("tr[id=depositCardRowData] th[id=value]").html(originalValue + " <b class='text-success'>(+" + parseInt(addedValue) + ")</b>");
        } else if (addedValue < 0) {
            $("tr[id=depositCardRowData] th[id=value]").html(originalValue + " <b class='text-danger'>(" + parseInt(addedValue) + ")</b>");
        } else {
            $("tr[id=depositCardRowData] th[id=value]").text(originalValue);
        }
    })

    $("#templateDepositParent .templateDeposit").each(function (index) {
        $(this).on("click", function () {
            if ($(this).text() == "儲值") {
                addedValue = addedValue + parseInt($(this).prev().val());
            } else if ($(this).text() == "扣款") {
                addedValue = addedValue - parseInt($(this).next().val());
            }
            if (addedValue > 0) {
                $("tr[id=depositCardRowData] th[id=value]").html(originalValue + " <b class='text-success'>(+" + parseInt(addedValue) + ")</b>");
            } else if (addedValue < 0) {
                $("tr[id=depositCardRowData] th[id=value]").html(originalValue + " <b class='text-danger'>(" + parseInt(addedValue) + ")</b>");
            } else {
                $("tr[id=depositCardRowData] th[id=value]").text(originalValue);
            }
        })
    });
}


$(function () {
    SearchCardDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
    ResetFreePoint();
    DepositCardValue();
});