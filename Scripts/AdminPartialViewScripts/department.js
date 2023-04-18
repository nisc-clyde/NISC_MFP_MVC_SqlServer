var datatable;
function SearchDepartmentDataTableInitial() {
    datatable = $("#searchDepartmentDataTable").DataTable({
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
                defaultContent: "<button type='button' class='btn btn-info btn-sm me-1 btn-edit'><i class='fa-solid fa-pen-to-square me-1'></i>修改</button>" +
                    "<button type='button' class='btn btn-danger btn-sm btn-delete'><i class='fa-solid fa-trash me-1'></i>刪除</button>",
                orderable: false
            },
            { data: "serial", name: "序列號" }
        ],
        columnDefs: [
            { "width": "150px", target: 6 },
            { visible: false, target: 7 }
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
            search: "全部欄位搜尋："
        }
    });
}

function ColumnSearch() {
    $("#searchDepartment_DepartmentID").keyup(function () {
        datatable.columns(0).search($("#searchDepartment_DepartmentID").val()).draw();

    });

    $("#searchDepartment_DepartmentName").keyup(function () {
        datatable.columns(1).search($("#searchDepartment_DepartmentName").val()).draw();

    });

    $("#searchDepartment_AvailablePointLimit").keyup(function () {
        datatable.columns(2).search($("#searchDepartment_AvailablePointLimit").val()).draw();

    });

    $("#searchDepartment_AvailableBalance").keyup(function () {
        datatable.columns(3).search($("#searchDepartment_AvailableBalance").val()).draw();

    });

    $("#searchDepartment_StatueSelect").change(function () {
        if ($("#searchDepartment_StatueSelect").val() != "0") {
            datatable.columns(4).search($("#searchDepartment_StatueSelect :selected").text()).draw();
        } else {
            datatable.columns(4).search("").draw();
        }
    });

    $("#searchDepartment_Mail").keyup(function () {
        datatable.columns(5).search($("#searchDepartment_Mail").val()).draw();
    });
}

function PopupFormForAdd() {
    $("#btnAddDepartment").on("click", function () {
        $.get("/Admin/Department/AddDepartment", { formTitle: $(this).text() },
            function (data) {
                $("#departmentForm").html(data);
                $("#departmentForm").modal("show");
            })
    });
}

function SubmitFormForAdd(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $("#departmentForm").modal("hide");
                    datatable.ajax.reload();
                }
            }
        });
    }
    return false;
}

function PopupFormForUpdate() {
    datatable.on("click", ".btn-edit", function (e) {
        e.preventDefault();

        var currentRow = $(this).closest("tr");
        var rowData = datatable.row(currentRow).data();

        $.get("/Admin/Department/UpdateDepartment", { formTitle: "修改部門", serial: rowData["serial"] },
            function (data) {
                $("#departmentForm").html(data);
                $("#departmentForm").modal("show");
            })
    });
}

function SubmitFormForUpdate(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: "/Admin/Department/UpdateDepartment",
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $("#departmentForm").modal("hide");
                    datatable.ajax.reload();
                }
            }
        });
    }
    return false;
}

$(function () {
    SearchDepartmentDataTableInitial();
    ColumnSearch();
    PopupFormForAdd();
    PopupFormForUpdate();
});