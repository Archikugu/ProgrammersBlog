$(document).ready(function () {

    // DataTable Başlatılıyor
    initializeDataTable();

    //Yenile
    initializeRefreshCategories();

    //Undo
    initializeUndoDeleteCategory();

    //Hard Delete
    initializeHardDeleteCategory();
});

function initializeDataTable() {
    $('#deletedCategoriesTable').DataTable({
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

function initializeRefreshCategories() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Category/GetAllDeletedCategories/',
            contentType: "application/json",
            beforeSend: function () {
                $('#deletedCategoriesTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const categoryListDto = jQuery.parseJSON(data);
                if (categoryListDto.ResultStatus === 0) {
                    updateCategoryTable(categoryListDto);
                } else {
                    $('.spinner-border').hide();
                    $('#deletedCategoriesTable').fadeIn(1000);
                    toastr.error(`${categoryListDto.Message}`, 'Operation Failed!');
                }
            },
            error: function (err) {
                console.log(err);
                toastr.error(`${err.responseText}`, 'Error!');
            }
        });
    });
}

// Kategorileri Tabloya Güncelleme
function updateCategoryTable(categoryListDto) {
    let tableBody = "";
    $.each(categoryListDto.Categories.$values, function (index, category) {
        tableBody += `
        <tr name="${category.Id}">
            <td>${category.Id}</td>
            <td>${category.Name}</td>
            <td>${category.Description}</td>
            <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
            <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
            <td>${category.Note}</td>
            <td>${convertToShortDate(category.CreatedDate)}</td>
            <td>${category.CreatedByName}</td>
            <td>${convertToShortDate(category.ModifiedDate)}</td>
            <td>${category.ModifiedByName}</td>
            <td>
                <div class="form-button-action">
                    <button class="btn btn-link btn-warning btn-update" data-id="${category.Id}"> <span class="fas fa-undo"></span></button>
                    <button class="btn btn-link btn-danger btn-delete" data-id="${category.Id}"><span class="fas fa-trash"></span></button>
                </div>
            </td>
        </tr>`;
    });
    $('#deletedCategoriesTable > tbody').html(tableBody);
    $('.spinner-border').hide();
    $('#deletedCategoriesTable').fadeIn(1400);
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


// 2. UndoDelete Category
function initializeUndoDeleteCategory() {
    $(document).on('click', '.btn-undo', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const tableRow = $(`tr[name='${id}']`);
        let categoryName = tableRow.find('td:eq(1)').text();
        const dataTable = $('#deletedCategoriesTable').DataTable();

        Swal.fire({
            title: 'Are you sure you want to restore?',
            text: `The category "${categoryName}" will be restored from the archive!`,
            icon: 'questions',
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
                    data: { categoryId: id },
                    url: '/Admin/Category/UndoDelete/',
                    success: function (data) {
                        const undoDeletedCategoryResult = jQuery.parseJSON(data);
                        if (undoDeletedCategoryResult.ResultStatus === 0) {
                            Swal.fire('Restored!', undoDeletedCategoryResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                            
                            // Tabloyu yenile
                            $.ajax({
                                type: 'GET',
                                url: '/Admin/Category/GetAllDeletedCategories/',
                                contentType: "application/json",
                                success: function (data) {
                                    const categoryListDto = jQuery.parseJSON(data);
                                    if (categoryListDto.ResultStatus === 0) {
                                        updateCategoryTable(categoryListDto);
                                    } else {
                                        toastr.error(`${categoryListDto.Message}`, 'Operation Failed!');
                                    }
                                },
                                error: function (err) {
                                    console.log(err);
                                    toastr.error(`${err.responseText}`, 'Error!');
                                }
                            });
                        } else {
                            Swal.fire('Failed!', undoDeletedCategoryResult.Message, 'error');
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


// 3. HardDelete Category
function initializeHardDeleteCategory() {
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const tableRow = $(`tr[name='${id}']`);
        let categoryName = tableRow.find('td:eq(1)').text();
        const dataTable = $('#deletedCategoriesTable').DataTable();

        Swal.fire({
            title: 'Are you sure you want to permanently delete?',
            text: `The category "${categoryName}" will be permanently removed from the database!`,
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
                    data: { categoryId: id },
                    url: '/Admin/Category/HardDelete/',
                    success: function (data) {
                        const hardDeleteResult = jQuery.parseJSON(data);
                        if (hardDeleteResult.ResultStatus === 0) {
                            Swal.fire('Deleted!', hardDeleteResult.Message, 'success');
                            dataTable.row(tableRow).remove().draw();
                            
                            // Tabloyu yenile
                            $.ajax({
                                type: 'GET',
                                url: '/Admin/Category/GetAllDeletedCategories/',
                                contentType: "application/json",
                                success: function (data) {
                                    const categoryListDto = jQuery.parseJSON(data);
                                    if (categoryListDto.ResultStatus === 0) {
                                        updateCategoryTable(categoryListDto);
                                    } else {
                                        toastr.error(`${categoryListDto.Message}`, 'Operation Failed!');
                                    }
                                },
                                error: function (err) {
                                    console.log(err);
                                    toastr.error(`${err.responseText}`, 'Error!');
                                }
                            });
                        } else {
                            Swal.fire('Failed!', hardDeleteResult.Message, 'error');
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
