var gridListVM;

gridListVM = {
    dt: null,
    init: function () {
        dt = $('#data-table').DataTable({
            "serverSide": true,
            "processing": true,
            "ajax": {
                "url": "/MessageType/data/"
            },
            "columns": [
                { "title": "Message Code", "data": "code", "searchable": true },
                { "title": "Type", "data": "type", "searchable": true },
                {
                    render: function (o, type, data) {
                        var links = "<a class='md-btn' id='edit' href='javascript:void();' data-url-structure='MessageType/Edit' data-url-data='MessageType/EditData/" + data.id;
                        links += "' data-id='" + data.id + "' onClick='return editRow(this)'><span class='glyphicon glyphicon-edit'></span></a>" + "&nbsp;";
                        links += "<a href='MeesageType/Delete/" + data.id + "'><span class='glyphicon glyphicon-trash'></span></a>"
                        return links;
                    }
                }
            ],
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "columnDefs": [
                { "width": "8.5%", "targets": -1, "sortable": false }
            ]
        });
    }
}
// initialize the datatables
gridListVM.init();

// ViewModel Object
function messageType(messageType) {
    this.id = messageType != null ? messageType.id : 0;
    this.code = messageType != null ? messageType.code : '';
    this.type = messageType != null ? messageType.type : '';
}

var infoViewModel = {
    info: new messageType(null),
}

// Add Button to load and Submit Form
$("#loadForm").on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    
    loadModal(url, 0, null, true);
});

function loadModal(url, messageTypeId, urlData, isNew) {

    formHelper.callModal('myModal');

    $.ajax({
        url: url + "/" + messageTypeId,
        dataType: "html",
        beforeSend: function () {
            formHelper.beforeSend();
        },
        success: function (data) {
            $('#create-form').empty();
            $('#create-form').html(data);
        },
        complete: function () {
            
            if (isNew) {
                infoViewModel.info = new messageType(null);
            }
            else {
                infoViewModel.info = new GetDataFromServer().loadData(urlData, null, 'messageType', false)
            }
                        
            app = new Vue({
                el: "#create-form",
                data: {
                    loading: false,
                    vm: infoViewModel
                }
            })

            validate()
        }
    })
}

function validate() {
    
    var validator = $("form[id='messageType']").validate({
        // Specify validation rules
        rules: {
            // The key name on the left side is the name attribute
            // of an input field. Validation rules are defined
            // on the right side
            code: "required",
            type: "required"
        },
        // Specify validation error messages
        messages: {
            code: "<strong>Message Code</strong> required",
            type: "<strong>Message Type</strong> required"
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function () {
            var data = app.vm.info;
            formHelper.saveData({
                data: data, //$("form#messageType").serialize(),
                url: $('#messageType').attr('action'),
                token: $("input[name = '__RequestVerificationToken']").val()
            });
            $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
                if (app.vm.info.id == 0) {
                    app.vm.messageType = new messageType(null);
                    $('form#messageType').bootstrapValidator('resetForm', true);
                    validator.resetForm();
                }
                $('#data-table').DataTable().ajax.reload();
            });
           
        }
    });
}

//function submit(e) {
//    e.preventDefault();
    
//    var url = $('#messageType').attr('action');
//    var data = app.vm.info //$("form#messageType").serialize();
    
//    formHelper.saveData({
//        data: data, //$("form#messageType").serialize(),
//        url: $('#messageType').attr('action'),
//        token: $("input[name = '__RequestVerificationToken']").val()
//    });
//    $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
//        if (app.vm.info.id == 0) {
//            app.vm.messageType = new messageType(null);
//            $('#form').bootstrapValidator('resetForm', true);
//        }
//        $('#data-table').DataTable().ajax.reload();
//    });
//}

$("#btnSubmit").on('click', function (e) {
    e.preventDefault();
    $('#sub-btn').submit();
})

function editRow(sender) {
    var urlStructure = $(sender).attr('data-url-structure');
    var urlData = $(sender).attr('data-url-data');
    var messageTypeId = $(sender).attr('data-id');
    
    loadModal(urlStructure, messageTypeId, urlData, false);
    return false;
}