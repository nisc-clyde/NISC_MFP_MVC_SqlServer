import { DataTableTemplate } from "./Shared.js"

/**
 * Visualization the data from Database
 * @param {Element} table - The table which you want populate.
 * @param {string} url - The table resource request from.
 * @param {string} page - Identify columns index when  request to controller.
 * @param {string[]} columns - Declare all columns will exists.
 * @param {string[]} columnDefs - Custom column definition your self.
 * @param {string[]} order - Specify default orderable by column when data table already.
 * @param {function} rowCallback - Do things between response from backend and render to element, 
 */
var dataTable;
function SearchDepositeDataTableInitial() {
    const table = $("#searchDepositeDataTable");
    const url = "/Admin/Deposite/InitialDataTable";
    const page = "deposite";
    const columns = [
        { data: "user_name", name: "儲值人員" },
        { data: "user_id", name: "儲值帳號" },
        { data: "card_id", name: "儲值卡號" },
        { data: "card_user_id", name: "卡號持有者帳號" },
        { data: "card_user_name", name: "卡號持有者姓名" },
        { data: "pbalance", name: "儲值前點數" },
        { data: "deposit_value", name: "儲值點數" },
        { data: "final_value", name: "儲值後點數" },
        { data: "deposit_date", name: "儲值時間" }
    ];
    const columnDefs = [];
    const order = [8, "desc"];
    const rowCallback = function () { };

    dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, columnDefs, order, rowCallback);
};

function ColumnSearch() {
    $("#searchDeposite_Name").keyup(function () {
        dataTable.columns(0).search($("#searchDeposite_Name").val()).draw();

    });

    $("#searchDeposite_Account").keyup(function () {
        dataTable.columns(1).search($("#searchDeposite_Account").val()).draw();

    });

    $("#searchDeposite_CardNumber").keyup(function () {
        dataTable.columns(2).search($("#searchDeposite_CardNumber").val()).draw();

    });

    $("#searchDeposite_CardOwnerAccount").keyup(function () {
        dataTable.columns(3).search($("#searchDeposite_CardOwnerAccount").val()).draw();

    });

    $("#searchDeposite_CardOwnerName").keyup(function () {
        dataTable.columns(4).search($("#searchDeposite_CardOwnerName").val()).draw();
    });

    $("#searchDeposite_BeforePoint").keyup(function () {
        dataTable.columns(5).search($("#searchDeposite_BeforePoint").val()).draw();
    });

    $("#searchDeposite_Point").keyup(function () {
        dataTable.columns(6).search($("#searchDeposite_Point").val()).draw();
    });

    $("#searchDeposite_AfterPoint").keyup(function () {
        dataTable.columns(7).search($("#searchDeposite_AfterPoint").val()).draw();
    });

    $("#searchDeposite_Time").keyup(function () {
        dataTable.columns(8).search($("#searchDeposite_Time").val()).draw();
    });
}

$(function () {
    SearchDepositeDataTableInitial();
    ColumnSearch();
})