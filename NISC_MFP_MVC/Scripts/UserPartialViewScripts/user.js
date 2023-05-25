import { CustomSweetAlert2, DataTableTemplate } from "../AdminPartialViewScripts/Shared.js";

function UsedValue() {
    $("#userCardUsedValue").text($("#userCardSelect option:selected").val());
    $("#userCardSelect").on("change", function () {
        $("#userCardUsedValue").text($("#userCardSelect option:selected").val());
    })
}

function PopupFormForEdit() {
    const modalForm = "generalUserForm";
    const url = $("#" + modalForm).data("url");

    $("#btnEditUserPassword").on("click", function (e) {
        e.preventDefault();

        $.get(url, { user_id: $("#user_idDiv").text() }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");

            $("#userEditForm").on("submit", function () {
                $.validator.unobtrusive.parse($("#userEditForm"));
                if ($("#userEditForm").valid()) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: $(this).serialize(),
                        success: function (data) {
                            console.log(data.message);
                            if (data.success) {
                                $("#" + modalForm).modal("hide");
                                CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire()
                                    .then((result) => {
                                        if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
                                            $.get("/User/LogOut/LogOutForJavaScript");
                                        }
                                    });
                            } else {
                                CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                            }
                        },
                    });
                }
                return false;
            });
        });
    });
}


$(function () {
    UsedValue();
    PopupFormForEdit();
})