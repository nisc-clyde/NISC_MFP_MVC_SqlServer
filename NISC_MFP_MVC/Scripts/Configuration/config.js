import { CustomSweetAlert2 } from "../AdminPartialViewScripts/Shared.js";

function ConfigurationNext() {
    $("#btnNext").on("click", function () {
        $("#databaseDiv").hide();
        $("#adminRegisterDiv").show();
    });
}

function ConfigurationPrevious() {
    $("#btnPrevious").on("click", function () {
        $("#databaseDiv").show();
        $("#adminRegisterDiv").hide();
    });
}

function ConfigurationSkip() {
    $("#btnSkip").on("click", function () {
        window.location.replace("/Login/Admin");
    });
}

function RegisterAdmin() {
    $("#adminRegisterForm").on("submit", function () {
        $.ajax({
            url: "/Login/ConfigAdminRegister",
            type: "POST",
            data: $(this).serialize(),
            success: function (data) {
                if (data.success) {
                    CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire()
                        .then((result) => {
                            if (result.isConfirmed) {
                                $("#adminRegisterForm").trigger("reset");
                            }
                        });
                } else {
                    CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                }
            }
        })
        return false;
    });
}

$(function () {
    ConfigurationNext();
    ConfigurationPrevious();
    ConfigurationSkip();
    RegisterAdmin();
});