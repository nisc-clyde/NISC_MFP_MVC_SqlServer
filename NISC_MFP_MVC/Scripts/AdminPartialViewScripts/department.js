import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete } from "./Shared.js"

var dataTable;
function SearchDepartmentDataTableInitial() {
    dataTable = $("#searchDepartmentDataTable").DataTable({
        ajax: {
            url: "/Admin/Department/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "department" }
        }, columns: [
            { data: "dept_id", name: "部門編號" },
            { data: "dept_name", name: "部門名稱" },
            { data: "dept_value", name: "可用點數上限" },
            { data: "dept_month_sum", name: "可用遞增餘額" },
            { data: "dept_usable", name: "狀態" },
            { data: "dept_email", name: "部門管理者Email" },
            {
                data: null,
                render: function (data, type, row) {
                    return "<div class='row g-2'><div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-6'><button type='button' class='btn btn-info btn-sm btn-edit' data-id='" + data.serial + "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-12 col-xl-6'><button type='button' class='btn btn-danger btn-sm btn-sm btn-delete' data-id='" + data.serial + "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>";
                },
                orderable: false
            },
            { data: "serial", name: "serial" }
        ],
        columnDefs: [
            { "width": "150px", target: 6 },
            { visible: false, target: 7 }
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
            if (data.dept_usable == "停用") {
                $('td:eq(4)', row).html("<b class='text-danger'>停用</b>");
            } else {
                $('td:eq(4)', row).html("<b class='text-success'>啟用</b>");
            }

        }
    });
}

function ColumnSearch() {
    $("#searchDepartment_DepartmentID").keyup(function () {
        dataTable.columns(0).search($("#searchDepartment_DepartmentID").val()).draw();

    });

    $("#searchDepartment_DepartmentName").keyup(function () {
        dataTable.columns(1).search($("#searchDepartment_DepartmentName").val()).draw();

    });

    $("#searchDepartment_AvailablePointLimit").keyup(function () {
        dataTable.columns(2).search($("#searchDepartment_AvailablePointLimit").val()).draw();

    });

    $("#searchDepartment_AvailableBalance").keyup(function () {
        dataTable.columns(3).search($("#searchDepartment_AvailableBalance").val()).draw();

    });

    $("#searchDepartment_StatueSelect").change(function () {
        if ($("#searchDepartment_StatueSelect").val() != "") {
            dataTable.columns(4).search($("#searchDepartment_StatueSelect :selected").text()).draw();
        } else {
            dataTable.columns(4).search("").draw();
        }
    });

    $("#searchDepartment_Mail").keyup(function () {
        dataTable.columns(5).search($("#searchDepartment_Mail").val()).draw();
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
    const btnAdd = "btnAddDepartment";
    const modalForm = "departmentForm";
    const title = "部門";
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
    const getURL = "/Admin/Department/DeleteDepartment";
    const postURL = "/Admin/Department/ReadyDeleteDepartment"
    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, getURL, postURL);
}

$(function () {
    SearchDepartmentDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
});