var datatable;
function SearchWatermarkDataTableInitial() {
    datatable = $("#searchWatermarkDataTable").DataTable({
        ajax: {
            url: "/Admin/Watermark/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "watermark" }
        },
        columns: [
            { data: "type", name: "類別" },
            { data: "left_offset", name: "左邊偏移" },
            { data: "right_offset", name: "右邊偏移" },
            { data: "top_offset", name: "上邊偏移" },
            { data: "bottom_offset", name: "下邊偏移" },
            { data: "position_mode", name: "浮水印位置" },
            { data: "fill_mode", name: "填滿方式" },
            { data: "text", name: "文字" },
            { data: "image_path", name: "圖片位置" },
            { data: "rotation", name: "旋轉角度" },
            { data: "color", name: "顏色" }
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
    });
}

function ColumnSearch() {
    $("#searchWatermark_TypeSelect").change(function () {
        if ($("#searchWatermark_TypeSelect").val() != "0") {
            datatable.columns(0).search($("#searchWatermark_TypeSelect :selected").text()).draw();
        } else {
            datatable.columns(0).search("").draw();
        }
    });

    $("#searchWatermark_LeftBias").keyup(function () {
        datatable.columns(1).search($("#searchWatermark_LeftBias").val()).draw();

    });

    $("#searchWatermark_RightBias").keyup(function () {
        datatable.columns(2).search($("#searchWatermark_RightBias").val()).draw();

    });

    $("#searchWatermark_TopBias").keyup(function () {
        datatable.columns(3).search($("#searchWatermark_TopBias").val()).draw();

    });

    $("#searchWatermark_BottomBias").keyup(function () {
        datatable.columns(4).search($("#searchWatermark_BottomBias").val()).draw();

    });

    $("#searchWatermark_PositionSelect").change(function () {
        if ($("#searchWatermark_PositionSelect").val() != "0") {
            datatable.columns(5).search($("#searchWatermark_PositionSelect :selected").text()).draw();
        } else {
            datatable.columns(5).search("").draw();
        }
    });

    $("#searchWatermark_FillTypeSelect").change(function () {
        if ($("#searchWatermark_FillTypeSelect").val() != "0") {
            datatable.columns(6).search($("#searchWatermark_FillTypeSelect :selected").text()).draw();
        } else {
            datatable.columns(6).search("").draw();
        }
    });

    $("#searchWatermark_Text").keyup(function () {
        datatable.columns(7).search($("#searchWatermark_Text").val()).draw();

    });

    $("#searchWatermark_ImageLocation").keyup(function () {
        datatable.columns(8).search($("#searchWatermark_ImageLocation").val()).draw();

    });

    $("#searchWatermark_Rotate").keyup(function () {
        datatable.columns(9).search($("#searchWatermark_Rotate").val()).draw();

    });

    $("#searchWatermark_Color").keyup(function () {
        datatable.columns(10).search($("#searchWatermark_Color").val()).draw();

    });

}

function PopupForm() {
    $("#btnAddWatermark").on("click", function () {
        var url = $("#addWatermarkForm").data("url");
        $.get(
            url,
            { formTitle: $(this).text() },
            function (data) {
                $("#addWatermarkForm").html(data);
                $("#addWatermarkForm").modal("show");
            }
        )
    });
};

$(function () {
    SearchWatermarkDataTableInitial();
    ColumnSearch();
    PopupForm();
});