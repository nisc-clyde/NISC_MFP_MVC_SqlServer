export const CustomSweetAlert2 = (function () {
    function SweetAlertTemplateHome(message = "此操作成功後即無法復原，請再次確認此為欲刪除之資料") {
        return Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success ms-2",
                cancelButton: "btn btn-danger me-2",
            },
            focusCancel: true,
            buttonsStyling: false,
            title: "確定刪除此筆資料嗎?",
            text: message,
            icon: "warning",
            showCancelButton: true,
            reverseButtons: true,
            confirmButtonText: "確定",
            cancelButtonText: "取消",
            width: 800,
        });
    }

    function SweetAlertTemplateSuccess(message = "資料已刪除") {
        return Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
            },
            buttonsStyling: false,
            allowOutsideClick: false,
            title: "操作成功",
            text: message,
            icon: "success",
            confirmButtonText: "確定",
            timer: 1300,
            timerProgressBar: true,
        });
    }

    function SweetAlertTemplateError(message = "未知的錯誤") {
        return Swal.mixin({
            customClass: {
                confirmButton: "btn btn-danger",
            },
            buttonsStyling: false,
            allowOutsideClick: false,
            title: "操作失敗",
            text: message,
            icon: "error",
        });
    }

    return {
        SweetAlertTemplateHome: SweetAlertTemplateHome,
        SweetAlertTemplateSuccess: SweetAlertTemplateSuccess,
        SweetAlertTemplateError: SweetAlertTemplateError,
    };
})();

export const RequestAddOrEdit = (function () {
    function GetAddOrEditTemplate(btnAdd, modalForm, dataTable, title) {
        const url = $("#" + modalForm).data("url");

        //Add
        $("#" + btnAdd).on("click", function () {
            $.get(url, { formTitle: $(this).text(), serial: -1 }, function (data) {
                $("#" + modalForm).html(data);
                $("#" + modalForm).modal("show");
                $("#addOrEditForm").on("submit", function () {
                    PostAddOrEditTemplate(this, modalForm, dataTable);
                    return false;
                });
            });
        });

        //Edit
        dataTable.on("click", ".btn-edit", function (e) {
            e.preventDefault();

            let currentRow;
            if ($(this).parents("tr").prev().hasClass("dt-hasChild")) {
                //Is In Responsiveness
                currentRow = $(this).parents("tr").prev();
            } else {
                //Not In Responsiveness
                currentRow = $(this).closest("tr");
            }

            $.get(url, { formTitle: $(this).text() + title, serial: $(this).data("id") }, function (data) {
                $("#" + modalForm).html(data);
                $("#" + modalForm).modal("show");
                $("#" + modalForm + " .modal-footer button:submit").text("修改");
                $("#" + modalForm + " .modal-footer button:submit").val("Edit");

                $("#addOrEditForm").on("submit", function () {
                    PostAddOrEditTemplate(this, modalForm, dataTable, currentRow);
                    return false;
                });
            });
        });
    }

    function PostAddOrEditTemplate(form, modalForm, dataTable, currentRow) {
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            $.ajax({
                type: "POST",
                url: form.action,
                data: $(form).serialize() + "&currentOperation=" + $("#btnSubmmit").val(),
                success: function (data) {
                    if (data.success) {
                        if ($("#btnSubmmit").val() == "Add") {
                            $("#" + modalForm).modal("hide");
                            dataTable.ajax.reload();
                            CustomSweetAlert2.SweetAlertTemplateSuccess("新增成功").fire();
                        } else {
                            //除User Page之外的Page修改，isCurrentUserUpdate必為空
                            if (data.isCurrentUserUpdate == null || data.isCurrentUserUpdate.updatedUserId != data.isCurrentUserUpdate.currentUserId) {
                                $("#" + modalForm).modal("hide");
                                CustomSweetAlert2.SweetAlertTemplateSuccess("修改成功").fire();
                                currentRow.addClass("animate__animated animate__flash animate__faster animate__repeat-2");
                                currentRow.on("animationend", function () {
                                    currentRow.removeClass("animate__animated animate__flash animate__faster animate__repeat-2");
                                    dataTable.row(currentRow).data(form).draw();
                                });
                            } else {
                                //只有對當前登入的User進行資料修改時才執行，在User Page時isCurrentUserUpdate非空
                                $("#" + modalForm).modal("hide");
                                CustomSweetAlert2.SweetAlertTemplateSuccess()
                                    .fire({
                                        title: "修改成功",
                                        text: "此修改之使用者資料為當前登入使用者，系統將自動登出，請重新登入",
                                        icon: "warning",
                                        timer: 5000,
                                    })
                                    .then((result) => {
                                        if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
                                            $.get("/Admin/LogOut/LogOutForJavaScript");
                                        }
                                    });
                            }
                        }
                    } else {
                        Swal.fire({
                            customClass: {
                                confirmButton: "btn btn-danger",
                            },
                            buttonsStyling: false,
                            allowOutsideClick: false,
                            title: "操作失敗",
                            text: data.message,
                            icon: "error",
                        });
                    }
                },
            });
        }
    }

    return {
        GetAddOrEditTemplate: GetAddOrEditTemplate,
        PostAddOrEditTemplate: PostAddOrEditTemplate,
    };
})();

export const RequestDelete = (function () {
    function GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, url) {
        dataTable.on("click", ".btn-delete", function (e) {
            e.preventDefault();

            const currentRow = $(this).closest("tr");
            const uniqueId = $(this).data("id");
            const rowData = dataTable.row(currentRow).data();

            $.get(url, { serial: uniqueId }, function (data) {
                var dataTableAsFormSerialize = uniqueIdProperty + "=" + uniqueId;
                const sweetalertHtml = $.parseHTML(data);
                const dataTableHtml = $("#deleteRowData", sweetalertHtml);

                dataTableHtml.find("th").each(function () {
                    dataTableAsFormSerialize += "&" + this.id + "=" + $(this).text();
                });

                CustomSweetAlert2.SweetAlertTemplateHome().fire({
                    html: data,
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: "POST",
                            url: url,
                            data: dataTableAsFormSerialize,
                            success: function (data) {
                                dataTable.row(currentRow).remove().draw();
                                if (data.success) {
                                    if (data.isCurrentUserUpdate == null || data.isCurrentUserUpdate.updatedUserId != data.isCurrentUserUpdate.currentUserId) {
                                        CustomSweetAlert2.SweetAlertTemplateSuccess().fire();
                                    } else {
                                        //只有在User Page對當前登入使用者進行Delete時才執行
                                        CustomSweetAlert2.SweetAlertTemplateSuccess()
                                            .fire({
                                                title: "修改成功",
                                                text: "此刪除之使用者資料為當前登入使用者，系統將自動登出，請重新登入",
                                                icon: "warning",
                                                timer: 5000,
                                            })
                                            .then((result) => {
                                                if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
                                                    $.get("/Admin/LogOut/LogOutForJavaScript");
                                                }
                                            });
                                    }
                                }
                            },
                        });
                    }
                });

            });
        });
    }

    return {
        GetAndPostDeleteTemplate: GetAndPostDeleteTemplate,
    };
})();

export const DataTableTemplate = (function () {
    function DataTableInitial(table, url, page, columns, columnDefs, order, rowCallback) {
        return table.DataTable({
            ajax: {
                url: url,
                type: "POST",
                datatype: "json",
                data: { page: page },
            },
            columns: columns,
            columnDefs: columnDefs,
            dom:
                "<'row'<'col-sm-12 col-md-6 text-start'B><'col-sm-12 col-md-6'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5 text-start'i><'col-sm-12 col-md-7'p>>",
            buttons: [
                { text: "輸出：", className: "btn btn-secondary disabled" },
                { extend: "excel", className: "btn btn-warning buttons-excel buttons-html5" },
                { extend: "csv", bom: true, className: "btn btn-warning buttons-csv buttons-html5" },
                {
                    extend: "print",
                    exportOptions: { columns: "th:not(:last-child):visible" },
                    className: "btn btn-warning buttons-print buttons-html5",
                },
            ],
            order: order,
            paging: true,
            pagingType: "full_numbers",
            deferRender: true,
            deferLoading:true,
            serverSide: true,
            processing: true,
            responsive: true,
            language: {
                processing: "資料載入中...請稍後",
                paginate: {
                    first: "首頁",
                    last: "尾頁",
                    previous: "上一頁",
                    next: "下一頁",
                },
                info: "顯示第 _START_ 至 _END_ 筆資料，共 _TOTAL_ 筆",
                zeroRecords: "找不到相符資料",
                search: "全部欄位搜尋：",
                infoFiltered: "",
            },
            rowCallback: rowCallback,
        });
    }

    return {
        DataTableInitial: DataTableInitial,
    };
})();
