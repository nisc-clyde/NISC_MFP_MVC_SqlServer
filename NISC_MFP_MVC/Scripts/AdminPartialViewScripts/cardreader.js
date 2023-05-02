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
                    return "<div class='row gx-0'><div class='col-4 '><button type='button' class='btn btn-primary btn-sm btn-management' data-id='" + data.serial + "'><i class='fa-solid fa-circle-info me-1'></i>管理</button></div>" +
                        "<div class='col-4'><button type='button' class='btn btn-info btn-sm  btn-edit'data-id='" + data.serial + "'><i class='fa-solid fa-pen-to-square me-1'></i>修改</button></div>" +
                        "<div class='col-4'><button type='button' class='btn btn-danger btn-sm btn-sm btn-delete'data-id='" + data.serial + "'><i class='fa-solid fa-trash me-1'></i>刪除</button></div></div>";
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

$(function () {
    SearchCardReaderDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
});