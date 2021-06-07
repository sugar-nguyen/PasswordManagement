
var Common = {
    createModalCustom: function (title, url,dataObj, modalWith, button1, functionButton1, button2, functionButton2) {
        var modal = $('#modal-notify');

        $.ajax({
            url: url,
            type: 'GET',
            data: dataObj,
            success: function (rs) {
                modal.find('.modal-body').html(rs);
            }
        });

        modal.find('.modal-dialog').css('width', modalWith || 500 + 'px');
        modal.find('.modal-title').html(title);

        var _button2 = '<button type="button" class="btn btn-outline-light" data-dismiss="modal">Close</button>';
        if (button2 !== null) {
            _button2 = '<button type="button" class="btn btn-outline-light" onclick="' + functionButton2 + '">' + button2 + '</button>';
        }

        modal.find('.modal-footer').html(
            '<button type="button" class="btn btn-outline-light" onclick="' + functionButton1 + '">' + button1 + '</button>' + _button2
        );

        modal.modal('show');
    },

    appendDataTable: function (target) {

        if ($.fn.DataTable.isDataTable(target)) {
            $(target).DataTable().destroy();
        }

        $(target).DataTable({
            "searching": false,
            "pagingType": "full_numbers",
            "renderer": "bootstrap",
            "lengthChange": false,
            "info":false
        });
    },


}