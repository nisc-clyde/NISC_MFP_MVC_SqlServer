function FormSelect() {
    $("#searchDepartment_StatueSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

$(function () {
    $(document).ready(FormSelect);
});