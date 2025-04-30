$(document).ready(function () {

    // DataTable Başlatılıyor
    initializeDataTable();

    // Add (Ekle) Modal Açma İşlemi
    initializeAddUserModal();

    // Kaydet Butonuna Tıklama (Modal içinde)
    initializeSaveUser();

    // Yenile Butonuna Tıklama
    initializeRefreshUsers();

    // Silme İşlemi (SweetAlert ile)
    initializeDeleteUser();

    // Update (Güncelle) Modal Açma İşlemi
    initializeUpdateUserModal();

    // Update Butonuna Tıklama (Modal içinde)
    initializeUpdateUser();
});

// DataTable Başlatma
function initializeDataTable() {
    dataTable = $('#usersTable').DataTable({
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
function initializeAddUserModal() {
    const url = `/Admin/User/Add/`;
    const placeHolderDiv = $('#modalPlaceHolder');

    $('#btnAdd').click(function () {
        $.get(url).done(function (data) {
            placeHolderDiv.html(data);
            placeHolderDiv.find(".modal").modal('show');
        });
    });
}

// Kaydet Butonuna Tıklama (Modal içinde)
function initializeSaveUser() {
    const placeHolderDiv = $('#modalPlaceHolder');

    placeHolderDiv.on('click', '#btnSave', function (event) {
        event.preventDefault();
        const form = $('#form-user-add');
        const actionUrl = form.attr('action');
        const dataToSend = new FormData(form.get(0));

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: dataToSend,
            processData: false,
            contentType: false,
            success: function (data) {
                const userAddAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', userAddAjaxModel.UserAddPartial);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';

                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    appendNewUserRow(userAddAjaxModel);
                    toastr.success(`${userAddAjaxModel.UserDto.Message}`, 'Operation Successful!');
                } else {
                    showValidationErrors();
                }
            },
            error: function (err) {
                console.log(err);
                toastr.error(`${err.responseText}`, 'Error!');
            }
        });
    });
}

// Yeni User Satırını Tabloya Ekleme
function appendNewUserRow(userAddAjaxModel) {
    const newTableRow = dataTable.row.add([
        userAddAjaxModel.UserDto.User.Id,
        userAddAjaxModel.UserDto.User.UserName,
        userAddAjaxModel.UserDto.User.Email,
        userAddAjaxModel.UserDto.User.PhoneNumber,
        `<img src="/images/${userAddAjaxModel.UserDto.User.Picture}" alt="${userAddAjaxModel.UserDto.User.UserName}" class="my-image-table" />`,
        `<div class="form-button-action">
            <button class="btn btn-link btn-primary btn-update" data-id="${userAddAjaxModel.UserDto.User.Id}"> <span class="fas fa-pen-to-square"></span></button>
            <button class="btn btn-link btn-danger btn-delete" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-trash"></span></button>
         </div>`
    ]).node();

    const jqueryTableRow = $(newTableRow);
    jqueryTableRow.attr(`name`, `${userAddAjaxModel.UserDto.User.Id}`);

    // Yeni satırlara hizalama sınıflarını ekleyelim
    jqueryTableRow.find('td').addClass('text-center align-middle');

    dataTable.row(newTableRow).draw();
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
function initializeRefreshUsers() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/User/GetAllUsers/',
            contentType: "application/json",
            beforeSend: function () {
                $('#usersTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const userListDto = jQuery.parseJSON(data);
                dataTable.clear();
                if (userListDto.ResultStatus === 0) {
                    updateUserTable(userListDto);
                } else {
                    $('.spinner-border').hide();
                    $('#usersTable').fadeIn(1000);
                    toastr.error(`${userListDto.Message}`, 'Operation Failed!');
                }
            },
            error: function (err) {
                console.log(err);
                toastr.error(`${err.responseText}`, 'Error!');
            }
        });
    });
}

// Userları Tabloya Güncelleme
function updateUserTable(userListDto) {
    $.each(userListDto.Users.$values, function (index, user) {
        const newTableRow = dataTable.row.add([
            user.Id,
            user.UserName,
            user.Email,
            user.PhoneNumber,
            `<img src="/images/${user.Picture}" alt="${user.UserName}" class="my-image-table" />`,
            `<div class="form-button-action">
                <button class="btn btn-link btn-primary btn-update" data-id="${user.Id}"> <span class="fas fa-pen-to-square"></span></button>
                <button class="btn btn-link btn-danger btn-delete" data-id="${user.Id}"><span class="fas fa-trash"></span></button>
            </div>`
        ]).node();

        const jqueryTableRow = $(newTableRow);
        jqueryTableRow.attr('name', `${user.Id}`);

        // Yeni satırlara hizalama sınıflarını ekleyelim
        jqueryTableRow.find('td').addClass('text-center align-middle');
    });

    dataTable.draw();
    $('.spinner-border').hide();
    $('#usersTable').fadeIn(1400);
}

// Silme İşlemi (SweetAlert ile)
function initializeDeleteUser() {
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const userName = tableRow.find('td:eq(1)').text();
        Swal.fire({
            title: 'Are you sure you want to delete?',
            text: `The selected user "${userName}" will be permanently deleted!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it.',
            cancelButtonText: 'No, cancel.'
        }).then((result) => {
            if (result.isConfirmed) {
                deleteUser(id, tableRow);
            }
        });
    });
}

// User Silme
function deleteUser(id, tableRow) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: { userId: id },
        url: '/Admin/User/Delete/',
        success: function (data) {
            const userDto = jQuery.parseJSON(data);
            if (userDto.ResultStatus === 0) {
                Swal.fire(
                    'Deleted!',
                    `The "${userDto.User.UserName}" user has been successfully deleted.`,
                    'success'
                );
                dataTable.row(tableRow).remove().draw();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Operation Failed!',
                    text: `${userDto.Message}`
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
function initializeUpdateUserModal() {
    const url = `/Admin/User/Update/`;
    const placeHolderDiv = $('#modalPlaceHolder');

    $(document).on('click', '.btn-update', function (event) {
        event.preventDefault();
        const id = $(this).data('id');

        $.get(url, { userId: id }).done(function (data) {
            placeHolderDiv.html(data);
            placeHolderDiv.find('.modal').modal('show');
        }).fail(function () {
            toastr.error("An error occurred while loading the update form.");
        });
    });
}


// Update Butonuna Tıklama (Modal içinde)
function initializeUpdateUser() {
    const placeHolderDiv = $('#modalPlaceHolder');

    placeHolderDiv.on('click', '#btnUpdate', function (event) {
        event.preventDefault();
        const form = $('#form-user-update');
        const actionUrl = form.attr('action');
        const dataToSend = new FormData(form.get(0));

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: dataToSend,
            processData: false,
            contentType: false,
            success: function (data) {
                const userUpdateAjaxModel = jQuery.parseJSON(data);

                if (!userUpdateAjaxModel.UserDto || !userUpdateAjaxModel.UserDto.User) {
                    toastr.error(`${userUpdateAjaxModel.UserDto?.Message ?? "Unknown error occurred."}`, "Update Failed");
                    return;
                }
                const newFormBody = $('.modal-body', userUpdateAjaxModel.UserUpdatePartial);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);

                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';

                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    updateUserRow(userUpdateAjaxModel);
                    toastr.success(`${userUpdateAjaxModel.UserDto.Message}`, "Operation Successful!");
                } else {
                    showValidationErrors();
                }
            },
            error: function (err) {
                console.error(err);
                toastr.error(`${err.responseText}`, 'Error!');
            }
        });
    });
}


// Yeni Güncellenmis user Satırını Tabloya Ekleme
function updateUserRow(userUpdateAjaxModel) {
    const updatedUser = userUpdateAjaxModel.UserDto.User;
    const tableRow = $(`[name="${updatedUser.Id}"]`);

    dataTable.row(tableRow).data([
        updatedUser.Id,
        updatedUser.UserName,
        updatedUser.Email,
        updatedUser.PhoneNumber,
        `<img src="/images/${updatedUser.Picture}" alt="${updatedUser.UserName}" class="my-image-table" />`,
        `<div class="form-button-action">
            <button class="btn btn-link btn-primary btn-update" data-id="${updatedUser.Id}"><span class="fas fa-pen-to-square"></span></button>
            <button class="btn btn-link btn-danger btn-delete" data-id="${updatedUser.Id}"><span class="fas fa-trash"></span></button>
         </div>`
    ]).draw(false);

    tableRow.attr("name", updatedUser.Id);
}
