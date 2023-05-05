﻿export const CustomSweetAlert2 = (function () {

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
            title: "成功",
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

    function GetAddOrEditTemplate(btnAdd, modalForm, dataTable, title) {
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

            let currentRow;
            if ($(this).parents("tr").prev().hasClass("dt-hasChild")) {
                //Is In Responsiveness
                currentRow = $(this).parents("tr").prev();
            } else {
                //Not In Responsiveness
                currentRow = $(this).closest("tr");
            }

            $.get(
                url,
                { formTitle: $(this).text() + title, serial: $(this).data("id") },
                function (data) {
                    $("#" + modalForm).html(data);
                    $("#" + modalForm).modal("show");
                    $("#" + modalForm + " .modal-footer button:submit").text("修改");
                    $("#" + modalForm + " .modal-footer button:submit").val("Edit");

                    $("#addOrEditForm").on("submit", function () {

                        PostAddOrEditTemplate(this, modalForm, dataTable, currentRow);
                        return false;
                    })
                }
            )
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
                        } else {
                            $("#" + modalForm).modal("hide");
                            currentRow.addClass("animate__animated animate__flash animate__faster animate__repeat-2");
                            currentRow.on("animationend", function () {
                                currentRow.removeClass("animate__animated animate__flash animate__faster animate__repeat-2");
                                dataTable.row(currentRow).data(form).draw();
                            })
                        }
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

    function GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, getURL, postURL) {
        dataTable.on("click", ".btn-delete", function (e) {
            e.preventDefault();

            const currentRow = $(this).closest("tr");
            const uniqueId = $(this).data("id");
            const rowData = dataTable.row(currentRow).data();
            const sweetAlertHome = CustomSweetAlert2.SweetAlertTemplateHome();
            const sweetAlertSuccess = CustomSweetAlert2.SweetAlertTemplateSuccess();

            $.get(
                getURL,
                { serial: uniqueId },
                function (data) {

                    var dataTableAsFormSerialize = uniqueIdProperty + "=" + uniqueId;
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
                                    dataTable.row(currentRow).remove().draw()
                                    if (data.success) {
                                        sweetAlertSuccess.fire();
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

