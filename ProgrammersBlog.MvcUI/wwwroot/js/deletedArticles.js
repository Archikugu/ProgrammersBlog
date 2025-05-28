$(document).ready(function () {
    initializeDataTable();
    initializeRefreshArticles();
    initializeUndoDeleteArticle();
    initializeHardDeleteArticle();
});

function initializeDataTable() {
    $('#deletedArticlesTable').DataTable({
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

function initializeRefreshArticles() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Article/GetAllDeletedArticles/',
            contentType: "application/json",
            beforeSend: function () {
                $('#deletedArticlesTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const articleListDto = jQuery.parseJSON(data);
                if (articleListDto.ResultStatus === 0) {
                    updateArticleTable(articleListDto);
                } else {
                    $('.spinner-border').hide();
                    $('#deletedArticlesTable').fadeIn(1000);
                    toastr.error(`${articleListDto.Message}`, 'İşlem Başarısız!');
                }
            },
            error: function (err) {
                console.log(err);
                toastr.error(`${err.responseText}`, 'Hata!');
            }
        });
    });
}

function updateArticleTable(articleListDto) {
    let tableBody = "";
    $.each(articleListDto.Data.Articles.$values, function (index, article) {
        const newArticle = getJsonNetObject(article, articleListDto.Data.Articles.$values);
        let newCategory = getJsonNetObject(newArticle.Category, newArticle);
        
        tableBody += `
        <tr name="${newArticle.Id}">
            <td class="text-center align-middle">${newArticle.Id}</td>
            <td class="text-center align-middle">${newCategory.Name}</td>
            <td class="text-center align-middle">${newArticle.Title}</td>
            <td class="text-center align-middle"><img src="/images/${newArticle.Thumbnail}" class="my-image-table" /></td>
            <td class="text-center align-middle" data-order="${newArticle.Date}">${convertToShortDate(newArticle.Date)}</td>
            <td class="text-center align-middle">${newArticle.ViewsCount}</td>
            <td class="text-center align-middle">${newArticle.CommentCount}</td>
            <td class="text-center align-middle">${convertFirstLetterToUpperCase(newArticle.IsActive.toString())}</td>
            <td class="text-center align-middle">${convertFirstLetterToUpperCase(newArticle.IsDeleted.toString())}</td>
            <td class="text-center align-middle">${convertToShortDate(newArticle.CreatedDate)}</td>
            <td class="text-center align-middle">${newArticle.CreatedByName}</td>
            <td class="text-center align-middle">${convertToShortDate(newArticle.ModifiedDate)}</td>
            <td class="text-center align-middle">${newArticle.ModifiedByName}</td>
            <td class="text-center align-middle">
                <div class="form-button-action">
                    <button class="btn btn-link btn-warning btn-undo" data-id="${newArticle.Id}">
                        <span class="fas fa-undo"></span>
                    </button>
                    <button class="btn btn-link btn-danger btn-delete" data-id="${newArticle.Id}">
                        <span class="fas fa-trash"></span>
                    </button>
                </div>
            </td>
        </tr>`;
    });
    $('#deletedArticlesTable > tbody').html(tableBody);
    $('.spinner-border').hide();
    $('#deletedArticlesTable').fadeIn(1400);
}

function initializeUndoDeleteArticle() {
    $(document).on('click', '.btn-undo', function (e) {
        e.preventDefault();
        e.stopPropagation();
        const id = $(this).attr('data-id');
        const tableRow = $(`tr[name='${id}']`);
        let articleTitle = tableRow.find('td:eq(2)').text();
        const dataTable = $('#deletedArticlesTable').DataTable();

        Swal.fire({
            title: 'Geri almak istediğinize emin misiniz?',
            text: `"${articleTitle}" makalesi arşivden geri alınacak!`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, geri al!',
            cancelButtonText: 'İptal'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { articleId: id },
                    url: '/Admin/Article/UndoDelete/',
                    success: function (data) {
                        const undoDeletedArticleResult = jQuery.parseJSON(data);
                        if (undoDeletedArticleResult.ResultStatus === 0) {
                            Swal.fire('Geri Alındı!', undoDeletedArticleResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                            
                            $.ajax({
                                type: 'GET',
                                url: '/Admin/Article/GetAllDeletedArticles/',
                                contentType: "application/json",
                                success: function (data) {
                                    const articleListDto = jQuery.parseJSON(data);
                                    if (articleListDto.ResultStatus === 0) {
                                        updateArticleTable(articleListDto);
                                    } else {
                                        toastr.error(`${articleListDto.Message}`, 'İşlem Başarısız!');
                                    }
                                },
                                error: function (err) {
                                    console.log(err);
                                    toastr.error(`${err.responseText}`, 'Hata!');
                                }
                            });
                        } else {
                            Swal.fire('Başarısız!', undoDeletedArticleResult.Message, 'error');
                        }
                    },
                    error: function (err) {
                        console.error(err);
                        toastr.error(err.responseText, 'Hata!');
                    }
                });
            }
        });
    });
}

function initializeHardDeleteArticle() {
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        e.stopPropagation();
        const id = $(this).attr('data-id');
        const tableRow = $(`tr[name='${id}']`);
        let articleTitle = tableRow.find('td:eq(2)').text();
        const dataTable = $('#deletedArticlesTable').DataTable();

        Swal.fire({
            title: 'Kalıcı olarak silmek istediğinize emin misiniz?',
            text: `"${articleTitle}" makalesi veritabanından kalıcı olarak silinecek!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, kalıcı olarak sil!',
            cancelButtonText: 'İptal'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { articleId: id },
                    url: '/Admin/Article/HardDelete/',
                    success: function (data) {
                        const hardDeleteResult = jQuery.parseJSON(data);
                        if (hardDeleteResult.ResultStatus === 0) {
                            Swal.fire('Silindi!', hardDeleteResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                            
                            $.ajax({
                                type: 'GET',
                                url: '/Admin/Article/GetAllDeletedArticles/',
                                contentType: "application/json",
                                success: function (data) {
                                    const articleListDto = jQuery.parseJSON(data);
                                    if (articleListDto.ResultStatus === 0) {
                                        updateArticleTable(articleListDto);
                                    } else {
                                        toastr.error(`${articleListDto.Message}`, 'İşlem Başarısız!');
                                    }
                                },
                                error: function (err) {
                                    console.log(err);
                                    toastr.error(`${err.responseText}`, 'Hata!');
                                }
                            });
                        } else {
                            Swal.fire('Başarısız!', hardDeleteResult.Message, 'error');
                        }
                    },
                    error: function (err) {
                        console.error(err);
                        toastr.error(err.responseText, 'Hata!');
                    }
                });
            }
        });
    });
}
