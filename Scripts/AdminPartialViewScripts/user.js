function FormSelect() {
    $("#searchUser_ColorPermissionSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

function SearchUserDataTableInitial() {
    $("#searchUserDataTable").DataTable({
        ajax: {
            url: "/Admin/User",
            type: "POST",
            datatype: "json"
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
            info: "顯示第 _START_ 至 _END_ 筆資料，共 _TOTAL_ 筆"
        }
    });
}

$(function () {
    FormSelect();
    SearchUserDataTableInitial();
});