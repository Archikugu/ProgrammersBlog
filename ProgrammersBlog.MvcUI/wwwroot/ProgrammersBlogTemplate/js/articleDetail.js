$(document).ready(function () {
    // Formun submit olmasını tamamen engelle
    $(document).on('submit', '#form-comment-add', function (event) {
        event.preventDefault();
    });

    // Yorum ekleme butonuna tıklandığında
    $(document).on('click', '#btnSave', function (event) {
        event.preventDefault();
        const form = $('#form-comment-add');
        const actionUrl = form.attr('action');
        const dataToSend = form.serialize();
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: dataToSend,
            dataType: "json",
            headers: {
                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                // Partial'ı güncelle
                const newFormBody = $(data.commentAddPartial).find('.form-card');
                const cardBody = $('.form-card');
                cardBody.replaceWith(newFormBody);

                // Validasyon kontrolü
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    // Yeni yorumu ekle
                    const newSingleComment = `
                        <div class="media mb-4">
                            <img class="d-flex mr-3 rounded-circle" src="https://randomuser.me/api/portraits/men/34.jpg" alt="">
                            <div class="media-body">
                                <h5 class="mt-0">${data.commentDto.comment.createdByName}</h5>
                                ${data.commentDto.comment.text}
                            </div>
                        </div>`;
                    const newSingleCommentObject = $(newSingleComment);
                    newSingleCommentObject.hide();
                    $('#comments').append(newSingleCommentObject);
                    newSingleCommentObject.fadeIn(3000);
                    toastr.success(
                        `Dear ${data.commentDto.comment.createdByName}, your comment has been successfully added. A sample will be shown to you, but your comment will only be visible after it is approved.`);
                    $('#btnSave').prop('disabled', true);
                    setTimeout(function () {
                        $('#btnSave').prop('disabled', false);
                    }, 15000);
                } else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            }
        });
    });
});