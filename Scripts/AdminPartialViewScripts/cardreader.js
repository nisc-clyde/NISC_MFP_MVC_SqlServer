var datatable;
function SearchCardReaderDataTableInitial() {
    datatable = $("#searchCardReaderDataTable").DataTable({
        ajax: {
            url: "/Admin/CardReader",
            type: "POST",
            datatype: "json",
            data: { page: "cardreader" }
        },
        columns: [
            { data: "cr_id", name: "卡機編號" },
            { data: "cr_ip", name: "IP位置" },
            { data: "cr_port", name: "PORT" },
            { data: "cr_type", name: "卡機種類" },
            { data: "cr_mode", name: "運作模式" },
            { data: "cr_card_switch", name: "卡號判斷開關" },
            { data: "cr_status", name: "狀態" },
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
        },
        rowCallback: function (row, data) {
            if (data.cr_mode == "連線") {
                $('td:eq(4)', row).html("<b class='text-success'>連線</b>");
            } else {
                $('td:eq(4)', row).html("<b class='text-success'>連線</b>");
            }
        }
    });
}

function ColumnSearch() {
    $("#searchCardReader_CardreaderID").keyup(function () {
        datatable.columns(0).search($("#searchCardReader_CardreaderID").val()).draw();

    });

    $("#searchCardReader_IPAddress").keyup(function () {
        datatable.columns(1).search($("#searchCardReader_IPAddress").val()).draw();

    });

    $("#searchCardReader_Port").keyup(function () {
        datatable.columns(2).search($("#searchCardReader_Port").val()).draw();

    });

    $("#searchCardReader_CardMachineTypeSelect").change(function () {
        if ($("#searchCardReader_CardMachineTypeSelect").val() != "0") {
            datatable.columns(3).search($("#searchCardReader_CardMachineTypeSelect :selected").text()).draw();
        } else {
            datatable.columns(3).search("").draw();
        }
    });

    $("#searchCardReader_WorkModeSelect").change(function () {
        if ($("#searchCardReader_WorkModeSelect").val() != "0") {
            datatable.columns(4).search($("#searchCardReader_WorkModeSelect :selected").text()).draw();
        } else {
            datatable.columns(4).search("").draw();
        }
    });

    $("#searchCardReader_CardOnOffSelect").change(function () {
        if ($("#searchCardReader_CardOnOffSelect").val() != "0") {
            datatable.columns(5).search($("#searchCardReader_CardOnOffSelect :selected").text()).draw();
        } else {
            datatable.columns(5).search("").draw();
        }
    });

    $("#searchCardReader_CardStatusSelect").change(function () {
        if ($("#searchCardReader_CardStatusSelect").val() != "0") {
            datatable.columns(6).search($("#searchCardReader_CardStatusSelect :selected").text()).draw();
        } else {
            datatable.columns(6).search("").draw();
        }
    });
}

function PopupForm() {
    $("#btnAddCardReader").on("click", function () {
        var url = $("#addCardReaderForm").data("url");
        $.get(url, function (data) {
            $("#addCardReaderForm").html(data);
            $("#addCardReaderForm").modal("show");
        })
    });
};


$(function () {
    SearchCardReaderDataTableInitial();
    ColumnSearch();
    PopupForm();
});