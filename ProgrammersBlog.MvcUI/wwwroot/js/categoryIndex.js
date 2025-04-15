$(document).ready(function () {

    // DataTable Başlatılıyor
    initializeDataTable();

    // Add (Ekle) Modal Açma İşlemi
    initializeAddCategoryModal();

    // Kaydet Butonuna Tıklama (Modal içinde)
    initializeSaveCategory();

    // Yenile Butonuna Tıklama
    initializeRefreshCategories();

    // Silme İşlemi (SweetAlert ile)
    initializeDeleteCategory();

    // Update (Güncelle) Modal Açma İşlemi
    initializeUpdateCategoryModal();

    // Update Butonuna Tıklama (Modal içinde)
    initializeUpdateCategory();
});

// DataTable Başlatma
function initializeDataTable() {
    $('#categoriesTable').DataTable({
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

// Add (Ekle) Modal Açma İşlemi
function initializeAddCategoryModal() {
    const url = `/Admin/Category/Add/`;
    const placeHolderDiv = $('#modalPlaceHolder');

    $('#btnAdd').click(function () {
        $.get(url).done(function (data) {
            placeHolderDiv.html(data);
            placeHolderDiv.find(".modal").modal('show');
        });
    });
}

// Kaydet Butonuna Tıklama (Modal içinde)
function initializeSaveCategory() {
    const placeHolderDiv = $('#modalPlaceHolder');

    placeHolderDiv.on('click', '#btnSave', function (event) {
        event.preventDefault();
        const form = $('#form-category-add');
        const actionUrl = form.attr('action');
        const dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            const categoryAddAjaxModel = jQuery.parseJSON(data);
            const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial);
            placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
            const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';

            if (isValid) {
                placeHolderDiv.find('.modal').modal('hide');
                appendNewCategoryRow(categoryAddAjaxModel);
                toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Operation Successful !');
            } else {
                showValidationErrors();
            }
        });
    });
}

// Yeni Kategori Satırını Tabloya Ekleme
function appendNewCategoryRow(categoryAddAjaxModel) {
    const newTableRow = `
        <tr name = "${categoryAddAjaxModel.CategoryDto.Category.Id}">
            <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
            <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
            <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
            <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
            <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
            <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
            <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
            <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
            <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
            <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
            <td>
                <div class="form-button-action">
                    <button class="btn btn-link btn-primary btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"> <span class="fas fa-pen-to-square"></span></button>
                    <button class="btn btn-link btn-danger btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-trash"></span></button>
                </div>
            </td>
        </tr>`;
    const newTableRowObject = $(newTableRow);
    newTableRowObject.hide();
    $('#categoriesTable tbody').append(newTableRowObject);
    newTableRowObject.fadeIn(3500);
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
function initializeRefreshCategories() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Category/GetAllCategories/',
            contentType: "application/json",
            beforeSend: function () {
                $('#categoriesTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const categoryListDto = jQuery.parseJSON(data);
                if (categoryListDto.ResultStatus === 0) {
                    updateCategoryTable(categoryListDto);
                } else {
                    $('.spinner-border').hide();
                    $('#categoriesTable').fadeIn(1000);
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
                    <button class="btn btn-link btn-primary btn-update" data-id="${category.Id}"> <span class="fas fa-pen-to-square"></span></button>
                    <button class="btn btn-link btn-danger btn-delete" data-id="${category.Id}"><span class="fas fa-trash"></span></button>
                </div>
            </td>
        </tr>`;
    });
    $('#categoriesTable > tbody').html(tableBody);
    $('.spinner-border').hide();
    $('#categoriesTable').fadeIn(1400);
}

// Silme İşlemi (SweetAlert ile)
function initializeDeleteCategory() {
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const categoryName = tableRow.find('td:eq(1)').text();
        Swal.fire({
            title: 'Are you sure you want to delete?',
            text: `The selected category "${categoryName}" will be permanently deleted!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it.',
            cancelButtonText: 'No, cancel.'
        }).then((result) => {
            if (result.isConfirmed) {
                deleteCategory(id, tableRow);
            }
        });
    });
}

// Kategoriyi Silme
function deleteCategory(id, tableRow) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: { categoryId: id },
        url: '/Admin/Category/Delete/',
        success: function (data) {
            const categoryDto = jQuery.parseJSON(data);
            if (categoryDto.ResultStatus === 0) {
                Swal.fire(
                    'Deleted!',
                    `The "${categoryDto.Category.Name}" category has been successfully deleted.`,
                    'success'
                );
                tableRow.fadeOut(3500);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Operation Failed!',
                    text: `${categoryDto.Message}`
                });
            }
        },
        error: function (err) {
            console.log(err);
            toastr.error(`${err.responseText}`, 'Error!');
        }
    });
}

// Update (Güncelle) Modal Açma İşlemi
function initializeUpdateCategoryModal() {
    const url = `/Admin/Category/Update/`;
    const placeHolderDiv = $('#modalPlaceHolder');

    $(document).on('click', '.btn-update', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');

        $.get(url, { categoryId: id }).done(function (data) {
            placeHolderDiv.html(data);
            placeHolderDiv.find(".modal").modal('show');
        }).fail(function () {
            toastr.error("Operation Failed!", "Error!");
        });
    });
}

// Update Butonuna Tıklama (Modal içinde)
function initializeUpdateCategory() {
    const placeHolderDiv = $('#modalPlaceHolder');

    placeHolderDiv.on('click', '#btnUpdate', function (event) {
        event.preventDefault();
        const form = $('#form-category-update');
        const actionUrl = form.attr('action');
        const dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            const categoryUpdateAjaxModel = jQuery.parseJSON(data);
            console.log(categoryUpdateAjaxModel);
            const newFormBody = $('.modal-body', categoryUpdateAjaxModel.CategoryUpdatePartial);
            placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
            const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
            if (isValid) {
                placeHolderDiv.find('.modal').modal('hide');
                appendUpdatedNewCategoryRow(categoryUpdateAjaxModel);
                toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, 'Operation Successful !');
            } else {
                showValidationErrors();
            }
        }).fail(function (response) {
            console.log(response);
        });
    });
}

// Yeni Güncellenmis Kategori Satırını Tabloya Ekleme
function appendUpdatedNewCategoryRow(categoryUpdateAjaxModel) {
    const newTableRow = `
        <tr name ="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td>
            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Description}</td>
            <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
            <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
            <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.CreatedDate)}</td>
            <td>${categoryUpdateAjaxModel.CategoryDto.Category.CreatedByName}</td>
            <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
            <td>${categoryUpdateAjaxModel.CategoryDto.Category.ModifiedByName}</td>
            <td>
                <div class="form-button-action">
                    <button class="btn btn-link btn-primary btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"> <span class="fas fa-pen-to-square"></span></button>
                    <button class="btn btn-link btn-danger btn-delete" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-trash"></span></button>
                </div>
            </td>
        </tr>`;
    const newTableRowObject = $(newTableRow);
    const categoryTableRow = $(`[name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"]`);
    newTableRowObject.hide();
    categoryTableRow.replaceWith(newTableRowObject);
    newTableRowObject.fadeIn(3500);
}