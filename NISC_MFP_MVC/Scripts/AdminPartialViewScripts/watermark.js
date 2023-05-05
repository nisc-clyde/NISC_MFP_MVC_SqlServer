import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete } from "./Shared.js"

var dataTable;
function SearchWatermarkDataTableInitial() {
    dataTable = $("#searchWatermarkDataTable").DataTable({
        ajax: {
            url: "/Admin/Watermark/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "watermark" }
        },
        columns: [
            { data: "type", name: "類別" },
            { data: "left_offset", name: "左邊偏移" },
            { data: "right_offset", name: "右邊偏移" },
            { data: "top_offset", name: "上邊偏移" },
            { data: "bottom_offset", name: "下邊偏移" },
            { data: "position_mode", name: "浮水印位置" },
            { data: "fill_mode", name: "填滿方式" },
            { data: "text", name: "文字" },
            { data: "image_path", name: "圖片位置" },
            { data: "rotation", name: "旋轉角度" },
            { data: "color", name: "顏色" },
            {
                data: null,
                render: function (data, type, row) {
                    return "<div class='row g-2'><div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-6'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" + data.id + "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-6'><button type='button' class='btn btn-danger btn-sm btn-sm btn-delete' data-id='" + data.id + "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>";
                },
                orderable: false
            },
            { data: "id", name: "id" }
        ],
        columnDefs: [
            { width: "150px", targets: 11 },
            { visible: false, target: 12 }
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
            if (data.rotation != null) {
                $('td:eq(9)', row).html(data.rotation + "°");
            }
        }
    });
}

function ColumnSearch() {
    $("#searchWatermark_TypeSelect").change(function () {
        if ($("#searchWatermark_TypeSelect").val() != "") {
            dataTable.columns(0).search($("#searchWatermark_TypeSelect :selected").text()).draw();
        } else {
            dataTable.columns(0).search("").draw();
        }
    });

    $("#searchWatermark_LeftBias").keyup(function () {
        dataTable.columns(1).search($("#searchWatermark_LeftBias").val()).draw();

    });

    $("#searchWatermark_RightBias").keyup(function () {
        dataTable.columns(2).search($("#searchWatermark_RightBias").val()).draw();

    });

    $("#searchWatermark_TopBias").keyup(function () {
        dataTable.columns(3).search($("#searchWatermark_TopBias").val()).draw();

    });

    $("#searchWatermark_BottomBias").keyup(function () {
        dataTable.columns(4).search($("#searchWatermark_BottomBias").val()).draw();

    });

    $("#searchWatermark_PositionSelect").change(function () {
        if ($("#searchWatermark_PositionSelect").val() != "") {
            dataTable.columns(5).search($("#searchWatermark_PositionSelect :selected").text()).draw();
        } else {
            dataTable.columns(5).search("").draw();
        }
    });

    $("#searchWatermark_FillTypeSelect").change(function () {
        if ($("#searchWatermark_FillTypeSelect").val() != "") {
            dataTable.columns(6).search($("#searchWatermark_FillTypeSelect :selected").text()).draw();
        } else {
            dataTable.columns(6).search("").draw();
        }
    });

    $("#searchWatermark_Text").keyup(function () {
        dataTable.columns(7).search($("#searchWatermark_Text").val()).draw();

    });

    $("#searchWatermark_ImageLocation").keyup(function () {
        dataTable.columns(8).search($("#searchWatermark_ImageLocation").val()).draw();

    });

    $("#searchWatermark_Rotate").keyup(function () {
        dataTable.columns(9).search($("#searchWatermark_Rotate").val()).draw();

    });

    $("#searchWatermark_Color").keyup(function () {
        dataTable.columns(10).search($("#searchWatermark_Color").val()).draw();

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
    const btnAdd = "btnAddWatermark";
    const modalForm = "watermarkForm";
    const title = "浮水印";
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
    const uniqueIdProperty = "id";
    const getURL = "/Admin/Watermark/DeleteWatermark";
    const postURL = "/Admin/Watermark/ReadyDeleteWatermark"
    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, getURL, postURL);
}


$(function () {
    SearchWatermarkDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
});