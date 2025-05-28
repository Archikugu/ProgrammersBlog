$(document).ready(function () {
    initializeRolesDataTable();
    initializeRefreshRoles();
});

function initializeRolesDataTable() {
    window.dataTable = $('#rolesTable').DataTable({
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

// 2. Yenile Butonu İşlevi
function initializeRefreshRoles() {
    $('#btnRefresh').click(function () {
        $.ajax({
            type: 'GET',
            url: '/Admin/Role/GetAllRoles/',
            contentType: "application/json",
            beforeSend: function () {
                $('#rolesTable').hide();
                $('.spinner-border').show();
            },
            success: function (data) {
                const rolesResult = jQuery.parseJSON(data);
                if (rolesResult && rolesResult.Roles) {
                    updateRolesTable(rolesResult.Roles);
                } else {
                    toastr.error('An unexpected error occurred.', 'Operation Failed!');
                    $('.spinner-border').hide();
                    $('#rolesTable').fadeIn(1000);
                }
            },
            error: function (err) {
                console.log(err);
                toastr.error(`${err.responseText}`, 'Error!');
                $('.spinner-border').hide();
                $('#rolesTable').fadeIn(1000);
            }
        });
    });
}

// 3. Tabloyu Güncelle
function updateRolesTable(roles) {
    window.dataTable.clear().draw();

    if (roles.length === 0) {
        toastr.info('No roles found to display.', 'Information');
    }

    $.each(roles, function (index, role) {
        const newRow = window.dataTable.row.add([
            role.Id,
            role.Name
        ]).draw(false).node();

        $(newRow).attr('name', role.Id);
    });

    $('.spinner-border').hide();
    $('#rolesTable').fadeIn(1400);
}
