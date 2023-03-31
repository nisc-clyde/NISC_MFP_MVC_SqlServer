function FormSelect() {

    $("#searchCardReader_CardMachineTypeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCardReader_WorkModeSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCardReader_CardOnOffSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })

    $("#searchCardReader_CardStatusSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

$(function () {
    $(document).ready(FormSelect);
});