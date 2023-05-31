import { CustomSweetAlert2 } from "../AdminPartialViewScripts/Shared.js";

function ConfigurationControlButton() {
    $("#btnNext").on("click", function () {
        $("#databaseDiv").hide();
        $("#adminRegisterDiv").show();
    });

    $("#btnPrevious").on("click", function () {
        $("#databaseDiv").show();
        $("#adminRegisterDiv").hide();
    });

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

function WindowsOrSqlServerAuth() {
    $("#btnWindowsAuth").on("click", function () {
        $("#btnWindowsAuth").attr("checked", true);
        $("#btnSqlServerAuth").attr("checked", false);
        $("#databaseAccountDiv").hide();
        $("#databasePasswordDiv").hide();
        $("#btnSaveConnectionString").attr("disabled", true);
    })
    $("#btnSqlServerAuth").on("click", function () {
        $("#btnWindowsAuth").attr("checked", false);
        $("#btnSqlServerAuth").attr("checked", true);
        $("#databaseAccountDiv").show();
        $("#databasePasswordDiv").show();
        $("#btnSaveConnectionString").attr("disabled", true);
    })
}

function TestConnection() {
    $("#btnTestConnection").on("click", function () {

        $("#btnTestConnectionSpinner").show();
        $("#btnTestConnection").attr("disabled", true);

        if ($("#btnWindowsAuth").is(":checked")) {
            $("input[name=integrated_security]").val(true);
        } else {
            $("input[name=integrated_security]").val(false);
        }

        $.ajax({
            url: "/Login/TestConnection",
            type: "POST",
            data: $("#databaseConfigForm").serialize(),
            success: function (data) {
                if (data.success) {
                    CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire();
                    $("#btnSaveConnectionString").attr("disabled", false);
                    $("#btnTestConnectionSpinner").hide();
                    $("#btnTestConnection").attr("disabled", false);
                } else {
                    CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                    $("#btnSaveConnectionString").attr("disabled", true);
                    $("#btnTestConnectionSpinner").hide();
                    $("#btnTestConnection").attr("disabled", false);
                }
            }
        })

        return false;
    })
}

function SaveConnectionStirng() {
    $("#databaseConfigForm").on("submit", function () {
        let url;

        if ($("#btnWindowsAuth").is(":checked")) {
            url = "/Login/SetWindowsAuthConnection";
        } else {
            url = "/Login/SetSqlServerAuthConnection";
        }

        $.ajax({
            url: url,
            type: "POST",
            data: $("#databaseConfigForm").serialize(),
            success: function (data) {
                if (data.success) {
                    CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire();
                } else {
                    CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                }
            }
        })

        return false;
    })
}

$(function () {
    ConfigurationControlButton();
    RegisterAdmin();
    WindowsOrSqlServerAuth();
    TestConnection();
    SaveConnectionStirng();
});