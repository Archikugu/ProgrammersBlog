$(document).ready(function () {
    initializeCommentsDataTable();
    initializeRefreshComments();
    initializeDeleteComment();
    initializeUpdateCommentModal();
    initializeUpdateCommentAjax();
    initializeDetailCommentModal();
    initializeApproveComment();
});

// 1. Initialize DataTable
function initializeCommentsDataTable() {
    window.dataTable = $('#commentsTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                extend: 'copy',
                text: '<i class="fas fa-copy"></i> Copy',
                className: 'btn btn-secondary',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel"></i> Excel',
                className: 'btn btn-success',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdf',
                text: '<i class="fas fa-file-pdf"></i> PDF',
                className: 'btn btn-danger',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                className: 'btn btn-warning',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        responsive: true,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });
}

// Yorumlar Yenile Butonuna Tıklama
function initializeRefreshComments() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Comment/GetAllComments/',
            contentType: "application/json",
            beforeSend: function () {
                $('#commentsTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const commentListDto = jQuery.parseJSON(data);
                if (commentListDto.ResultStatus === 0) {
                    updateCommentTable(commentListDto);
                } else {
                    $('.spinner-border').hide();
                    $('#commentsTable').fadeIn(1000);
                    toastr.error(`${commentListDto.Message}`, 'Operation Failed!');
                }
            },
            error: function (err) {
                console.log(err);
                toastr.error(`${err.responseText}`, 'Error!');
            }
        });
    });
}

// Yorumları Tabloya Güncelleme
function updateCommentTable(commentListDto) {
    dataTable.clear().draw();

    // Check data structure
    if (!commentListDto || !commentListDto.Data || !commentListDto.Data.Comments) {
        console.error('Invalid data structure:', commentListDto);
        toastr.error('An error occurred while loading comments.', 'Error!');
        $('.spinner-border').hide();
        $('#commentsTable').fadeIn(1000);
        return;
    }

    // Get comments array
    const comments = commentListDto.Data.Comments.$values || [];

    if (comments.length === 0) {
        toastr.info('No comments found to display.', 'Info');
    }

    $.each(comments, function (index, comment) {
        const newRow = dataTable.row.add([
            comment.Id,
            comment.Article.Title,
            comment.Text.length > 75 ? comment.Text.substring(0, 75) + '...' : comment.Text,
            comment.IsActive ? "Yes" : "No",
            comment.IsDeleted ? "Yes" : "No",
            convertToShortDate(comment.CreatedDate),
            comment.CreatedByName,
            convertToShortDate(comment.ModifiedDate),
            comment.ModifiedByName,
            getButtonsForDataTable(comment)
        ]).draw(false).node();

        $(newRow).attr('name', comment.Id);
    });

    $('.spinner-border').hide();
    $('#commentsTable').fadeIn(1400);
}

// 2. Delete Comment
function initializeDeleteComment() {
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const tableRow = $(`tr[name='${id}']`);
        let commentText = tableRow.find('td:eq(2)').text().trim();
        commentText = commentText.length > 75 ? commentText.substring(0, 75) + '...' : commentText;

        Swal.fire({
            title: 'Are you sure you want to delete?',
            text: `The comment "${commentText}" will be deleted!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { commentId: id },
                    url: '/Admin/Comment/Delete/',
                    success: function (data) {
                        const commentResult = jQuery.parseJSON(data);
                        if (commentResult.Data) {
                            Swal.fire('Deleted!', commentResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                        } else {
                            Swal.fire('Failed!', commentResult.Message, 'error');
                        }
                    },
                    error: function (err) {
                        console.error(err);
                        toastr.error(err.responseText, 'Error!');
                    }
                });
            }
        });
    });
}

// 3. Load Update Modal
function initializeUpdateCommentModal() {
    const url = '/Admin/Comment/Update';
    const placeHolderDiv = $('#modalPlaceHolder');

    $(document).on('click', '.btn-update', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        console.log('Update modal opening for comment:', id);

        $.get(url, { commentId: id })
            .done(function (data) {
                console.log('Modal content loaded:', data);
                placeHolderDiv.html(data);
                const modal = placeHolderDiv.find('.modal');
                modal.modal('show');
                console.log('Modal shown');
            })
            .fail(function (err) {
                console.error('Error loading modal:', err);
                toastr.error(err.responseText, 'Error!');
            });
    });
}

// 4. Submit Update via AJAX
function initializeUpdateCommentAjax() {
    const placeHolderDiv = $('#modalPlaceHolder');

    placeHolderDiv.on('click', '#btnUpdate', function (e) {
        e.preventDefault();
        console.log('Update button clicked');

        const form = $('#form-comment-update');
        const actionUrl = form.attr('action');
        const dataToSend = new FormData(form.get(0));

        console.log('Form data:', Object.fromEntries(dataToSend));
        console.log('Action URL:', actionUrl);

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: dataToSend,
            processData: false,
            contentType: false,
            success: function (data) {
                console.log('Update response:', data);
                const result = jQuery.parseJSON(data);
                console.log('Parsed result:', result);

                if (!result.CommentDto) {
                    console.error('Invalid response format:', result);
                    toastr.error('Invalid response from server', 'Error!');
                    return;
                }

                const commentDto = result.CommentDto;
                const isValid = $('.modal-body [name="IsValid"]').val() === 'True';
                console.log('IsValid:', isValid);

                if (isValid) {
                    const tableRow = $(`tr[name='${commentDto.Comment.Id}']`);
                    console.log('Updating table row:', tableRow);

                    dataTable.row(tableRow).data([
                        commentDto.Comment.Id,
                        commentDto.Comment.Article.Title,
                        commentDto.Comment.Text.length > 75 ? commentDto.Comment.Text.substring(0, 75) + '...' : commentDto.Comment.Text,
                        commentDto.Comment.IsActive ? "Yes" : "No",
                        commentDto.Comment.IsDeleted ? "Yes" : "No",
                        convertToShortDate(commentDto.Comment.CreatedDate),
                        commentDto.Comment.CreatedByName,
                        convertToShortDate(commentDto.Comment.ModifiedDate),
                        commentDto.Comment.ModifiedByName,
                        getButtonsForDataTable(commentDto.Comment)
                    ]);

                    tableRow.attr('name', commentDto.Comment.Id);
                    dataTable.row(tableRow).invalidate();

                    // Modal içeriğini güncelle
                    if (result.CommentUpdatePartial) {
                        placeHolderDiv.find('.modal-body').html(result.CommentUpdatePartial);
                    }

                    const modal = placeHolderDiv.find('.modal');
                    modal.modal('hide');
                    console.log('Modal hidden');

                    toastr.success(`Comment #${commentDto.Comment.Id} was updated successfully.`, 'Success!');
                } else {
                    console.log('Validation failed');
                    // Validation hatalarını göster
                    if (result.CommentUpdatePartial) {
                        placeHolderDiv.find('.modal-body').html(result.CommentUpdatePartial);
                    }
                    showValidationErrors();
                }
            },
            error: function (err) {
                console.error('Update error:', err);
                toastr.error(err.responseText, 'Error!');
            }
        });
    });
}

// 5. Show Validation Errors
function showValidationErrors() {
    let summaryText = "";
    $('#validation-summary > ul > li').each(function () {
        summaryText += `* ${$(this).text()}\n`;
    });
    toastr.warning(summaryText, 'Validation Failed!');
}

// 6. Get Action Buttons HTML
function getButtonsForDataTable(comment) {
    const approveButton = `
        <button class="btn btn-link btn-success btn-approve" data-id="${comment.Id}" title="Approve">
            <i class="fas fa-thumbs-up"></i>
        </button>`;

    const detailButton = `
        <button class="btn btn-link btn-primary btn-detail" data-id="${comment.Id}" title="Details">
            <i class="fas fa-eye"></i>
        </button>`;

    const updateButton = `
        <button class="btn btn-link btn-warning btn-update mt-1" data-id="${comment.Id}" title="Update">
            <i class="fas fa-edit"></i>
        </button>`;

    const deleteButton = `
        <button class="btn btn-link btn-danger btn-delete mt-1" data-id="${comment.Id}" title="Delete">
            <i class="fas fa-trash"></i>
        </button>`;

    let buttonsHtml = '<div class="form-button-action">';
    if (!comment.IsActive) {
        buttonsHtml += approveButton;
    }
    buttonsHtml += detailButton + updateButton + deleteButton;
    buttonsHtml += '</div>';

    return buttonsHtml;
}

// Utility: Convert date string to short format
function convertToShortDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}


// 7. Load Detail Modal
function initializeDetailCommentModal() {
    const url = '/Admin/Comment/GetDetail';
    const placeHolderDiv = $('#modalPlaceHolder');

    $(document).on('click', '.btn-detail', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        console.log('Update modal opening for comment:', id);

        $.get(url, { commentId: id })
            .done(function (data) {
                console.log('Modal content loaded:', data);
                placeHolderDiv.html(data);
                const modal = placeHolderDiv.find('.modal');
                modal.modal('show');
                console.log('Modal shown');
            })
            .fail(function (err) {
                console.error('Error loading modal:', err);
                toastr.error(err.responseText, 'Error!');
            });
    });
}

function initializeApproveComment() {
    $(document).on('click', '.btn-approve', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const tableRow = $(`tr[name='${id}']`);
        let commentText = tableRow.find('td:eq(2)').text().trim();
        commentText = commentText.length > 75 ? commentText.substring(0, 75) + '...' : commentText;

        Swal.fire({
            title: 'Are you sure you want to approve?',
            text: `The comment "${commentText}" will be approve!`,
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, approve it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { commentId: id },
                    url: '/Admin/Comment/Approve/',
                    success: function (data) {
                        const commentResult = jQuery.parseJSON(data);
                        if (commentResult.Data) {

                            dataTable.row(tableRow).data([
                                commentResult.Data.Comment.Id,
                                commentResult.Data.Comment.Article.Title,
                                commentResult.Data.Comment.Text.length > 75 ? commentResult.Data.Comment.Text.substring(0, 75) + '...' : commentResult.Data.Comment.Text,
                                commentResult.Data.Comment.IsActive ? "Yes" : "No",
                                commentResult.Data.Comment.IsDeleted ? "Yes" : "No",
                                convertToShortDate(commentResult.Data.Comment.CreatedDate),
                                commentResult.Data.Comment.CreatedByName,
                                convertToShortDate(commentResult.Data.Comment.ModifiedDate),
                                commentResult.Data.Comment.ModifiedByName,
                                getButtonsForDataTable(commentResult.Data.Comment)
                            ]);

                            tableRow.attr('name', comment.Id);
                            dataTable.row(tableRow).invalidate();

                            Swal.fire('Aproved!', `Comment by $(commentResult.Data.Comment.Id) has been successfully approved.`, 'success');
                           
                        } else {
                            Swal.fire('Failed!', `Comment by $(commentResult.Data.Comment.Id) has been unsuccessfully approved.`, 'error');
                        }
                    },
                    error: function (err) {
                        console.error(err);
                        toastr.error(err.responseText, 'Error!');
                    }
                });
            }
        });
    });
}
