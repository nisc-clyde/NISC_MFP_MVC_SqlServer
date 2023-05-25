import {
  CustomSweetAlert2,
  RequestAddOrEdit,
  RequestDelete,
  DataTableTemplate,
} from "./Shared.js";

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
function SearchDepartmentDataTableInitial() {
  const table = $("#searchDepartmentDataTable");
  const url = "/Admin/Department/InitialDataTable";
  const page = "department";
  const columns = [
    { data: "dept_id", name: "部門編號" },
    { data: "dept_name", name: "部門名稱" },
    { data: "dept_value", name: "可用點數上限" },
    { data: "dept_month_sum", name: "可用遞增餘額" },
    { data: "dept_usable", name: "狀態" },
    { data: "dept_email", name: "部門管理者email" },
    {
      data: null,
      render: function (data, type, row) {
        return (
          "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-2 row-cols-xxl-2 g-2'><div class='col'><button type='button' class='btn btn-info btn-sm btn-edit' data-id='" +
          data.serial +
          "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
          "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-sm btn-delete' data-id='" +
          data.serial +
          "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>"
        );
      },
      orderable: false,
    },
    { data: "serial", name: "serial" },
  ];
  const columnDefs = [
    { width: "10%", target: 6 },
    { visible: false, target: 7 },
  ];
  const order = [0, "desc"];
  const rowCallback = function (row, data) {
    data.dept_usable == "停用"
      ? $("td:eq(4)", row).html("<b class='text-danger'>停用</b>")
      : $("td:eq(4)", row).html("<b class='text-success'>啟用</b>");
  };

  dataTable = DataTableTemplate.DataTableInitial(
    table,
    url,
    page,
    columns,
    columnDefs,
    order,
    rowCallback
  );
}

/**
 * 輸入欲搜尋之欄位資料並Refresh DataTable
 */
function ColumnSearch() {
  $("#searchDepartment_DepartmentID").keyup(function () {
    dataTable
      .columns(0)
      .search($("#searchDepartment_DepartmentID").val())
      .draw();
  });

  $("#searchDepartment_DepartmentName").keyup(function () {
    dataTable
      .columns(1)
      .search($("#searchDepartment_DepartmentName").val())
      .draw();
  });

  $("#searchDepartment_AvailablePointLimit").keyup(function () {
    dataTable
      .columns(2)
      .search($("#searchDepartment_AvailablePointLimit").val())
      .draw();
  });

  $("#searchDepartment_AvailableBalance").keyup(function () {
    dataTable
      .columns(3)
      .search($("#searchDepartment_AvailableBalance").val())
      .draw();
  });

  $("#searchDepartment_StatueSelect").change(function () {
    if ($("#searchDepartment_StatueSelect").val() != "") {
      dataTable
        .columns(4)
        .search($("#searchDepartment_StatueSelect :selected").text())
        .draw();
    } else {
      dataTable.columns(4).search("").draw();
    }
  });

  $("#searchDepartment_Mail").keyup(function () {
    dataTable.columns(5).search($("#searchDepartment_Mail").val()).draw();
  });
}

/**
 * Popup the modal from bootstrap library for adding or updating the data
 * @param {string} btnAdd - click which button topopup modal for adding.
 * @param {string} modalForm - modal's id.
 * @param {jQuery DataTable} dataTable - the data of container.
 * @param {string} uniqueIdProperty - identification each row data of property.
 */
function PopupFormForAddOrEdit() {
  const btnAdd = "btnAddDepartment";
  const modalForm = "departmentForm";
  const title = "部門";
  RequestAddOrEdit.GetAddOrEditTemplate(btnAdd, modalForm, dataTable, title);
}

/**
 * Popup the modal from bootstrap library for deleting the data
 * @param {jQuery DataTable} dataTable - The data of container.
 * @param {string} uniqueIdProperty - Identification each row data of property.
 * @param {string} getURL - Send request of get destination
 * @param {string} postURL - Send request of post destination
 */
function DeleteAlertPopUp() {
  const uniqueIdProperty = "serial";
  const url = "/Admin/Department/Delete";
  RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, url);
}

/**
 * Auto execution when document ready
 */
$(function () {
  SearchDepartmentDataTableInitial();
  ColumnSearch();
  PopupFormForAddOrEdit();
  DeleteAlertPopUp();
});
