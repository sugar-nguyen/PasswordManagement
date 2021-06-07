var Category = {
    saveCate: function (e) {
        var cateName = $('#CateName').val();
        var desc = $('#Description').val();

        if (cateName === '' || desc === '') {
            Toast.fire({
                icon: 'error',
                title: 'Category name or description is empty'
            })
            return;
        }

        $.ajax({
            url: '/home/InsertCategory',
            type: 'POST',
            data: { cateName: cateName, description: desc },
            success: function (rs) {
                if (rs.success) {
                    Toast.fire({
                        icon: 'success',
                        title: rs.message
                    })
                    Category.reloadTBCate();
                    $('#modal-notify').modal('hide');
                } else {
                    Toast.fire({
                        icon: 'error',
                        title: rs.message
                    })
                }
            }
        });
    },
    DelCate: function (cateID) {
        Swal.fire({
            title: 'Are you sure?',
            text: "Do you want to delete ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    url: '/home/DelCategory',
                    type: 'POST',
                    data: { cateID: cateID },
                    success: function (rs) {
                        if (rs.success) {
                            Toast.fire({
                                icon: 'success',
                                title: rs.message
                            })
                            Category.reloadTBCate();
                        } else {
                            Toast.fire({
                                icon: 'error',
                                title: rs.message
                            })
                        }
                    }
                });

              
            }
        })
        
    },
    updateCate: function (cateID) {

        var cateName = $('#CateName').val();
        var desc = $('#Description').val();

        if (cateName === '' || desc === '') {
            Toast.fire({
                icon: 'error',
                title: 'Category name or description is empty'
            })
            return;
        }

        $.ajax({
            url: '/home/UpdateCategory',
            type: 'POST',
            data: {cateID: cateID, cateName: cateName, description: desc },
            success: function (rs) {
                if (rs.success) {
                    Toast.fire({
                        icon: 'success',
                        title: rs.message
                    })
                    Category.reloadTBCate();
                    $('#modal-notify').modal('hide');
                } else {
                    Toast.fire({
                        icon: 'error',
                        title: rs.message
                    })
                }
            }
        });
    },
    reloadTBCate: function () {
        $.ajax({
            url: '/home/_GetGetCategoryViewModel',
            type: 'GET',
            success: function (rs) {
                var cardCate = $('#card-tb-cate');
                cardCate.find('.card-body').html(rs);

                var table = cardCate.find('#table-cate');
                Common.appendDataTable(table);
            }
        })
    },
}