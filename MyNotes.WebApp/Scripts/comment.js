function addComment() {
    $.post("/Comment/Create", $("#commentForm").serialize(), function (res) {

        if (res.success) {

            $("#commentModalBody").load(
                "/Comment/List/" + res.noteId);

        } else {
            alert(res.message);
        }

    }).fail(function () {
        alert("Sunucu hatası oluştu.");
    });

}

function deleteComment(id) {
    if (confirm("Yorumu silmek istiyor musunuz?")) {
        $.post("/Comment/Delete/" + id, function (res) {

            if (res.success) {

                $("#commentModalBody").load(
                    "/Comment/List/" + res.noteId);
            }
            else {
                alert("Yorun Silinemedi");
            }

        }).fail(function () {
            alert("Sunucu hatası oluştu.");
        });

    }
}

function editComment(commentId) {

    let textElement = document.getElementById("comment-text-" + commentId);
    let button = document.getElementById("edit-btn-" + commentId);
    let isEditing = textElement.getAttribute("data-editing") === "true";

    if (!isEditing) {
        // EDIT MODE
        textElement.setAttribute("contenteditable", "true");
        textElement.setAttribute("data-editing", "true");
        textElement.focus();

        // cursor sona gitsin
        document.execCommand('selectAll', false, null);
        document.getSelection().collapseToEnd();

        // ikon ✔️ olsun
        button.innerHTML = '<i class="bi bi-check-lg"></i>';
    }
    else {
        // SAVE MODE
        let newText = textElement.innerText.trim();

        if (newText.length === 0) {
            alert("Yorum boş olamaz.");
            return;
        }

        $.post("/Comment/Edit", {
            id: commentId,
            text: newText
        }, function (response) {

            if (response.success) {
                // readonly mode
                textElement.setAttribute("contenteditable", "false");
                textElement.setAttribute("data-editing", "false");

                // ikon tekrar ✏️
                button.innerHTML = '<i class="bi bi-pencil"></i>';
            }

            else {
                // başarısız durum
                alert(response.message || "Yorum güncellenemedi.");

                // edit modunda kal
                textElement.focus();
            }
        }).fail(function () {
            // server error (500, 404 vs.)
            alert("Sunucu hatası oluştu. Lütfen tekrar deneyin.");
        });
    }
}

function openCommentsModal(noteId) {
    $("#commentModalBody").load(
        "/Comment/List/" + noteId,
        function () {
            var modal = new bootstrap.Modal(document.getElementById('commentModal'));

            modal.show();
        }
    );
}