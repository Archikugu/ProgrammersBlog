$(document).ready(function () {
    initializeDeletedCommentsDataTable();
    initializeRefreshComments();
    initializeUndoDeleteComment();
    initializeHardDeleteComment();
});

function initializeDeletedCommentsDataTable() {
    window.dataTable = $('#deletedCommentsTable').DataTable({
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

function initializeRefreshComments() {
    $('#btnRefresh').click(function () {
        refreshComments();
    });
}

function refreshComments() {
    $.ajax({
        type: 'GET',
        url: '/Admin/Comment/GetAllDeletedComments/',
        contentType: "application/json",
        beforeSend: function () {
            $('#deletedCommentsTable').hide();
            $('.spinner-border').show();
        },
        success: function (data) {
            const commentResult = jQuery.parseJSON(data);
            if (commentResult.ResultStatus === 0) {
                updateCommentTable(commentResult);
            } else {
                $('.spinner-border').hide();
                $('#deletedCommentsTable').fadeIn(1000);
                toastr.error(`${commentResult.Message}`, 'Operation Failed!');
            }
        },
        error: function (err) {
            console.log(err);
            toastr.error(`${err.responseText}`, 'Error!');
        }
    });
}

function updateCommentTable(commentResult) {
    dataTable.clear();
    
    if (commentResult.Data) {
        const articlesArray = [];
        $.each(commentResult.Data.Comments.$values, function (index, comment) {
            const newComment = getJsonNetObject(comment, commentResult.Data.Comments.$values);
            let newArticle = getJsonNetObject(newComment.Article, newComment);
            
            if (newArticle !== null) {
                articlesArray.push(newArticle);
            }
            if (newArticle === null) {
                newArticle = articlesArray.find((article) => {
                    return article.$id === newComment.Article.$ref;
                });
            }
            
            const newTableRow = dataTable.row.add([
                newComment.Id,
                newArticle.Title,
                newComment.Text.length > 75 ? newComment.Text.substring(0, 75) + "..." : newComment.Text,
                `${newComment.IsActive ? "Yes" : "No"}`,
                `${newComment.IsDeleted ? "Yes" : "No"}`,
                `${convertToShortDate(newComment.CreatedDate)}`,
                newComment.CreatedByName,
                `${convertToShortDate(newComment.ModifiedDate)}`,
                newComment.ModifiedByName,
                getButtonsForDataTable(newComment)
            ]).node();

            const jqueryTableRow = $(newTableRow);
            jqueryTableRow.attr('name', `${newComment.Id}`);
        });
        
        dataTable.draw();
        $('.spinner-border').hide();
        $('#deletedCommentsTable').fadeIn(1400);
    } else {
        toastr.error(`${commentResult.Message}`, 'Operation Failed!');
    }
}

function getButtonsForDataTable(comment) {
    return `
        <div class="form-button-action">
            <button class="btn btn-link btn-warning btn-undo" data-id="${comment.Id}" title="Undo Delete">
                <i class="fas fa-undo"></i>
            </button>
            <button class="btn btn-link btn-danger btn-delete" data-id="${comment.Id}" title="Hard Delete">
                <i class="fas fa-trash"></i>
            </button>
        </div>
    `;
}

function initializeUndoDeleteComment() {
    $(document).on('click', '.btn-undo', function (e) {
        e.preventDefault();
        e.stopPropagation();
        const id = $(this).data('id');
        const tableRow = $(`tr[name='${id}']`);
        let commentText = tableRow.find('td:eq(2)').text().trim();
        commentText = commentText.length > 75 ? commentText.substring(0, 75) + '...' : commentText;

        Swal.fire({
            title: 'Are you sure you want to restore?',
            text: `The comment "${commentText}" will be restored from archive!`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, restore it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { commentId: id },
                    url: '/Admin/Comment/UndoDelete/',
                    success: function (data) {
                        const commentResult = jQuery.parseJSON(data);
                        if (commentResult.ResultStatus === 0) {
                            Swal.fire('Restored!', commentResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                            refreshComments();
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

function initializeHardDeleteComment() {
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        e.stopPropagation();
        const id = $(this).data('id');
        const tableRow = $(`tr[name='${id}']`);
        let commentText = tableRow.find('td:eq(2)').text().trim();
        commentText = commentText.length > 75 ? commentText.substring(0, 75) + '...' : commentText;

        Swal.fire({
            title: 'Are you sure you want to delete permanently?',
            text: `The comment "${commentText}" will be permanently deleted!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete permanently!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { commentId: id },
                    url: '/Admin/Comment/HardDelete/',
                    success: function (data) {
                        const commentResult = jQuery.parseJSON(data);
                        if (commentResult.ResultStatus === 0) {
                            Swal.fire('Deleted!', commentResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                            refreshComments();
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

function convertToShortDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}