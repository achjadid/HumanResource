function Login() {
    let validateForm = true
    if (
        $("#email").val() == "" ||
        $("#password").val() == ""
    ) {
        swal({
            icon: 'error',
            title: 'Failed',
            text: "Please fill out all your data",
        })
        validateForm = false
    }


    if (validateForm) {
        var login = new Object();
        login.email = $('#email').val();
        login.password = $('#password').val();

        $.ajax({
            "type": "POST",
            "url": "https://localhost:44311/api/accounts/login",
            "data": JSON.stringify(login),
            "contentType": "application/json;charset=utf-8",
            "success": (result) => {
                if (result.status == 200) {
                    localStorage.setItem('nik', result.data.nik);
                    localStorage.setItem('email', result.data.email);
                    localStorage.setItem('roleId', result.data.roleId);
                    localStorage.setItem('roleName', result.data.roleName);
                    localStorage.setItem('departmentId', result.data.department_Id);
                    localStorage.setItem('departmentName', result.data.departmentName);
                    localStorage.setItem('token', result.data.token);
                    window.location.assign("https://localhost:44396/account");
                } else {
                    alert("Login failed")
                }
            },
            "error": (result) => {
                swal({
                    icon: 'error',
                    title: 'Failed',
                    text: result.responseJSON.message,
                })
            },
        })
    }
}