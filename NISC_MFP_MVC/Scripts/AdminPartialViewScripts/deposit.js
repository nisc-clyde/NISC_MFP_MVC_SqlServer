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
function SearchDepositDataTableInitial() {
    const table = $("#searchDepositDataTable");
    const url = "/Admin/Deposit/InitialDataTable";
    const page = "deposit";
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
    dataTable.draw();

};

/**
 * 輸入欲搜尋之欄位資料並Refresh DataTable
 */
function ColumnSearch() {
    $("#searchDeposit_Name").keyup(function () {
        dataTable.columns(0).search($("#searchDeposit_Name").val()).draw();

    });

    $("#searchDeposit_Account").keyup(function () {
        dataTable.columns(1).search($("#searchDeposit_Account").val()).draw();

    });

    $("#searchDeposit_CardNumber").keyup(function () {
        dataTable.columns(2).search($("#searchDeposit_CardNumber").val()).draw();

    });

    $("#searchDeposit_CardOwnerAccount").keyup(function () {
        dataTable.columns(3).search($("#searchDeposit_CardOwnerAccount").val()).draw();

    });

    $("#searchDeposit_CardOwnerName").keyup(function () {
        dataTable.columns(4).search($("#searchDeposit_CardOwnerName").val()).draw();
    });

    $("#searchDeposit_BeforePoint").keyup(function () {
        dataTable.columns(5).search($("#searchDeposit_BeforePoint").val()).draw();
    });

    $("#searchDeposit_Point").keyup(function () {
        dataTable.columns(6).search($("#searchDeposit_Point").val()).draw();
    });

    $("#searchDeposit_AfterPoint").keyup(function () {
        dataTable.columns(7).search($("#searchDeposit_AfterPoint").val()).draw();
    });

    $("#searchDeposit_Time").keyup(function () {
        dataTable.columns(8).search($("#searchDeposit_Time").val()).draw();
    });
}

$(function () {
    SearchDepositDataTableInitial();
    ColumnSearch();
})