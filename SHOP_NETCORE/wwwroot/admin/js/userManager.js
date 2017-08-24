var user = {
    init: function () {
        user.registerEvent();
    },
    registerEvent: function () {
        $('.btnAddUser').off('click').on('click', function () {
            user.addUser();
        });

        $('.btnEditUser').off('click').on('click', function () {
            var id = $(this).data('id');
            user.editUser(id);
            $('#modal-edit-user').modal('show');
        });

        $('.btnUpdateUser').off('click').on('click', function () {
            user.updateUser();
        });

        $('.btnDeleteUser').off('click').on('click', function () {
            var id = $(this).data('id');
            user.deleteUser(id);
        });
    },

    addUser: function () {
        var userName = $('#txtUserName').val();
        var fullName = $('#txtFullName').val();
        var email = $('#txtEmail').val();
        var role = $('#selectRole').val();
        var pass = $('#txtPass').val();
        var confirmPass = $('#txtConfirmPass').val();
        var data = {
            UserName: userName,
            Name: fullName,
            Email: email,
            ApplicationRoleId: role,
            Password: pass,
            ConfirmPassword: confirmPass
        };
        $.ajax({
            url: '/admin/usermanager/adduser',
            data: { modelString: JSON.stringify(data) },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status == true)
                    location.reload();
            }
        })
    },

    editUser: function (id) {
        $.ajax({
            url: '/admin/usermanager/edituser',
            data: { id: id },
            type: 'GET',
            dataType: 'json',
            success: function (responce) {
                if (responce.status == true)
                    var data = responce.data;
                $('#txtUserNameEdit').val(data.userName);
                $('#txtFullNameEdit').val(data.name);
                $('#txtEmailEdit').val(data.email);
                $('#selectRoleEdit').val(data.applicationRoleId);
                $('#txtId').val(data.id);
            }
        })
    },

    updateUser: function () {
        var id = $('#txtId').val();
        var userName = $('#txtUserNameEdit').val();
        var fullName = $('#txtFullNameEdit').val();
        var email = $('#txtEmailEdit').val();
        var role = $('#selectRoleEdit').val();
        var data = {
            Id: id,
            UserName: userName,
            Name: fullName,
            Email: email,
            ApplicationRoleId: role
        };
        $.ajax({
            url: '/admin/usermanager/updateuser',
            data: { modelString: JSON.stringify(data) },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status == true)
                    location.reload();
            }
        })
    },

    deleteUser: function (id) {
        $.ajax({
            url: '/admin/usermanager/deleteuser',
            data: { id: id },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status == true)
                    location.reload();
            }
        })
    }
}
user.init();