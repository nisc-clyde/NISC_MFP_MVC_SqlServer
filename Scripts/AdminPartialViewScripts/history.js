function SearchHistoryDataTableInitial() {

    //$date_time, $login_user_id, $login_user_name, $operation, $affected_data
    $("#searchHistoryDataTable").DataTable({
        ajax: {
            url: "/Admin/History/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: {page:"history"}
        }, columns: [
            { data: "date_time", name: "時間" },
            { data: "login_user_id", name: "帳號" },
            { data: "login_user_name", name: "姓名" },
            { data: "operation", name: "操作動作" },
            { data: "affected_data", name: "操作資料" },
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

$(function () {
    SearchHistoryDataTableInitial();
})