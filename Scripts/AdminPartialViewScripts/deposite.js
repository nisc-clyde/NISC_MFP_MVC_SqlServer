function SearchDepositeDataTableInitial() {
    $("#searchDepositeDataTable").DataTable({
        ajax: {
            url: "/Admin/Deposite",
            type: "POST",
            datatype: "json"
        }, columns: [
            { data: "user_name", name: "儲值人員" },
            { data: "user_id", name: "儲值帳號" },
            { data: "card_id", name: "儲值卡號" },
            { data: "card_user_id", name: "卡號持有者帳號" },
            { data: "card_user_name", name: "卡號持有者姓名" },
            { data: "pbalance", name: "儲值前點數" },
            { data: "deposit_value", name: "儲值點數" },
            { data: "final_value", name: "儲值後點數" },
            { data: "deposit_date", name: "儲值時間" }
        ],
        dom: "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            { extend: "excel", className: "btn btn-success buttons-excel buttons-html5" },
            { extend: "csv", className: "btn btn-success buttons-csv buttons-html5" },
            { extend: "print", className: "btn btn-success buttons-print buttons-html5" }
        ],
        order: [8, "desc"],
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
};

$(function () {
    SearchDepositeDataTableInitial();
})