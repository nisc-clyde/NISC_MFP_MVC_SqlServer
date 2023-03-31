function FormSelect() {
    $("#searchCard_AttributeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCard_StatusSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

$(function () {
    $(document).ready(FormSelect);
});