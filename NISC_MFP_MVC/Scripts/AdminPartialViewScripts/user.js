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
        { data: "color_enable_flag", name: "彩色使用權限", width: "10%" },
        { data: "e_mail", name: "信箱" },
        { data: "operation", name: "操作", orderable: false, width: "15%" },
        { data: "serial", name: "serial", visible: false },
    ];
    const order = [0, "desc"];
    dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, null, order, null).draw();
}

/**
 * 輸入欲搜尋之欄位資料並Refresh DataTable
 */
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

/**
 * 載入編輯權限之Partial View
 */
function EditUserPermissionConfig() {
    const url = "/Admin/User/UserPermissionConfig";
    const modalForm = "userForm";

    dataTable.on("click", ".btn-permission", function (e) {
        e.preventDefault();
        const currentRow = $(this).closest("tr");
        const rowData = dataTable.row(currentRow).data();
        const user_id = rowData["user_id"];

        $.get(url, { formTitle: "使用者權限設定", user_id: user_id }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");
            PermissionCheckedAll();
            PermissionCheckedClearAll();
            PermissionBinding_PrintWithView();
            PermissionBinding_UserWithManagePermission();

            /**
             * 提交權限更新
             */
            $("#permissionForm").on("submit", function () {
                /**
                 * 所有勾選之權限加入至Array
                 */
                let authorities = [];
                $(".container input[type=checkbox]:checked").each(function () {
                    authorities.push($(this).val());
                });

                $.ajax({
                    url: url,
                    //權限Array轉換成字串以「,」逗號分隔
                    data: { authority: authorities.join(","), user_id: user_id },
                    type: "POST",
                    success: function (data) {
                        if (data.success) {
                            CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire();
                        } else {
                            CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                        }
                        $("#" + modalForm).modal("hide");
                    },
                });
                return false;
            });
        });
    });
}

/**
 * 控制DOM，選取所有權限
 */
function PermissionCheckedAll() {
    $("#btnChekcedAll").on("click", function () {
        $(".container input[type=checkbox]").each(function () {
            $(this).attr("checked", true);
        });
    });
}

/**
 * 控DOM，取消選取所有權限
 */
function PermissionCheckedClearAll() {
    $("#btnChekcedClearAll").on("click", function () {
        $(".container input[type=checkbox]").each(function () {
            $(this).attr("checked", false);
        });
    });
}

function PermissionBinding_PrintWithView() {
    if ($("#checkBoxPermissionView").prop("checked") == true) {
        $("#checkBoxPermissionPrint").attr("disabled", true);
        $("#chainOfPrintWithView").removeClass("fa-link-slash").addClass("fa-link");
    }

    $("#checkBoxPermissionView").on("click", function () {
        if ($(this).prop("checked") == true) {
            $("#checkBoxPermissionPrint").prop("checked", true);
            $("#checkBoxPermissionPrint").attr("disabled", true);
            $("#chainOfPrintWithView").removeClass("fa-link-slash").addClass("fa-link");
        } else {
            $("#checkBoxPermissionPrint").attr("disabled", false);
            $("#chainOfPrintWithView").removeClass("fa-link").addClass("fa-link-slash");
        }
    });
}

function PermissionBinding_UserWithManagePermission() {
    if ($("#checkBoxPermissionManagePermission").prop("checked") == true) {
        $("#checkBoxPermissionUser").attr("disabled", true);
        $("#chainOfUserWithManagePermission").removeClass("fa-link-slash").addClass("fa-link");
    }

    $("#checkBoxPermissionManagePermission").on("click", function () {
        if ($(this).prop("checked") == true) {
            $("#checkBoxPermissionUser").prop("checked", true);
            $("#checkBoxPermissionUser").attr("disabled", true);
            $("#chainOfUserWithManagePermission").removeClass("fa-link-slash").addClass("fa-link");
        } else {
            $("#checkBoxPermissionUser").attr("disabled", false);
            $("#chainOfUserWithManagePermission").removeClass("fa-link").addClass("fa-link-slash");
        }
    });
}

$(function () {
    SearchUserDataTableInitial();
    ColumnSearch();
    PopupFormForAddOrEdit();
    DeleteAlertPopUp();
    EditUserPermissionConfig();
});
