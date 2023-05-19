import { CustomSweetAlert2, RequestAddOrEdit, RequestDelete, DataTableTemplate } from "./Shared.js";

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
function SearchCardReaderDataTableInitial() {
  const table = $("#searchCardReaderDataTable");
  const url = "/Admin/CardReader/InitialDataTable";
  const page = "cardreader";
  const columns = [
    { data: "cr_id", name: "卡機編號" },
    { data: "cr_ip", name: "IP位置" },
    { data: "cr_port", name: "PORT" },
    { data: "cr_type", name: "卡機種類" },
    { data: "cr_mode", name: "運作模式" },
    { data: "cr_card_switch", name: "卡號判斷開關" },
    { data: "cr_status", name: "狀態" },
    {
      data: null,
      render: function (data, type, row) {
        return (
          "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-1 row-cols-xxl-3 g-2'>" +
          "<div class='col' > <button type='button' class='btn btn-primary btn-sm btn-management' data-id='" +
          data.serial +
          "'><i class='fa-solid fa-circle-info me-1'></i><div style='display: inline-block; white-space: nowrap;'>管理</div></button></div > " +
          "<div class='col'><button type='button' class='btn btn-info btn-sm  btn-edit' data-id='" +
          data.serial +
          "'><i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
          "<div class='col'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='" +
          data.serial +
          "'><i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div></div>"
        );
      },
      orderable: false,
    },
    { data: "serial", name: "serial" },
  ];
  const columnDefs = [
    { width: "15%", targets: 7 },
    { visible: false, target: 8 },
  ];
  const order = [0, "desc"];
  const rowCallback = function (row, data) {
    data.cr_mode == "連線"
      ? $("td:eq(4)", row).html("<b class='text-success'>連線</b>")
      : $("td:eq(4)", row).html("<b class='text-danger'>離線</b>");
    data.cr_card_switch == "開啟"
      ? $("td:eq(5)", row).html("<b class='text-success'>開啟</b>")
      : $("td:eq(5)", row).html("<b class='text-danger'>關閉</b>");
    data.cr_status == "線上"
      ? $("td:eq(6)", row).html("<b class='text-success'>線上</b>")
      : $("td:eq(6)", row).html("<b class='text-danger'>離線</b>");
  };

  dataTable = DataTableTemplate.DataTableInitial(table, url, page, columns, columnDefs, order, rowCallback);
}

function ColumnSearch() {
  $("#searchCardReader_CardreaderID").keyup(function () {
    dataTable.columns(0).search($("#searchCardReader_CardreaderID").val()).draw();
  });

  $("#searchCardReader_IPAddress").keyup(function () {
    dataTable.columns(1).search($("#searchCardReader_IPAddress").val()).draw();
  });

  $("#searchCardReader_Port").keyup(function () {
    dataTable.columns(2).search($("#searchCardReader_Port").val()).draw();
  });

  $("#searchCardReader_CardMachineTypeSelect").change(function () {
    if ($("#searchCardReader_CardMachineTypeSelect").val() != "") {
      dataTable.columns(3).search($("#searchCardReader_CardMachineTypeSelect :selected").text()).draw();
    } else {
      dataTable.columns(3).search("").draw();
    }
  });

  $("#searchCardReader_WorkModeSelect").change(function () {
    if ($("#searchCardReader_WorkModeSelect").val() != "") {
      dataTable.columns(4).search($("#searchCardReader_WorkModeSelect :selected").text()).draw();
    } else {
      dataTable.columns(4).search("").draw();
    }
  });

  $("#searchCardReader_CardOnOffSelect").change(function () {
    if ($("#searchCardReader_CardOnOffSelect").val() != "") {
      dataTable.columns(5).search($("#searchCardReader_CardOnOffSelect :selected").text()).draw();
    } else {
      dataTable.columns(5).search("").draw();
    }
  });

  $("#searchCardReader_CardStatusSelect").change(function () {
    if ($("#searchCardReader_CardStatusSelect").val() != "") {
      dataTable.columns(6).search($("#searchCardReader_CardStatusSelect :selected").text()).draw();
    } else {
      dataTable.columns(6).search("").draw();
    }
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
  const btnAdd = "btnAddCardReader";
  const modalForm = "cardReaderForm";
  const title = "事務機";
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
  const url = "/Admin/CardReader/Delete";
  RequestDelete.GetAndPostDeleteTemplate(dataTable, uniqueIdProperty, url);
}

function CardReaderManagement() {
  dataTable.on("click", ".btn-management", function (e) {
    e.preventDefault();
    const getViewUrl = "/Admin/CardReader/CardReaderManagement";
    const modalForm = "cardReaderForm";
    let currentRow;
    if ($(this).parents("tr").prev().hasClass("dt-hasChild")) {
      //Is Responsiveness
      currentRow = $(this).parents("tr").prev();
    } else {
      //Not Responsiveness
      currentRow = $(this).closest("tr");
    }
    const rowData = dataTable.row(currentRow).data();

    const cardReaderSerial = $(this).data("id");
    const cardReaderId = rowData["cr_id"];
    const addOrEditUrl = "/Admin/CardReader/AddOrEditMFP";
    const deleteUrl = "/Admin/CardReader/DeleteMFP";

    $.get(
      getViewUrl,
      {
        formTitle: "事務機" + $(this).text(),
        serial: cardReaderSerial,
        cardReaderId: cardReaderId,
      },
      function (data) {
        $("#" + modalForm).html(data);
        $("#" + modalForm).modal("show");

        CardReaderManagementAdd(addOrEditUrl, cardReaderId);
        CardReaderManagementEdit(addOrEditUrl, cardReaderId);
        CardReaderManagementDelete(deleteUrl);
        CardReaderManagementCancelSwitch();
        CardReaderManagementEditSwitch();
      }
    );
  });
}

function CardReaderManagementAdd(url, cardReaderId) {
  $("#cardreaderManagermentForm").on("submit", function () {
    if ($(".btnCardReaderManagement").attr("id") == "btnCardReaderManagementAdd") {
      $.validator.unobtrusive.parse(this);
      if ($(this).valid()) {
        $.ajax({
          type: "POST",
          url: url,
          data: $(this).serialize() + "&cr_id=" + cardReaderId + "&currentOperation=" + "Add",
          success: function (data) {
            if (data.success) {
              $("#cardreaderManagermentForm").load("/Admin/CardReader/CardReaderManager", {
                cardReaderId: cardReaderId,
              });
            }
          },
        });
      }
    }
    return false;
  });
}

function CardReaderManagementEdit(url, cardReaderId) {
  $("#cardreaderManagermentForm").on("submit", function (e) {
    e.preventDefault();
    if ($(".btnCardReaderManagement").attr("id") == "btnCardReaderManagementEdit") {
      let deleteRowDataSerialize = $(this).serialize();
      deleteRowDataSerialize = deleteRowDataSerialize.replace("multiFunctionPrintModel.", "");

      $.validator.unobtrusive.parse(this);
      if ($(this).valid()) {
        $.ajax({
          type: "POST",
          url: url,
          data:
            deleteRowDataSerialize +
            "&cr_id=" +
            cardReaderId +
            "&currentOperation=" +
            $("#btnCardReaderManagementEdit").val(),
          success: function (data) {
            if (data.success) {
              $("#cardreaderManagermentForm").load("/Admin/CardReader/CardReaderManager", {
                cardReaderId: cardReaderId,
              });
            }
          },
        });
      }
    }
    return false;
  });
}

function CardReaderManagementDelete(url) {
  $("body").on("click", "#cardreaderManagermentTable #btnCardReaderManagementRowDelete", function (e) {
    e.preventDefault();
    const sweetAlertHome = CustomSweetAlert2.SweetAlertTemplateHome();
    const sweetAlertSuccess = CustomSweetAlert2.SweetAlertTemplateSuccess();

    let deleteRow = $(this).closest("tr");
    var deleteRowDataSerialize = "serial=" + $(this).closest("tr").find("input").val();
    let $row = $(this).closest("tr"),
      $tds = $row.find("td");
    $.each($tds, function () {
      if ($(this).attr("id") != undefined) {
        deleteRowDataSerialize += "&" + $(this).attr("id") + "=" + $(this).text();
      }
    });
    deleteRowDataSerialize = deleteRowDataSerialize.replace(/線上/g, "Online");
    deleteRowDataSerialize = deleteRowDataSerialize.replace(/離線/g, "Offline");
    deleteRowDataSerialize = deleteRowDataSerialize.replace(/C(彩色)/g, "C");
    deleteRowDataSerialize = deleteRowDataSerialize.replace(/M(單色)/g, "M");
    deleteRowDataSerialize = deleteRowDataSerialize.replace(/未知/g, "");

    sweetAlertHome
      .fire({
        allowEnterKey: false,
        keydownListenerCapture: true,
      })
      .then((result) => {
        $(".btn-management").focus();
        if (result.isConfirmed) {
          $.ajax({
            type: "POST",
            url: url,
            data: deleteRowDataSerialize,
            success: function (data) {
              if (data.success) {
                deleteRow.remove();
                sweetAlertSuccess.fire();
              }
            },
          });
        }
      });
  });
}

function CardReaderManagementEditSwitch() {
  $("body").on("click", "#cardreaderManagermentTable #btnCardReaderManagementRowEdit", function () {
    let mfpSerial = $(this).parent().siblings("input").val();
    let rowData = [];
    let $row = $(this).closest("tr"),
      $tds = $row.find("td");
    $.each($tds, function () {
      rowData.push($(this).text());
    });

    $("#cardreaderManagermentTable tbody tr:first #serial").val(mfpSerial);
    $("#cardreaderManagermentTable tbody tr:first #printer_id").val(rowData[0]);
    $("#cardreaderManagermentTable tbody tr:first #mfp_ip").val(rowData[1]);
    $("#cardreaderManagermentTable tbody tr:first #mfp_name").val(rowData[2]);
    $("#cardreaderManagermentTable tbody tr:first #mfp_color").val(rowData[3].substring(0, 1));
    $("#cardreaderManagermentTable tbody tr:first #driver_number").val(rowData[4]);
    $("#cardreaderManagermentTable tbody tr:first #mfp_status").val(rowData[5] == "線上" ? "Online" : "Offline");

    $("#cardreaderManagermentTable #btnCardReaderManagementAdd").text("修改");
    $("#cardreaderManagermentTable #btnCardReaderManagementAdd").val("Edit");
    $("#cardreaderManagermentTable #btnCardReaderManagementAdd").attr("id", "btnCardReaderManagementEdit");
    $("#cardreaderManagermentTable #btnCardReaderManagementCancel").show();
  });
}

function CardReaderManagementCancelSwitch() {
  $("body").on("click", "#cardreaderManagermentTable #btnCardReaderManagementCancel", function () {
    $("#cardreaderManagermentTable tbody tr:first #serial").val("");
    $("#cardreaderManagermentTable tbody tr:first #printer_id").val("");
    $("#cardreaderManagermentTable tbody tr:first #mfp_ip").val("");
    $("#cardreaderManagermentTable tbody tr:first #mfp_name").val("");
    $("#cardreaderManagermentTable tbody tr:first #mfp_color").val("C");
    $("#cardreaderManagermentTable tbody tr:first #driver_number").val("");
    $("#cardreaderManagermentTable tbody tr:first #mfp_status").val("Online");

    $("#cardreaderManagermentTable #btnCardReaderManagementEdit").text("新增");
    $("#cardreaderManagermentTable #btnCardReaderManagementEdit").val("Add");
    $("#cardreaderManagermentTable #btnCardReaderManagementEdit").attr("id", "btnCardReaderManagementAdd");
    $("#cardreaderManagermentTable #btnCardReaderManagementCancel").hide();
  });
}

$(function () {
  $.fn.modal.Constructor.prototype._enforceFocus = function () {};
  SearchCardReaderDataTableInitial();
  ColumnSearch();
  PopupFormForAddOrEdit();
  DeleteAlertPopUp();
  CardReaderManagement();
});
