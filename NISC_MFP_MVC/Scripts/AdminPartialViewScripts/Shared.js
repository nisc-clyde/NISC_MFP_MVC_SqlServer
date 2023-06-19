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
                                    dataTable.row(currentRow).draw();
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
                    exportOptions: { columns: "th:not(:last-child):visible"},
                    className: "btn btn-warning buttons-print buttons-html5",
                    customize: function (win) {
                        $(win.document.body).find('h1').css('text-align', 'center');
                        $(win.document.body)
                            .css('font-size', '10pt')
                            .prepend(
                                "<img src='data:image/png;base64,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'" +
                                "style = 'opacity:0.2; position:absolute; top:0; bottom:0; left:0; right:0; margin:auto; width:70%;' />"
                        );
                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
                    },
                },
            ],
            order: order,
            paging: true,
            pagingType: "full_numbers",
            deferRender: true,
            deferLoading: true,
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
                infoEmpty: "顯示第 0 至 0 筆資料，共 0 筆",
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
