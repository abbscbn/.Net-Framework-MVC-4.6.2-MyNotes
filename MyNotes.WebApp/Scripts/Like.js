
$(function () {

    var notIds = [];

    $("div[data-note-id]").each(function (i, e) {
        notIds.push($(e).data("note-id"));
    });

    

    $.ajax({
        method: "POST",
        url: "/Like/GetLiked",
        data: { ids: notIds }
    }).done(function (data) {


        console.log(data);

        if (!data.success) {
            //window.location.href = data.redirect;
            return;
        }


        if (data.result != null && data.result.length > 0) {

            for (var i = 0; i < data.result.length; i++) {

                var noteId = data.result[i];

                console.log(noteId);

                let icon = document.querySelector(`#like-icon-${noteId}`);

                console.log(icon);

                if (icon) {
                    icon.classList.remove("bi-heart");
                    icon.classList.add("bi-heart-fill");

                }
            }

        }



    });

});




function toggleLike(noteId, button) {

    $.post("/Like/Toggle", { noteId: noteId }, function (response) {

        if (!response.success) {
            alert(response.message || "İşlem başarısız");
            return;
        }

        let icon = button.querySelector("i");
        let countSpan = document.getElementById("like-count-" + noteId);

        if (response.liked) {
            // like atıldı
            icon.classList.remove("bi-heart");
            icon.classList.add("bi-heart-fill");
        } else {
            // like geri alındı
            icon.classList.remove("bi-heart-fill");
            icon.classList.add("bi-heart");
        }

        countSpan.innerText = response.likeCount;

    }).fail(function () {
        alert("Sunucu hatası");
    });
}