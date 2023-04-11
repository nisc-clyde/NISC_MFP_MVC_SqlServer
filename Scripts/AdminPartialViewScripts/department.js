var datatable;
function SearchDepartmentDataTableInitial() {
    datatable = $("#searchDepartmentDataTable").DataTable({
        ajax: {
            url: "/Admin/Department",
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
        ],
        dom: "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { text: "輸出：", className: 'btn btn-secondary disabled' },
            { extend: "excel", className: "btn btn-success buttons-excel buttons-html5" },
            { extend: "csv", className: "btn btn-success buttons-csv buttons-html5" },
            { extend: "print", className: "btn btn-success buttons-print buttons-html5" }
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

$(function () {
    SearchDepartmentDataTableInitial();
    ColumnSearch();
});