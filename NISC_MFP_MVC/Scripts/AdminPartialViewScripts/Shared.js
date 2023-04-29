export const CustomSweetAlert2 = (function () {

    function SweetAlertTemplateHome() {
        return Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success ms-2',
                cancelButton: 'btn btn-danger me-2'
            },
            buttonsStyling: false,
            title: "確定刪除此筆資料嗎?",
            text: "此操作成功後即無法復原，請再次確認此為欲刪除之資料",
            icon: 'warning',
            showCancelButton: true,
            reverseButtons: true,
            confirmButtonText: "確定",
            cancelButtonText: "取消",
            width: 800
        });
    }

    function SweetAlertTemplateSuccess() {
        return Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success',
            },
            buttonsStyling: false,
            allowOutsideClick: false,
            title: "成功?",
            text: "此資料已刪除",
            icon: 'success',
            confirmButtonText: "確定",
            timer: 1300,
            timerProgressBar: true,
        })
    }

    return {
        SweetAlertTemplateHome: SweetAlertTemplateHome,
        SweetAlertTemplateSuccess: SweetAlertTemplateSuccess,
    };

})();

export const RequestAddOrEdit = (function () {

    function GetAddOrEditTemplate(btnAdd, modalForm, dataTable, uniqueIdProperty) {
        const url = $("#" + modalForm).data("url");

        //Add Department
        $("#" + btnAdd).on("click", function () {
            $.get(
                url,
                { formTitle: $(this).text(), serial: -1 },
                function (data) {
                    $("#" + modalForm).html(data);
                    $("#" + modalForm).modal("show");
                    $("#addOrEditForm").on("submit", function () {
                        PostAddOrEditTemplate(this, modalForm, dataTable);
                        return false;
                    })
                }
            )
        });

        //Edit Department
        dataTable.on("click", ".btn-edit", function (e) {
            e.preventDefault();
            const currentRow = $(this).closest("tr");
            const rowData = dataTable.row(currentRow).data();

            $.get(
                url,
                { formTitle: $(this).text() + "部門", serial: rowData[uniqueIdProperty] },
                function (data) {
                    $("#" + modalForm).html(data);
                    $("#" + modalForm).modal("show");
                    $("#" + modalForm + " .modal-footer button:submit").text("修改");
                    $("#" + modalForm + " .modal-footer button:submit").val("Edit");

                    $("#addOrEditForm").on("submit", function () {
                        PostAddOrEditTemplate(this, modalForm, dataTable);
                        return false;
                    })
                }
            )
        });
    }

    function PostAddOrEditTemplate(form, modalForm, dataTable) {
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            $.ajax({
                type: "POST",
                url: form.action,
                data: $(form).serialize() + "&currentOperation=" + $("#btnSubmmit").val(),
                success: function (data) {
                    if (data.success) {
                        $("#" + modalForm).modal("hide");
                        dataTable.ajax.reload();
                    }
                }
            });
        }
    }

    return {
        GetAddOrEditTemplate: GetAddOrEditTemplate,
        PostAddOrEditTemplate: PostAddOrEditTemplate,
    };

})();

export const RequestDelete = (function () {

    function GetAndPostDeleteTemplate(dataTable, uniqueIdProperty,getURL,postURL) {

        dataTable.on("click", ".btn-delete", function (e) {
            e.preventDefault();

            const currentRow = $(this).closest("tr");
            const rowData = dataTable.row(currentRow).data();
            const sweetAlertHome = CustomSweetAlert2.SweetAlertTemplateHome();
            const sweetAlertSuccess = CustomSweetAlert2.SweetAlertTemplateSuccess();

            $.get(
                getURL,
                { serial: rowData[uniqueIdProperty] },
                function (data) {

                    var dataTableAsFormSerialize = uniqueIdProperty+"=" + rowData[uniqueIdProperty];
                    const sweetalertHtml = $.parseHTML(data);
                    const dataTableHtml = $("#deleteRowData", sweetalertHtml);

                    dataTableHtml.find("th").each(function () {
                        dataTableAsFormSerialize += "&" + this.id + "=" + $(this).text();
                    })

                    sweetAlertHome.fire({
                        html: data,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $.ajax({
                                type: "POST",
                                url: postURL,
                                data: dataTableAsFormSerialize,
                                success: function (data) {
                                    if (data.success) {
                                        sweetAlertSuccess.fire()
                                            .then((result) => {
                                                if (result.isConfirmed || result.dismiss == Swal.DismissReason.timer) {
                                                    currentRow.fadeOut(600, function () {
                                                        dataTable.row(currentRow).remove().draw()
                                                    });
                                                    //dataTable.ajax.reload();
                                                }
                                            })
                                    }
                                }
                            });
                        }
                    })
                }
            )
        });
    }

    return {
        GetAndPostDeleteTemplate: GetAndPostDeleteTemplate,
    };

})();

