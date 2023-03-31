function FormSelect() {
    $("#searchUser_ColorPermissionSelect").change(function () {
        console.log($(this).find("option:selected").val());
    })
}

$(function () {
    $(document).ready(FormSelect);
});