$(function () {
    $('#modal_not_detail').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        noteId = btn.data('note-id');

        $('#modal_not_detail_body').load("/Note/GetNoteText/" + noteId);
    });
});