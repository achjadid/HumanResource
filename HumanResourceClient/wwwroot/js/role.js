$(document).ready(function () {

    var t = $('#table_roles').DataTable({
        ajax: {
            url: 'https://localhost:44311/api/roles',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: null },
            { data: 'name' },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="BtnAction(\'update\');GetById(' + row.id + ');"><i class="fa fa-pen"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="ConfirmDelete(' + row.id + ');"><i class="fa fa-trash"></i></button >'
                }
            }
        ],
        order: [],
        columnDefs: [
            { orderable: false, targets: [0] }
        ],
        scrollX: false,
        language: {
            infoEmpty: "No records available",
        }
    });

    t.on('draw', function () {
        $('#table_roles tbody tr').each(function (index) {
            $('td', this).first().html(index + 1);
        });
    });

    $('#myFilter').on('keyup', function () {
        table
            .search(this.value)
            .draw();
    });

});

function BtnAction(type) {
    var btnSave = 'none';
    var btnUpdate = 'none';
    var message = '';

    $('#name').val('');
    switch (type) {
        case 'create':
            btnSave = 'block';
            btnUpdate = 'none';
            message = "Add New Role";
            break;
        case 'update':
            btnSave = 'none';
            btnUpdate = 'block';
            message = "Update Role";
            break;
        default:
            break;
    }

    document.getElementById('save-role').style.display = btnSave;
    document.getElementById('update-role').style.display = btnUpdate;
    document.getElementById('roleModalLabel').innerHTML = message;

}

function handleSubmit(event) {
    event.preventDefault();

    if (event.submitter.name === "save-role") {
        Save();

    } else if (event.submitter.name === "update-role") {
        Update();
    }

}

function Save() {

    var role = new Object();
    role.Name = $('#name').val();
    $.ajax({
        url: 'https://localhost:44311/api/roles',
        type: 'POST',
        data: JSON.stringify(role),
        contentType: 'application/json; charset=utf-8'
    }).then((result) => {
        if (result.status == 200) {
            $('#roleModal').modal('hide');
            $('#table_roles').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Saved Successfully'
            });
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Failed',
                text: 'Failed to Save'
            });
        }
    })

}

function GetById(id) {
    $.ajax({
        url: 'https://localhost:44311/api/roles/' + id,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            $('#roleId').val(obj.id);
            $('#name').val(obj.name);
            $('#roleModal').modal('show');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Update() {
    var role = new Object();
    role.Id = $('#roleId').val();
    role.Name = $('#name').val();
    $.ajax({
        url: 'https://localhost:44311/api/roles',
        type: 'PUT',
        data: JSON.stringify(role),
        contentType: 'application/json; charset=utf-8'
    }).then((result) => {
        if (result.status == 200) {
            $('#roleModal').modal('hide');
            $('#table_roles').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'Updated',
                text: 'Saved Successfully'
            });
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Failed',
                text: 'Failed to Save'
            });
        }
    })
}

function Delete(id) {
    $.ajax({
        url: "https://localhost:44311/api/roles/" + id,
        type: "DELETE",
        dataType: "json",
    }).then((result) => {
        if (result.status == 200) {
            $('#table_roles').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'Deleted',
                text: 'Deleted Successfully'
            });
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Failed',
                text: 'Failed to Delete'
            });
        }
    });
}

function ConfirmDelete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            Delete(id);
        }
    })
}