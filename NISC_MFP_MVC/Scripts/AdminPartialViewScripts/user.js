import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete } from "./Shared.js"

var dataTable;
function SearchUserDataTableInitial() {
    dataTable = $("#searchUserDataTable").DataTable({
        ajax: {
            url: "/Admin/User/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "user" }
        },
        columns: [
            { data: "user_id", name: "帳號" },
            { data: "user_password", name: "密碼" },
            { data: "work_id", name: "工號" },
            { data: "user_name", name: "姓名" },
            { data: "dept_id", name: "部門代碼" },
            { data: "dept_name", name: "部門名稱" },
            { data: "color_enable_flag", name: "彩色使用權限" },
            { data: "e_mail", name: "信箱" },
            {
                data: null,
                render: function (data, type, row) {
                    return "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-1 row-cols-xxl-3  g-2'><div class='col'><button type='button' class='btn btn-primary btn-sm btn-permission' data-id='" + data.serial + "'><i class='fa-solid fa-circle-info me-1'></i><div style='display: inline-block; white-space: nowrap;'>權限</div></button></div>" +
                        "<div class='col'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" + data.serial + "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                        "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-sm btn-delete' data-id='" + data.serial + "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>";
                },
                orderable: false
            },
            { data: "serial", name: "serial" }
        ],
        columnDefs: [
            { width: "15%", targets: 8 },
            { visible: false, target: 9 }
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
            if (data.color_enable_flag == "有") {
                $('td:eq(6)', row).html("<b class='text-success'>有</b>");
            } else {
                $('td:eq(6)', row).html("<b class='text-danger'>無</b>");
            }

        }
    });
}

function ColumnSearch() {
    $("#searchUser_Account").keyup(function () {
        dataTable.columns(0).search($("#searchUser_Account").val()).draw();

    });

    $("#searchUser_Password").keyup(function () {
        dataTable.columns(1).search($("#searchUser_Password").val()).draw();

    });

    $("#searchUser_WorkNumber").keyup(function () {
        dataTable.columns(2).search($("#searchUser_WorkNumber").val()).draw();

    });

    $("#searchUser_Name").keyup(function () {
        dataTable.columns(3).search($("#searchUser_Name").val()).draw();

    });

    $("#searchUser_DepartmentID").keyup(function () {
        dataTable.columns(4).search($("#searchUser_DepartmentID").val()).draw();
    });

    $("#searchUser_DepartmentName").keyup(function () {
        dataTable.columns(5).search($("#searchUser_DepartmentName").val()).draw();
    });

    $("#searchUser_ColorPermissionSelect").change(function () {
        if ($("#searchUser_ColorPermissionSelect").val() != "") {
            dataTable.columns(6).search($("#searchUser_ColorPermissionSelect :selected").text()).draw();
        } else {
            dataTable.columns(6).search("").draw();
        }
    });

    $("#searchUser_Mail").keyup(function () {
        dataTable.columns(7).search($("#searchUser_Mail").val()).draw();
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
    const btnAdd = "btnAddUser";
    const modalForm = "userForm";
    const title = "使用者";
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
    const getURL = "/Admin/User/DeleteUser";
    const postURL = "/Admin/User/ReadyDeleteUser"
    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, getURL, postURL);
}

function EditUserPermissionConfig() {
    const url = "/Admin/User/UserPermissionConfig";
    const modalForm = "userForm";


    dataTable.on("click", ".btn-permission", function (e) {
        e.preventDefault();
        const currentRow = $(this).closest("tr");
        const rowData = dataTable.row(currentRow).data();

        $.get(
            url,
            { formTitle: "使用者權限設定", serial: $(this).data("id") },
            function (data) {
                $("#" + modalForm).html(data);
                $("#" + modalForm).modal("show");

                //$("#addOrEditForm").on("submit", function () {
                //    PostAddOrEditTemplate(this, modalForm, dataTable);
                //    return false;
                //})
            }
        )
    });
}

$(function () {
    SearchUserDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
    EditUserPermissionConfig();
});