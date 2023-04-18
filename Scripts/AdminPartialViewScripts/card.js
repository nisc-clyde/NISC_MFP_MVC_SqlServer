var datatable;
function SearchCardDataTableInitial() {
    datatable = $("#searchCardDataTable").DataTable({
        ajax: {
            url: "/Admin/Card/InitialDataTable",
            type: "POST",
            datatype: "json",
            data: { page: "card" }
        },
        columns: [
            { data: "card_id", name: "卡片編號" },
            { data: "value", name: "點數" },
            { data: "freevalue", name: "免費點數" },
            { data: "user_id", name: "使用者帳號" },
            { data: "user_name", name: "使用者姓名" },
            { data: "card_type", name: "屬性" },
            { data: "enable", name: "使用狀態" },
            {
                data: null,
                defaultContent: "<button type='button' class='btn btn-primary btn-sm me-1 btn-manager'><i class='fa-solid fa-circle-info me-1'></i>儲值</button>" +
                    "<button type='button' class='btn btn-info btn-sm me-1 btn-edit'><i class='fa-solid fa-pen-to-square me-1'></i>修改</button>" +
                    "<button type='button' class='btn btn-danger btn-sm btn-delete'><i class='fa-solid fa-trash me-1'></i>刪除</button>",
                orderable: false
            },
            { data: "serial", name: "serial" }
        ],
        columnDefs: [
            { width: "220px", targets: 7 },
            { visible: false, target: 8 }
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
    $("#searchCard_CardID").keyup(function () {
        datatable.columns(0).search($("#searchCard_CardID").val()).draw();

    });

    $("#searchCard_Point").keyup(function () {
        datatable.columns(1).search($("#searchCard_Point").val()).draw();

    });

    $("#searchCard_FreePort").keyup(function () {
        datatable.columns(2).search($("#searchCard_FreePort").val()).draw();

    });

    $("#searchCard_Account").keyup(function () {
        datatable.columns(3).search($("#searchCard_Account").val()).draw();

    });

    $("#searchCard_Name").keyup(function () {
        datatable.columns(4).search($("#searchCard_Name").val()).draw();

    });

    $("#searchCard_AttributeSelect").change(function () {
        if ($("#searchCard_AttributeSelect").val() != "") {
            datatable.columns(5).search($("#searchCard_AttributeSelect :selected").text()).draw();
        } else {
            datatable.columns(5).search("").draw();
        }
    });

    $("#searchCard_StatusSelect").change(function () {
        if ($("#searchCard_StatusSelect").val() != "") {
            datatable.columns(6).search($("#searchCard_StatusSelect :selected").text()).draw();
        } else {
            datatable.columns(6).search("").draw();
        }
    });
}

function PopupFormForAdd() {
    $("#btnAddCard").on("click", function () {
        var url = "/Admin/Card/AddCard"
        $.get(
            url,
            { formTitle: $(this).text() },
            function (data) {
                $("#cardForm").html(data);
                $("#cardForm").modal("show");
            }
        )
    });

    //$("#btnResetCardFreePoint").on("click", function () {
    //    var url = "/Admin/Card/ResetCardFreePoint"
    //    $.get(
    //        url,
    //        { formTitle: $(this).text() },
    //        function (data) {

    //            $("#cardForm").html(data);
    //            $("#cardForm").modal("show");
    //        })
    //});

};

function SubmitFormForAdd(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $("#cardForm").modal("hide");
                    datatable.ajax.reload();
                }
            }
        });
    }
    return false;
}


$(function () {
    SearchCardDataTableInitial();
    ColumnSearch();
    PopupFormForAdd();
});