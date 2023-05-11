
var dataTable;
function EmployeePreviewInitial() {
    $("#btnAddFromFile, #btnOverrideFromFile").on("click", function () {
        bootstrap.Tooltip.getOrCreateInstance($(this)).hide();// Hide Tooltips
        const uploadFileURL = "/Admin/System/UploadFile";
        const viewEmployeePreviewURL = "/Admin/System/PreviewEmployee";
        const modalForm = "employeePreviewModal";
        let currentOperation = $(this).text();

        let formData = new FormData();
        let files = $("#fileResource").get(0).files;
        if (files.length > 0) formData.append("EmployeeSource", files[0]);

        $.ajax({
            url: uploadFileURL,
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: formData,
            success: function (response) {
                if (response.success) {
                    $.get(
                        viewEmployeePreviewURL,
                        { formTitle: "資料預覽 — " + currentOperation },
                        function (data) {
                            $("#" + modalForm).html(data);
                            $("#" + modalForm).modal("show");

                            EmployeePreviewDataTableInitial($("#employeePreviewDataTable"));
                        })
                } else {
                    Swal.fire({
                        customClass: {
                            confirmButton: 'btn btn-danger',
                        },
                        buttonsStyling: false,
                        allowOutsideClick: false,
                        title: '失敗',
                        text: response.message,
                        icon: 'error',
                    })
                }
            }
        })
    })
}

function EmployeePreviewDataTableInitial(table, url, columns, rowCallback) {
    const populateEmployeePreview = "/Admin/System/PreviewEmployeeDataTable";

    dataTable = table.DataTable({
        ajax: {
            url: populateEmployeePreview,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "card_id", name: "卡片編號" },
            { data: "dept_id", name: "部門編號" },
            { data: "dept_name", name: "部門名稱" },
            { data: "user_name", name: "姓名" },
            { data: "user_id", name: "帳號" },
            { data: "work_id", name: "工號" },
            { data: "card_type", name: "屬性" },
            { data: "enable", name: "使用狀態" },
            { data: "e_mail", name: "信箱" }
        ],
        dom: "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
        paging: true,
        pagingType: 'full_numbers',
        deferRender: true,
        serverSide: true,
        processing: true,
        ordering: false,
        searhcing: false,
        language: {
            processing: "資料載入中...請稍後",
            paginate: {
                first: "首頁",
                last: "尾頁",
                previous: "上一頁",
                next: "下一頁"
            },
            info: "顯示第 _START_ 至 _END_ 筆資料<br/>共 _TOTAL_ 筆",
            zeroRecords: "找不到相符資料",
            infoFiltered: ""
        },
        rowCallback: function (row, data) {
            if (data.card_type == "1") {
                $('td:eq(6)', row).html("<b class='text-success'>遞增</b>");
            } else {
                $('td:eq(6)', row).html("<b class='text-danger'>遞減</b>");
            }

            if (data.enable == "1") {
                $('td:eq(7)', row).html("<b class='text-success'>可用</b>");
            } else {
                $('td:eq(7)', row).html("<b class='text-danger'>停用</b>");
            }
        }
    })
    dataTable.columns.adjust().draw();
}

function AddEmployee() {

}

function OverrideEmployee() {

}

$(function () {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))
    EmployeePreviewInitial();

})