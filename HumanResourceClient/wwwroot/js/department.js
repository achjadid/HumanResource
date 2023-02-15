$(document).ready(function () {

    var t = $('#table_departments').DataTable({
        ajax: {
            url: 'https://localhost:44311/api/departments',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: null },
            { data: 'name' },
            {
                data: 'manager',
                render: function (data) {
                    if (data) {
                        return data.firstName + ' ' + data.lastName;
                    }
                    return '';
                }
            },
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
        $('#table_departments tbody tr').each(function (index) {
            $('td', this).first().html(index + 1);
        });
    });

    $('#myFilter').on('keyup', function () {
        table
            .search(this.value)
            .draw();
    });

});

function SelectManager() {
    

};

function BtnAction(type) {
    var btnSave = 'none';
    var btnUpdate = 'none';
    var message = '';

    $('#name').val('');
    $('#manager').val('');
    $('#manager').empty();
    $('#manager').append('<option value="" selected disabled>Choose Manager...</option>');
    switch (type) {
        case 'create':
            btnSave = 'block';
            btnUpdate = 'none';
            message = "Add New Department";
            break;
        case 'update':
            btnSave = 'none';
            btnUpdate = 'block';
            message = "Update Department";
            break;
        default:
            break;
    }

    document.getElementById('save-department').style.display = btnSave;
    document.getElementById('update-department').style.display = btnUpdate;
    document.getElementById('departmentModalLabel').innerHTML = message;

}

function handleSubmit(event) {
    event.preventDefault();

    if (event.submitter.name === "save-department") {
        Save();

    } else if (event.submitter.name === "update-department") {
        Update();
    }

}

function Save() {

    var department = new Object();
    department.Name = $('#name').val();
    department.Manager_Id = $('#manager').val();
    $.ajax({
        url: 'https://localhost:44311/api/departments',
        type: 'POST',
        data: JSON.stringify(department),
        contentType: 'application/json; charset=utf-8'
    }).then((result) => {
        if (result.status == 200) {
            $('#departmentModal').modal('hide');
            $('#table_departments').DataTable().ajax.reload();
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
        url: 'https://localhost:44311/api/departments/' + id,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            $('#departmentId').val(obj.id);
            $('#name').val(obj.name);
            $('#departmentModal').modal('show');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Update() {
    var department = new Object();
    department.Id = $('#departmentId').val();
    department.Name = $('#name').val();
    department.Manager_Id = $('#manager').val();
    $.ajax({
        url: 'https://localhost:44311/api/departments',
        type: 'PUT',
        data: JSON.stringify(department),
        contentType: 'application/json; charset=utf-8'
    }).then((result) => {
        if (result.status == 200) {
            $('#departmentModal').modal('hide');
            $('#table_departments').DataTable().ajax.reload();
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
        url: "https://localhost:44311/api/departments/" + id,
        type: "DELETE",
        dataType: "json",
    }).then((result) => {
        if (result.status == 200) {
            $('#table_departments').DataTable().ajax.reload();
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