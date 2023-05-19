import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete, DataTableTemplate } from "./Shared.js";

/**
 * Visualization the data from Database
 * @param {Element} table - The table which you want populate.
 * @param {string} url - The table resource request from.
 * @param {string} page - Identify columns index when  request to controller.
 * @param {string[]} columns - Declare all columns will exists.
 * @param {string[]} columnDefs - Custom column definition your self.
 * @param {string[]} order - Specify default orderable by column when data table already.
 * @param {function} rowCallback - Do things between response from backend and render to element,
 */
var dataTable;
function SearchUserDataTableInitial() {
    const table = $("#searchUserDataTable");
    const url = "/Admin/User/InitialDataTable";
    const page = "user";
    const columns = [
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
                return (
                    "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-1 row-cols-xxl-3  g-2'><div class='col'><button type='button' class='btn btn-primary btn-sm btn-permission' data-id='" +
                    data.serial +
                    "'><i class='fa-solid fa-circle-info me-1'></i><div style='display: inline-block; white-space: nowrap;'>權限</div></button></div>" +
                    "<div class='col'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" +
                    data.serial +
                    "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                    "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-sm btn-delete' data-id='" +
                    data.serial +
                    "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>"
                );
            },
            orderable: false,
        },
        { data: "serial", name: "serial" },
    ];
    const columnDefs = [
        { width: "15%", targets: 8 },
        { visible: false, target: 9 },
    ];
    const order = [0, "desc"];
    const rowCallback = function (row, data) {
        data.color_enable_flag == "有"
            ? $("td:eq(6)", row).html("<b class='text-success'>有</b>")
            : $("td:eq(6)", row).html("<b class='text-danger'>無</b>");
    };

    dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, columnDefs, order, rowCallback);
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
    const url = "/Admin/User/Delete";
    RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, url);
}

function EditUserPermissionConfig() {
    const url = "/Admin/User/UserPermissionConfig";
    const modalForm = "userForm";

    dataTable.on("click", ".btn-permission", function (e) {
        e.preventDefault();
        const currentRow = $(this).closest("tr");
        const rowData = dataTable.row(currentRow).data();
        const serial = $(this).data("id");

        $.get(url, { formTitle: "使用者權限設定", serial: serial }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");
            PermissionCheckedAll();
            PermissionCheckedClearAll();

            $("#permissionForm").on("submit", function () {
                let authoritys = [];
                $(".container input[type=checkbox]:checked").each(function () {
                    authoritys.push($(this).val());
                });

                $.ajax({
                    url: url,
                    data: { authority: authoritys.join(","), serial: serial },
                    type: "POST",
                    success: function (data) {
                        if (data.success) {
                            CustomSweetAlert2.SweetAlertTemplateSuccess().fire({
                                text: "權限已修改",
                            });
                        }
                        $("#" + modalForm).modal("hide");
                    },
                });
                return false;
            });
        });
    });
}

function PermissionCheckedAll() {
    $("#btnChekcedAll").on("click", function () {
        $(".container input[type=checkbox]").each(function () {
            $(this).attr("checked", true);
        });
    });
}

function PermissionCheckedClearAll() {
    $("#btnChekcedClearAll").on("click", function () {
        $(".container input[type=checkbox]").each(function () {
            $(this).attr("checked", false);
        });
    });
}

$(function () {
    SearchUserDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
    EditUserPermissionConfig();
});
