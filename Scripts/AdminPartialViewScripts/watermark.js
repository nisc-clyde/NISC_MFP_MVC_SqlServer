function FormSelect() {
    $("#searchWatermark_TypeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchWatermark_PositionSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchWatermark_FillTypeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

$(function () {
    $(document).ready(FormSelect);
});