var datatable;
function SearchDepositeDataTableInitial() {
    datatable=$("#searchDepositeDataTable").DataTable({
        ajax: {
            url: "/Admin/Deposite",
            type: "POST",
            datatype: "json",
            data: { page: "deposite" }
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
            { text: "輸出：", className: 'btn btn-secondary disabled' },
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

function ColumnSearch() {
    $("#searchDeposite_Name").keyup(function () {
        datatable.columns(0).search($("#searchDeposite_Name").val()).draw();

    });

    $("#searchDeposite_Account").keyup(function () {
        datatable.columns(1).search($("#searchDeposite_Account").val()).draw();

    });

    $("#searchDeposite_CardNumber").keyup(function () {
        datatable.columns(2).search($("#searchDeposite_CardNumber").val()).draw();

    });

    $("#searchDeposite_CardOwnerAccount").keyup(function () {
        datatable.columns(3).search($("#searchDeposite_CardOwnerAccount").val()).draw();

    });

    $("#searchDeposite_CardOwnerName").keyup(function () {
        datatable.columns(4).search($("#searchDeposite_CardOwnerName").val()).draw();
    });

    $("#searchDeposite_BeforePoint").keyup(function () {
        datatable.columns(5).search($("#searchDeposite_BeforePoint").val()).draw();
    });

    $("#searchDeposite_Point").keyup(function () {
        datatable.columns(6).search($("#searchDeposite_Point").val()).draw();
    });

    $("#searchDeposite_AfterPoint").keyup(function () {
        datatable.columns(7).search($("#searchDeposite_AfterPoint").val()).draw();
    });

    $("#searchDeposite_Time").keyup(function () {
        datatable.columns(8).search($("#searchDeposite_Time").val()).draw();
    });
}

$(function () {
    SearchDepositeDataTableInitial();
    ColumnSearch();
})