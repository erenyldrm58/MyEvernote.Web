var noteId = -1;
var modalCommentBodyId = '#modal_comment_body';

$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        noteId = btn.data('note-id');

        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteId);
    });
});

function doComment(button, process, commentId, spanId) {
    var btn = $(button);
    var mode = btn.data("edit-mode");

    if (process === "edit_clicked") {
        if (!mode)//Edit mode false
        {
            btn.data("edit-mode", true);
            btn.removeClass("btn-warning");
            btn.addClass("btn-success");

            btnSpan = btn.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanId).addClass("editable");
            $(spanId).attr("contenteditable", true);
            $(spanId).focus();
        }
        else {
            btn.data("edit-mode", false);
            btn.addClass("btn-warning");
            btn.removeClass("btn-success");

            btnSpan = btn.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanId).removeClass("editable");
            $(spanId).attr("contenteditable", false);

            var txt = $(spanId).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentId,
                data: { text: txt }
            }).done(function myfunction(data) {
                if (data.result) {
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteId);
                }
                else {
                    alert("Yorum güncellenemedi.");
                }
            }).fail(function myfunction(data) {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }
    }
    else if (process === "delete_clicked") {
        var dialogResult = confirm("Yorum silinsin mi?");

        if (!dialogResult)
            return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentId,
        }).done(function myfunction(data) {
            if (data.result) {
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteId);
            }
            else {
                alert("Yorum silinemedi.");
            }
        }).fail(function myfunction(data) {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
    else if (process === "new_clicked") {

        var txt = $("#txtNewComment").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create",
            data: { text: txt, "noteId": noteId }
        }).done(function myfunction(data) {
            if (data.result) {
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteId);
            }
            else {
                alert("Yorum eklenemedi.");
            }
        }).fail(function myfunction(data) {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
}