$(document).ready(function () {

    $("#assignDeveloperForm").click(function () {
        $("#developer-form").toggle("slow");
    });

    $("#show-box").click(function () {
        $("#comment-box").toggle("slow");
    });

    $("#file-submission-form").click(function () {
        $("#file-form").toggle("slow");
    });
    $("#edit-button").click(function () {
        $("#comment-edit-box").toggle("slow");
    });
    $("#edit-comment-button").click(function () {
        $(".details-box").toggle();
        $(".edit-delete").toggle();
        $(".hide-edit").toggle();
    });
    $("#cancel-edit").click(function () {
        $(".details-box").toggle();
        $(".edit-delete").toggle();
        $(".hide-edit").toggle();
    });
    $("#file-remove").click(function () {
        $(".cross-button").toggle();
        $(this).html($(this).html() === ('<i class="fas fa-ban separate"></i>Cancel') ?
            ('<i class="fas fa-times separate"></i>Remove') :
            ('<i class="fas fa-ban separate"></i>Cancel'))
    });
});
