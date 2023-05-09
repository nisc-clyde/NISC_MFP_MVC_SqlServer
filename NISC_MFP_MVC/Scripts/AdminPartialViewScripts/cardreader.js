import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete } from "./Shared.js"

var dataTable;
function SearchCardReaderDataTableInitial() {
    dataTable = $("#searchCardReaderDataTable").DataTable({
        ajax: {
            url: "/Admin/CardReader/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "cardreader" }
        },
        columns: [
            { data: "cr_id", name: "卡機編號" },
            { data: "cr_ip", name: "IP位置" },
            { data: "cr_port", name: "PORT" },
            { data: "cr_type", name: "卡機種類" },
            { data: "cr_mode", name: "運作模式" },
            { data: "cr_card_switch", name: "卡號判斷開關" },
            { data: "cr_status", name: "狀態" },
            {
                data: null,
                render: function (data, type, row) {
                    return "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-1 row-cols-xxl-3 g-2'>" +
                        "<div class='col' > <button type='button' class='btn btn-primary btn-sm btn-management' data-id='" + data.serial + "'><i class='fa-solid fa-circle-info me-1'></i><div style='display: inline-block; white-space: nowrap;'>管理</div></button></div > " +
                        "<div class='col'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" + data.serial + "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                        "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='" + data.serial + "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>";
                },
                orderable: false
            },
            { data: "serial", name: "serial" }
        ],
        columnDefs: [
            { width: "15%", targets: 7 },
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
            if (data.cr_mode == "連線") {
                $('td:eq(4)', row).html("<b class='text-success'>連線</b>");
            } else {
                $('td:eq(4)', row).html("<b class='text-danger'>離線</b>");
            }

            if (data.cr_card_switch == "開啟") {
                $('td:eq(5)', row).html("<b class='text-success'>開啟</b>");
            } else {
                $('td:eq(5)', row).html("<b class='text-danger'>關閉</b>");
            }

            if (data.cr_status == "線上") {
                $('td:eq(6)', row).html("<b class='text-success'>線上</b>");
            } else {
                $('td:eq(6)', row).html("<b class='text-danger'>離線</b>");
            }
        }
    });
}

function ColumnSearch() {
    $("#searchCardReader_CardreaderID").keyup(function () {
        dataTable.columns(0).search($("#searchCardReader_CardreaderID").val()).draw();

    });

    $("#searchCardReader_IPAddress").keyup(function () {
        dataTable.columns(1).search($("#searchCardReader_IPAddress").val()).draw();

    });

    $("#searchCardReader_Port").keyup(function () {
        dataTable.columns(2).search($("#searchCardReader_Port").val()).draw();

    });

    $("#searchCardReader_CardMachineTypeSelect").change(function () {
        if ($("#searchCardReader_CardMachineTypeSelect").val() != "") {
            dataTable.columns(3).search($("#searchCardReader_CardMachineTypeSelect :selected").text()).draw();
        } else {
            dataTable.columns(3).search("").draw();
        }
    });

    $("#searchCardReader_WorkModeSelect").change(function () {
        if ($("#searchCardReader_WorkModeSelect").val() != "") {
            dataTable.columns(4).search($("#searchCardReader_WorkModeSelect :selected").text()).draw();
        } else {
            dataTable.columns(4).search("").draw();
        }
    });

    $("#searchCardReader_CardOnOffSelect").change(function () {
        if ($("#searchCardReader_CardOnOffSelect").val() != "") {
            dataTable.columns(5).search($("#searchCardReader_CardOnOffSelect :selected").text()).draw();
        } else {
            dataTable.columns(5).search("").draw();
        }
    });

    $("#searchCardReader_CardStatusSelect").change(function () {
        if ($("#searchCardReader_CardStatusSelect").val() != "") {
            dataTable.columns(6).search($("#searchCardReader_CardStatusSelect :selected").text()).draw();
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
    const btnAdd = "btnAddCardReader";
    const modalForm = "cardReaderForm";
    const title = "事務機";
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
    const getURL = "/Admin/CardReader/DeleteCardReader";
    const postURL = "/Admin/CardReader/ReadyDeleteCardReader"
    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, getURL, postURL);
}

function CardReaderManagement() {
    dataTable.on("click", ".btn-management", function (e) {
        e.preventDefault();
        const getViewUrl = "/Admin/CardReader/CardReaderManagement";
        const modalForm = "cardReaderForm";
        let currentRow;
        if ($(this).parents("tr").prev().hasClass("dt-hasChild")) {
            //Is Responsiveness
            currentRow = $(this).parents("tr").prev();
        } else {
            //Not Responsiveness
            currentRow = $(this).closest("tr");
        }
        const rowData = dataTable.row(currentRow).data();

        const cardReaderSerial = $(this).data("id");
        const cardReaderId = rowData["cr_id"];
        const addOrEditUrl = "/Admin/CardReader/AddOrEditMFP";
        const deleteUrl = "/Admin/CardReader/DeleteMFP";

        $.get(
            getViewUrl,
            { formTitle: "事務機" + $(this).text(), serial: cardReaderSerial, cardReaderId: cardReaderId },
            function (data) {
                $("#" + modalForm).html(data);
                $("#" + modalForm).modal("show");

                CardReaderManagementAdd(addOrEditUrl, cardReaderId);
                CardReaderManagementEdit(addOrEditUrl, cardReaderId);
                CardReaderManagementDelete(deleteUrl);
                CardReaderManagementCancelSwitch();
                CardReaderManagementEditSwitch();
            }
        )
    });
}

function CardReaderManagementAdd(url, cardReaderId) {
    $("#cardreaderManagermentForm").on("submit", function () {
        if ($(".btnCardReaderManagement").attr("id") == "btnCardReaderManagementAdd") {
            $.validator.unobtrusive.parse(this);
            if ($(this).valid()) {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: $(this).serialize() + "&cr_id=" + cardReaderId + "&currentOperation=" + "Add",
                    success: function (data) {
                        if (data.success) {
                            $("#cardreaderManagermentForm").load("/Admin/CardReader/CardReaderManager", { cardReaderId: cardReaderId });
                        }
                    }
                });
            }
        }
        return false;
    })
}

function CardReaderManagementEdit(url, cardReaderId) {
    $("#cardreaderManagermentForm").on("submit", function (e) {
        e.preventDefault();
        if ($(".btnCardReaderManagement").attr("id") == "btnCardReaderManagementEdit") {
            let deleteRowDataSerialize = $(this).serialize();
            deleteRowDataSerialize = deleteRowDataSerialize.replace("multiFunctionPrintModel.", "");

            $.validator.unobtrusive.parse(this);
            if ($(this).valid()) {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: deleteRowDataSerialize + "&cr_id=" + cardReaderId + "&currentOperation=" + $("#btnCardReaderManagementEdit").val(),
                    success: function (data) {
                        if (data.success) {
                            $("#cardreaderManagermentForm").load("/Admin/CardReader/CardReaderManager", { cardReaderId: cardReaderId });
                        }
                    }
                });
            }
        }
        return false;
    })
}

function CardReaderManagementDelete(url) {
    $("body").on("click", "#cardreaderManagermentTable #btnCardReaderManagementRowDelete", function (e) {
        e.preventDefault();

        const sweetAlertHome = CustomSweetAlert2.SweetAlertTemplateHome();
        const sweetAlertSuccess = CustomSweetAlert2.SweetAlertTemplateSuccess();

        let deleteRow = $(this).closest("tr");
        var deleteRowDataSerialize = "serial=" + $(this).closest("tr").find("input").val();
        let $row = $(this).closest("tr"), $tds = $row.find("td");
        $.each($tds, function () {
            if ($(this).attr("id") != undefined) {
                deleteRowDataSerialize += "&" + $(this).attr("id") + "=" + $(this).text();
            }
        });
        deleteRowDataSerialize = deleteRowDataSerialize.replace(/線上/g, "Online");
        deleteRowDataSerialize = deleteRowDataSerialize.replace(/離線/g, "Offline");
        deleteRowDataSerialize = deleteRowDataSerialize.replace(/C(彩色)/g, "C");
        deleteRowDataSerialize = deleteRowDataSerialize.replace(/M(單色)/g, "M");
        deleteRowDataSerialize = deleteRowDataSerialize.replace(/未知/g, "");

        sweetAlertHome.fire()
            .then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: deleteRowDataSerialize,
                        success: function (data) {
                            if (data.success) {
                                deleteRow.remove();
                                sweetAlertSuccess.fire();
                            }
                        }
                    });
                }
            })
    });
}

function CardReaderManagementEditSwitch() {
    $("body").on("click", "#cardreaderManagermentTable #btnCardReaderManagementRowEdit", function () {
        let mfpSerial = $(this).parent().siblings("input").val();
        let rowData = [];
        let $row = $(this).closest("tr"), $tds = $row.find("td");
        $.each($tds, function () {
            rowData.push($(this).text());
        });

        $("#cardreaderManagermentTable tbody tr:first #serial").val(mfpSerial);
        $("#cardreaderManagermentTable tbody tr:first #printer_id").val(rowData[0]);
        $("#cardreaderManagermentTable tbody tr:first #mfp_ip").val(rowData[1]);
        $("#cardreaderManagermentTable tbody tr:first #mfp_name").val(rowData[2]);
        $("#cardreaderManagermentTable tbody tr:first #mfp_color").val(rowData[3].substring(0, 1));
        $("#cardreaderManagermentTable tbody tr:first #driver_number").val(rowData[4]);
        $("#cardreaderManagermentTable tbody tr:first #mfp_status").val((rowData[5] == "線上") ? "Online" : "Offline");

        $("#cardreaderManagermentTable #btnCardReaderManagementAdd").text("修改");
        $("#cardreaderManagermentTable #btnCardReaderManagementAdd").val("Edit");
        $("#cardreaderManagermentTable #btnCardReaderManagementAdd").attr("id", "btnCardReaderManagementEdit")
        $("#cardreaderManagermentTable #btnCardReaderManagementCancel").show();
    })
}

function CardReaderManagementCancelSwitch() {
    $("body").on("click", "#cardreaderManagermentTable #btnCardReaderManagementCancel", function () {
        $("#cardreaderManagermentTable tbody tr:first #serial").val("");
        $("#cardreaderManagermentTable tbody tr:first #printer_id").val("");
        $("#cardreaderManagermentTable tbody tr:first #mfp_ip").val("");
        $("#cardreaderManagermentTable tbody tr:first #mfp_name").val("");
        $("#cardreaderManagermentTable tbody tr:first #mfp_color").val("C");
        $("#cardreaderManagermentTable tbody tr:first #driver_number").val("");
        $("#cardreaderManagermentTable tbody tr:first #mfp_status").val("Online");

        $("#cardreaderManagermentTable #btnCardReaderManagementUpdate").text("新增");
        $("#cardreaderManagermentTable #btnCardReaderManagementUpdate").val("Add");
        $("#cardreaderManagermentTable #btnCardReaderManagementUpdate").attr("id", "btnCardReaderManagementAdd");
        $("#cardreaderManagermentTable #btnCardReaderManagementCancel").hide();
    });
}

$(function () {
    SearchCardReaderDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
    CardReaderManagement();
});