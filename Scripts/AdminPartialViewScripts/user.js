var datatable;
function SearchUserDataTableInitial() {
    datatable = $("#searchUserDataTable").DataTable({
        ajax: {
            url: "/Admin/User",
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
            { data: "e_mail", name: "信箱" }
        ],
        dom: "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { text: "輸出：", className: 'btn btn-secondary disabled' },
            { extend: "excel", className: "btn btn-warning buttons-excel buttons-html5" },
            { extend: "csv", className: "btn btn-warning buttons-csv buttons-html5" },
            { extend: "print", className: "btn btn-warning buttons-print buttons-html5" }
        ],
        order: [0, "desc"],
        paging: true,
        deferRender: true,
        serverSide: true,
        processing: true,
        pagingType: 'full_numbers',
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
            search: "全部欄位搜尋："
        }
    });
}

function ColumnSearch() {
    $("#searchUser_Account").keyup(function () {
        datatable.columns(0).search($("#searchUser_Account").val()).draw();

    });

    $("#searchUser_Password").keyup(function () {
        datatable.columns(1).search($("#searchUser_Password").val()).draw();

    });

    $("#searchUser_WorkNumber").keyup(function () {
        datatable.columns(2).search($("#searchUser_WorkNumber").val()).draw();

    });

    $("#searchUser_Name").keyup(function () {
        datatable.columns(3).search($("#searchUser_Name").val()).draw();

    });

    $("#searchUser_DepartmentID").keyup(function () {
        datatable.columns(4).search($("#searchUser_DepartmentID").val()).draw();
    });

    $("#searchUser_DepartmentName").keyup(function () {
        datatable.columns(5).search($("#searchUser_DepartmentName").val()).draw();
    });

    $("#searchUser_ColorPermissionSelect").change(function () {
        if ($("#searchUser_ColorPermissionSelect").val() != "0") {
            datatable.columns(6).search($("#searchUser_ColorPermissionSelect :selected").text()).draw();
        } else {
            datatable.columns(6).search("").draw();
        }
    });

    $("#searchUser_Mail").keyup(function () {
        datatable.columns(7).search($("#searchUser_Mail").val()).draw();
    });
}

function PopupForm() {
    $("#btnAddUser").on("click", function () {
        var url = $("#addUserForm").data("url");
        $.get(
            url,
            { formTitle: $(this).text() },
            function (data) {
                $("#addUserForm").html(data);
                $("#addUserForm").modal("show");
            }
        )
    });
}

function SubmitForm(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $("#addUserForm").modal("hide");
                    datatable.ajax.reload();
                }
            }
        });
    }
    return false;
}

function SearchDepartment() {
    $("#btnAddUser_SearchDepartment").on("click", function () {
        
    });
}

$(function () {
    SearchUserDataTableInitial();
    ColumnSearch();
    PopupForm();
    SearchDepartment();
});