$(document).ready(function () {

    // DataTable Başlatılıyor
    initializeDataTable();

    // Add Article Sayfasına Yönlendirme
    $('#btnAdd').click(function () {
        window.location.href = '/Admin/Article/Add';
    });

    // Yenile Butonuna Tıklama
    initializeRefreshArticle();

    // Silme İşlemi (SweetAlert ile)
    initializeDeleteArticle();

});

// DataTable Başlatma
function initializeDataTable() {
    dataTable = $('#articlesTable').DataTable({
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


// Hata Mesajlarını Gösterme
function showValidationErrors() {
    let summaryText = "";
    $('#validation-summary > ul > li').each(function () {
        let text = $(this).text();
        summaryText = `*${text}\n`;
    });
    toastr.warning(summaryText);
}

// Yenile Butonuna Tıklama
function initializeRefreshArticle() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Article/GetAllArticles/',
            contentType: "application/json",
            beforeSend: function () {
                $('#articlesTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const articleResult = jQuery.parseJSON(data);
                dataTable.clear();
                if (articleResult.Data.ResultStatus === 0) {
                    console.log("Gelen veri:", articleResult);
                    updateArticleTable(articleResult);
                } else {
                    $('.spinner-border').hide();
                    $('#articlesTable').fadeIn(1000);
                    toastr.error(`${articleResult.Data.Message}`, 'Operation Failed!');
                }
            },
            error: function (err) {
                console.log(err);
                $('.spinner-border').hide();
                $('#articlesTable').fadeIn(1000);
                toastr.error(`${err.responseText}`, 'Error!');
            }
        });
    });
}

// Makaleleri Tabloya Güncelleme
function updateArticleTable(articleResult) {
    const categoriesArray = [];
    dataTable.clear();

    $.each(articleResult.Data.Articles.$values, function (index, article) {
        const newArticle = getJsonNetObject(article, articleResult.Data.Articles.$values);

        let newCategory = getJsonNetObject(newArticle.Category, newArticle.$id);
        if (newCategory !== null) {
            categoriesArray.push(newCategory);
        }
        if (newCategory === null) {
            newCategory = categoriesArray.find((cat) => {
                return cat.$id === newArticle.Category.$ref;
            });
        }

        const newTableRow = dataTable.row.add([
            newArticle.Id,
            newCategory.Name,
            newArticle.Title,
            `<img src="/images/${newArticle.Thumbnail}" alt="${newArticle.Title}" class="my-image-table" />`,
            `${convertToShortDate(newArticle.Date)}`,
            newArticle.ViewsCount,
            newArticle.CommentCount,
            `${newArticle.IsActive ? "Yes" : "No"}`,
            `${newArticle.IsDeleted ? "Yes" : "No"}`,
            `${convertToShortDate(newArticle.CreatedDate)}`,
            newArticle.CreatedByName,
            `${convertToShortDate(newArticle.ModifiedDate)}`,
            newArticle.ModifiedByName,
            `
            <div class="form-button-action text-center">
                <a class="btn btn-link btn-primary btn-update" data-id="${newArticle.Id}" href="/Admin/Article/Update?articleId=${newArticle.Id}">
                    <span class="fas fa-pen-to-square"></span>
                </a>
                <button class="btn btn-link btn-danger btn-delete" data-id="${newArticle.Id}">
                    <span class="fas fa-trash"></span>
                </button>
            </div>
            `
        ]).node();

        const jqueryTableRow = $(newTableRow);
        jqueryTableRow.attr('name', `${newArticle.Id}`);
    });

    $('.spinner-border').hide();
    $('#articlesTable').fadeIn(1400);
}


// Silme İşlemi (SweetAlert ile)
function initializeDeleteArticle() {
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const articleTitle = tableRow.find('th:eq(2)').text();
        Swal.fire({
            title: 'Are you sure you want to delete?',
            text: `The selected article "${articleTitle}" will be permanently deleted!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it.',
            cancelButtonText: 'No, cancel.'
        }).then((result) => {
            if (result.isConfirmed) {
                deleteArticle(id, tableRow);
            }
        });
    });
}

// Article Silme
function deleteArticle(id, tableRow) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: { articleId: id },
        url: '/Admin/Article/Delete/',
        success: function (data) {
            const articleResult = jQuery.parseJSON(data);
            if (articleResult.ResultStatus === 0) {
                Swal.fire(
                    'Deleted!',
                    `${articleResult.Message}`,
                    'success'
                );
                dataTable.row(tableRow).remove().draw();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Operation Failed!',
                    text: `${articleResult.Message}`
                });
            }
        },
        error: function (err) {
            console.log(err);
            toastr.error(`${err.responseText}`, 'Error!');
        }
    });
}


