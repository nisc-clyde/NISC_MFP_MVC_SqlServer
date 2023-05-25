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
function SearchHistoryDataTableInitial() {

    const table = $("#searchHistoryDataTable");
    const url = "/Admin/History/InitialDataTable";
    const page = "history";
    const columns = [
        { data: "date_time", name: "時間" },
        { data: "login_user_id", name: "帳號" },
        { data: "login_user_name", name: "姓名" },
        { data: "operation", name: "操作動作" },
        { data: "affected_data", name: "操作資料" },
    ];
    const columnDefs = [];
    const order = [0, "desc"];
    const rowCallback = function () { };

    dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, columnDefs, order, rowCallback);
}

$(function () {
    SearchHistoryDataTableInitial();
})