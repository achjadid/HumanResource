$(document).ready(function () {

    var t = $('#table_accounts').DataTable({
        ajax: {
            url: 'https://localhost:44311/api/employees',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: null },
            {
                data: 'role',
                render: function (data) {
                    var roles = [];
                    data.forEach(function (element) {
                        roles.push(element.name)
                    })
                    return roles.join(", ");
                }
            },
            { data: 'email' },
            {
                "render": function (data, type, row) {
                    var arrName = [];
                    if (row.firstName != null) arrName.push(row.firstName);
                    if (row.lastName != null) arrName.push(row.lastName);
                    return arrName.join(" ");
                }
            },
            {
                "render": function (data, type, row) {
                    if (row.birthDate == null) return "";
                    var monthId = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                    const date = new Date(row.birthDate);
                    const formatedDate = date.getDate() + " " + monthId[date.getMonth()] + " " + date.getFullYear();
                    return formatedDate;
                }
            },
            {
                "data": "gender",
                "render": function (data) {
                    if (data == 1) {
                        return "Female";
                    }
                    return "Male";
                }
            },
            { data: 'salary' },
            { data: 'departmentName' },
            { data: 'managerName' },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="BtnAction(\'update\');GetById(\'' + row.nik + '\');"><i class="fa fa-pen"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="ConfirmDelete(\'' + row.nik + '\');"><i class="fa fa-trash"></i></button >'
                }
            }
        ],
        order: [],
        columnDefs: [
            { orderable: false, targets: [0, 4] }
        ],
        scrollX: false,
        language: {
            infoEmpty: "No records available",
        }
    });

    t.on('draw', function () {
        $('#table_accounts tbody tr').each(function (index) {
            $('td', this).first().html(index + 1);
        });
    });

    $('#myFilter').on('keyup', function () {
        table
            .search(this.value)
            .draw();
    });

    function setPhoneFormat(textbox, phoneFormat, errMsg) {
        ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop", "focusout"].forEach(function (event) {
            textbox.addEventListener(event, function (e) {
                if (!isNaN(this.value) && this.value.length === 1 && this.value != '0') {
                    this.value = '0' + this.value;
                }
                if (!isNaN(this.value) && this.value.length > 1 && this.value == '00') {
                    this.value = '0';
                }
                if (phoneFormat(this.value)) {
                    // Accepted value.
                    if (["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0) {
                        this.classList.remove("input-error");
                        this.setCustomValidity("");
                    }

                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                }
                else if (this.hasOwnProperty("oldValue")) {
                    // Rejected value: restore the previous one.
                    this.classList.add("input-error");
                    this.setCustomValidity(errMsg);
                    this.reportValidity();
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                }
                else {
                    // Rejected value: nothing to restore.
                    this.value = "";
                }
            });
        });
    }

    setPhoneFormat(document.getElementById("phone"), function (value) {
        return /^\d*$/.test(value);
    }, "number only");

});

function SelectDepartment() {
    $('#department').empty();
    $('#department').append('<option value="" selected disabled>Choose Department...</option>');
    $.ajax({
        type: 'GET',
        url: "https://localhost:44311/api/departments",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            for (var i = 0; i < obj.length; i++) {
                $('#department').append('<option value="' + obj[i].id + '">' + obj[i].name + '</option>');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

};

function BtnAction(type) {
    var btnSave = 'none';
    var btnUpdate = 'none';
    var message = '';

    $('#email').val('');
    $('#password').val('');
    document.getElementById("password").setAttribute("required", "true");
    $('#firstName').val('');
    $('#lastName').val('');
    $('#birthDate').val('');
    $('#gender').val('');
    $('#phone').val('');
    $('#salary').val('');
    $('#department').val('');
    $('#department').empty();
    $('#department').append('<option value="" selected disabled>Choose Department...</option>');
    $('#manager').val('');
    $('#manager').empty();
    $('#manager').append('<option value="" selected disabled>Choose Manager...</option>');
    switch (type) {
        case 'create':
            btnSave = 'block';
            btnUpdate = 'none';
            message = "Add New Account";
            break;
        case 'update':
            btnSave = 'none';
            btnUpdate = 'block';
            message = "Update Account";
            document.getElementById("password").removeAttribute("required");
            break;
        default:
            break;
    }

    document.getElementById('save-account').style.display = btnSave;
    document.getElementById('update-account').style.display = btnUpdate;
    document.getElementById('accountModalLabel').innerHTML = message;

}

function handleSubmit(event) {
    event.preventDefault();

    console.log("handle submit");
    if (event.submitter.name === "save-account") {
        Save();

    } else if (event.submitter.name === "update-account") {
        console.log("handle submit update");
        Update();
    }

}

function Save() {
    var account = new Object();
    account.Email = $('#email').val();
    account.Password = $('#password').val();
    account.FirstName = $('#firstName').val();
    account.LastName = $('#lastName').val();
    account.BirthDate = $('#birthDate').val();
    account.Gender = parseInt($('#gender').val());
    account.Phone = $('#phone').val();
    account.Salary = parseInt($('#salary').val());
    account.Department_Id = $('#department').val();
    account.Manager_Id = $('#manager').val();
    $.ajax({
        url: 'https://localhost:44311/api/employees',
        type: 'POST',
        data: JSON.stringify(account),
        contentType: 'application/json; charset=utf-8'
    }).then((result) => {
        if (result.status == 200) {
            $('#accountModal').modal('hide');
            $('#table_accounts').DataTable().ajax.reload();
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

function GetById(nik) {
    $.ajax({
        url: 'https://localhost:44311/api/employees/' + nik,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            $.ajax({
                type: 'GET',
                url: "https://localhost:44311/api/departments",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    var department = result.data;
                    for (var i = 0; i < department.length; i++) {
                        if (obj.department_Id == department[i].id) {
                            $('#department').append('<option value="' + department[i].id + '" selected>' + department[i].name + '</option>');
                        }
                        else {
                            $('#department').append('<option value="' + department[i].id + '">' + department[i].name + '</option>');
                        }
                    }
                }
            });
            $('#accountId').val(obj.nik);
            $('#email').val(obj.email);
            $('#firstName').val(obj.firstName);
            $('#lastName').val(obj.lastName);
            $('#birthDate').val(obj.birthDate.split('T')[0]);
            $('#gender').val(obj.gender);
            $('#phone').val(obj.phone);
            $('#salary').val(obj.salary);
            $('#accountModal').modal('show');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Update() {
    var account = new Object();
    account.NIK = $('#accountId').val();
    account.Email = $('#email').val();
    account.Password = $('#password').val();
    account.FirstName = $('#firstName').val();
    account.LastName = $('#lastName').val();
    account.BirthDate = $('#birthDate').val();
    account.Gender = parseInt($('#gender').val());
    account.Phone = $('#phone').val();
    account.Salary = parseInt($('#salary').val());
    account.Department_Id = $('#department').val();
    account.Manager_Id = $('#manager').val();
    $.ajax({
        url: 'https://localhost:44311/api/employees',
        type: 'PUT',
        data: JSON.stringify(account),
        contentType: 'application/json; charset=utf-8'
    }).then((result) => {
        if (result.status == 200) {
            $('#accountModal').modal('hide');
            $('#table_accounts').DataTable().ajax.reload();
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

function Delete(nik) {
    $.ajax({
        url: "https://localhost:44311/api/employees/" + nik,
        type: "DELETE",
        dataType: "json",
    }).then((result) => {
        if (result.status == 200) {
            $('#table_accounts').DataTable().ajax.reload();
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

function ConfirmDelete(nik) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Delete(nik);
        }
    })
}