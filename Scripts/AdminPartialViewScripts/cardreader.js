function FormSelect() {

    $("#searchCardReader_CardMachineTypeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCardReader_WorkModeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCardReader_CardOnOffSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCardReader_CardStatusSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

function SearchCardReaderDataTableInitial() {
    $("#searchCardReaderDataTable").DataTable({
        ajax: {
            url: "/Admin/CardReader",
            type: "POST",
            datatype: "json"
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
    SearchCardReaderDataTableInitial();
});