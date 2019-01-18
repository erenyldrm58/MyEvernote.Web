$(function () {
    var noteIds = [];

    $("div[data-note-id]").each(function myfunction(i, e) {
        noteIds.push($(e).data("note-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteIds }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find("span.like-star");

                btn.data("liked", true);
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }

        }
    }).fail(function () {

    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteIds }
    }).done(function (data) {

        if (data.result != null && data.result.legth > 0) {
            for (var i = 0; i < data.result.legth; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var spanStar = btn.find("span.like-star");

                btn.data("liked", true);
                spanStar.removeClass("glyphicon-star-empty");
                spanStar.addClass("glyphicon-star");
            }
        }
    }).fail(function (data) {

    });

    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var noteId = btn.data("note-id");
        var spanStar = btn.find("span.like-star");
        var spanCount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteId, "liked": !liked }
        })
            .done(function (data) {
                if (data.hasError) {
                    alert(data.errorMsg);
                }
                else {
                    liked = !liked;
                    btn.data("liked", liked);
                    spanCount.Text(data.likeCount);

                    spanStar.removeClass("glyphicon-star-empty");
                    spanStar.removeClass("glyphicon-star");

                    if (liked)
                        spanStar.addClass("glyphicon-star");
                    else
                        spanStar.addClass("glyphicon-star-empty");
                }
            })
            .fail(function (data) {
                alert("Sunucuyla bağlantı kurulamadı!");
            });
    });
});