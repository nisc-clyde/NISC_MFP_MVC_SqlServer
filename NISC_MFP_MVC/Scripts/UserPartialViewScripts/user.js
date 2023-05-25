import { CustomSweetAlert2, DataTableTemplate } from "../AdminPartialViewScripts/Shared.js";

/**
 * 選擇使用者卡片後代入已使用點數之值
 */
function UsedValue() {
    $("#userCardUsedValue").text($("#userCardSelect option:selected").val());
    $("#userCardSelect").on("change", function () {
        $("#userCardUsedValue").text($("#userCardSelect option:selected").val());
    })
}

/**
 * 載入Edit Partial View並更新User資料
 */
function PopupFormForEdit() {
    const modalForm = "generalUserForm";
    const url = $("#" + modalForm).data("url");

    $("#btnEditUserPassword").on("click", function (e) {
        e.preventDefault();

        /**
         * 取得Partial View
         */
        $.get(url, { user_id: $("#user_idDiv").text() }, function (data) {
            $("#" + modalForm).html(data);
            $("#" + modalForm).modal("show");

            /**
             * 提交更新
             */
            $("#userEditForm").on("submit", function () {
                $.validator.unobtrusive.parse($("#userEditForm"));
                if ($("#userEditForm").valid()) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: $(this).serialize(),
                        success: function (data) {
                            /**
                             * 更新成功後自動登出，否則提示告知錯誤
                             */
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