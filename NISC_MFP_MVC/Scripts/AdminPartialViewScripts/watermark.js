import {
    CustomSweetAlert2,
    RequestAddOrEdit,
    RequestDelete,
    DataTableTemplate,
} from "./Shared.js";

/**
 * Visualization the data from Database
 * @param {element} table - The table which you want populate.
 * @param {string} url - The table resource request from.
 * @param {string} page - Identify columns index when  request to controller.
 * @param {string[]} columns - Declare all columns will exists.
 * @param {string[]} columnDefs - Custom column definition your self.
 * @param {string[]} order - Specify default orderable by column when data table already.
 * @param {function} rowCallback - Do things between response from backend and render to element,
 */
var dataTable;
function SearchWatermarkDataTableInitial() {
    const table = $("#searchWatermarkDataTable");
    const url = "/Admin/Watermark/InitialDataTable";
    const page = "watermark";
    const columns = [
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
                return (
                    "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-2 row-cols-xxl-2 g-2'><div class='col'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" +
                    data.id +
                    "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                    "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='" +
                    data.id +
                    "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>"
                );
            },
            orderable: false,
        },
        { data: "id", name: "id" },
    ];
    const columnDefs = [
        { width: "10%", targets: 6 },
        { width: "10%", targets: 11 },
        { visible: false, target: 12 },
    ];
    const order = [0, "desc"];
    const rowCallback = function (row, data) {
        data.rotation != null
            ? $("td:eq(9)", row).html(data.rotation + "°")
            : $("td:eq(9)", row).html(data.rotation);
    };

    dataTable = DataTableTemplate.DataTableInitial(
        table,
        url,
        page,
        columns,
        columnDefs,
        order,
        rowCallback
    );
    dataTable.draw();

}

/**
 * 輸入欲搜尋之欄位資料並Refresh DataTable
 */
function ColumnSearch() {
    $("#searchWatermark_TypeSelect").change(function () {
        if ($("#searchWatermark_TypeSelect").val() != "") {
            dataTable
                .columns(0)
                .search($("#searchWatermark_TypeSelect :selected").text())
                .draw();
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
            dataTable
                .columns(5)
                .search($("#searchWatermark_PositionSelect :selected").text())
                .draw();
        } else {
            dataTable.columns(5).search("").draw();
        }
    });

    $("#searchWatermark_FillTypeSelect").change(function () {
        if ($("#searchWatermark_FillTypeSelect").val() != "") {
            dataTable
                .columns(6)
                .search($("#searchWatermark_FillTypeSelect :selected").text())
                .draw();
        } else {
            dataTable.columns(6).search("").draw();
        }
    });

    $("#searchWatermark_Text").keyup(function () {
        dataTable.columns(7).search($("#searchWatermark_Text").val()).draw();
    });

    $("#searchWatermark_ImageLocation").keyup(function () {
        dataTable
            .columns(8)
            .search($("#searchWatermark_ImageLocation").val())
            .draw();
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
    const url = "/Admin/Watermark/Delete";

    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, url);
}

$(function () {
    SearchWatermarkDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
});
