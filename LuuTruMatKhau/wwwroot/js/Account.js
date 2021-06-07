var Account = {
    CreatePrivateKey: function () {
        $.ajax({
            url: '/home/CreatePrivateKey',
            type: 'POST',
            success: function (rs) {
                if (rs.success) {
                    Toast.fire({
                        icon: 'success',
                        title: rs.message
                    });
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                } else {
                    Toast.fire({
                        icon: 'error',
                        title: rs.message
                    });
                }
            }
        })
    },
    SaveAccount: function () {
        var userName = $('#UserName').val();
        var psw = $('#Password').val();
        var memPsw = $('#mb-psw').val();
        var cateID = $('#CateID').val();

        if (userName === '' || psw === '' || memPsw === '') {
            Toast.fire({
                icon: 'error',
                title: 'Some field is empty.'
            });
            return;
        }

        $.ajax({
            url: '/home/InsertMemberAccount',
            type: 'POST',
            data: { cateID: cateID, userName: userName, psw: psw, memPsw: memPsw },
            success: function (rs) {
                if (rs.success) {
                    Toast.fire({
                        icon: 'success',
                        title: rs.message
                    });
                    $('#modal-notify').modal('hide');
                    Account.ReloadTable();

                } else {
                    Toast.fire({
                        icon: 'error',
                        title: rs.message
                    });
                }
            }
        })


    },
    ReloadTable: function () {

        var box = $('#card-tb-account');

        $.ajax({
            url: '/home/_GetAccountViewModel',
            type: 'GET',
            success: function (rs) {
                box.find('.card-body').html(rs);
                Common.appendDataTable(box.find('#tb-account'));
            }
        })
    }
}