import { CustomSweetAlert2 } from "../AdminPartialViewScripts/Shared.js";

function RegisterAdmin() {
    $("#adminRegisterForm").on("submit", function () {
        $.ajax({
            url: "/Config/Administrator/ConfigAdminRegister",
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

function TestConnectionString() {
    $("#btnTestConnection").on("click", function () {

        $("#btnTestConnectionSpinner").show();
        $("#btnTestConnection").attr("disabled", true);

        if ($("#btnWindowsAuth").is(":checked")) {
            $("input[name=IntegratedSecurity]").val(true);
        } else {
            $("input[name=IntegratedSecurity]").val(false);
        }

        $.ajax({
            url: "/Config/Connection/TestConnectionString",
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
        
        $("#btnSaveConnectionStringSpinner").show();
        $("#btnSaveConnectionString").attr("disabled", true);
        
        $.ajax({
            url: "/Config/Connection/SetConnectionString",
            type: "POST",
            data: $("#databaseConfigForm").serialize(),
            success: function (data) {
                if (data.success) {
                    CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire();
                } else {
                    CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                }
            }
        }).done(function () {
            $("#btnSaveConnectionStringSpinner").hide();
            $("#btnSaveConnectionString").attr("disabled", false);
        });

        return false;
    })
}

function ServerAddressTestConnection() {
    $("#btnServerAddressTestConnection").on("click",
        function () {
            $("#btnServerAddressTestConnectionSpinner").show();
            $("#btnServerAddressTestConnection").attr("disabled", true);

            $.ajax({
                url: "/Config/Connection/ServerAddressTestConnection",
                type: "POST",
                data: $("#serverAddressConfigForm").serialize(),
                success: function (data) {
                    if (data.success) {
                        CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire();
                        $("#btnSaveServerAddress").attr("disabled", false);
                        $("#btnServerAddressTestConnectionSpinner").hide();
                        $("#btnServerAddressTestConnection").attr("disabled", false);
                    } else {
                        CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                        $("#btnSaveServerAddress").attr("disabled", true);
                        $("#btnServerAddressTestConnectionSpinner").hide();
                        $("#btnServerAddressTestConnection").attr("disabled", false);
                    }
                }
            });
            return false;
        });
}

function SaveServerAddress() {
    $("#serverAddressConfigForm").on("submit",
        function () {
            $("#btnSaveServerAddressSpinner").show();
            $("#btnSaveServerAddress").attr("disabled", true);

            $.ajax({
                url: "/Config/Connection/SetServerAddress",
                type: "POST",
                data: $("#serverAddressConfigForm").serialize(),
                success: function (data) {
                    if (data.success) {
                        CustomSweetAlert2.SweetAlertTemplateSuccess(data.message).fire();
                    } else {
                        CustomSweetAlert2.SweetAlertTemplateError(data.message).fire();
                    }
                }
            }).done(function () {
                $("#btnSaveServerAddressSpinner").hide();
                $("#btnSaveServerAddress").attr("disabled", false);
            });

            return false;
        });
}

$(function () {
    RegisterAdmin();
    WindowsOrSqlServerAuth();
    TestConnectionString();
    SaveConnectionStirng();
    ServerAddressTestConnection();
    SaveServerAddress();
});