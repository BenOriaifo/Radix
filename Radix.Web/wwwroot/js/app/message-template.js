var gridListVM;

gridListVM = {
    dt: null,
    init: function () {
        dt = $('#data-table').DataTable({
            "serverSide": true,
            "processing": true,
            "ajax": {
                "url": "/messageTemplate/data/"
            },
            "columns": [
                { "title": "Message Type", "data": "messageTypeId", "searchable": true },
                { "title": "SMS Template", "data": "smsTemplate", "searchable": true },
                { "title": "Email Template", "data": "emailTemplate", "searchable": true },
                {
                    render: function (o, type, data) {
                        var links = "<a class='md-btn' id='edit' href='javascript:void();' data-url-structure='MessageTemplate/Edit' data-url-data='MessageTemplate/EditData/" + data.id;
                        links += "' data-id='" + data.id + "' onClick='return editRow(this)'><span class='glyphicon glyphicon-edit'></span></a>" + "&nbsp;";
                        links += "<a href='MessageTemplate/Delete/" + data.id + "'><span class='glyphicon glyphicon-trash'></span></a>"
                        return links;
                    }
                }
            ],
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "columnDefs": [
                { "width": "8%", "targets": -1, "sortable": false }
            ]
        });
    }
}
// initialize the datatables
gridListVM.init();

// ViewModel Object
function messageTemplate(messageTemplate) {
    this.id = messageTemplate != null ? messageTemplate.id : 0;
    this.messageTypeId = messageTemplate != null ? messageTemplate.messageTypeId : '';
    this.smsTemplate = messageTemplate != null ? messageTemplate.smsTemplate : '';
    this.emailTemplate = messageTemplate != null ? messageTemplate.emailTemplate : '';
}

var infoViewModel = {
    info: new messageTemplate(null),
    messageTypes: []
}

// Add Button to load and Submit Form
$("#loadForm").on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');

    loadModal(url, 0, null, true);
});

function loadModal(url, messageTemplateId, urlData, isNew) {

    formHelper.callModal('myModal');

    infoViewModel.types = new GetDataFromServer().loadData('/MessageType/Get', null, 'messageTypes', false);

    $.ajax({
        url: url + "/" + messageTemplateId,
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
                infoViewModel.info = new messageTemplate(null);
            }
            else {
                infoViewModel.messageTemplate = new GetDataFromServer().loadData(urlData, null, 'messageTemplate', false)
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

    var validator = $("form[id='messageTemplate']").validate({
        // Specify validation rules
        rules: {
            // The key name on the left side is the name attribute
            // of an input field. Validation rules are defined
            // on the right side
            messageTemplate: "required",
            smsTemplate: "required",
            emailTemplate: "required"
        },
        // Specify validation error messages
        messages: {
            messageTemplate: "Please enter Message Type",
            smsTemplate: "Please enter SMS",
            emailTemplate: "Please enter Email"
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function () {

            formHelper.saveData({
                data: app.vm.info, //$("form#messageType").serialize(),
                url: $('#messageTemplate').attr('action'),
                token: $("input[name = '__RequestVerificationToken']").val()
            });
            $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
                if (app.vm.info.id == 0) {
                    app.vm.messageTemplate = new messageTemplate(null);
                    $('form#messageType').bootstrapValidator('resetForm', true);
                    validator.resetForm();
                }
                $('#data-table').DataTable().ajax.reload();
            });

        }
    });
}

function submit(e) {
    e.preventDefault();

    var url = $('#messageTemplate').attr('action');
    var data = app.vm.info //$("form#messageType").serialize();

    formHelper.saveData({
        data: data, //$("form#messageType").serialize(),
        url: $('#messageTemplate').attr('action'),
        token: $("input[name = '__RequestVerificationToken']").val()
    });
    $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
        if (app.vm.info.id == 0) {
            app.vm.messageTemplate = new messageTemplate(null);
            $('form#messageTemplate').bootstrapValidator('resetForm', true);
        }
        $('#data-table').DataTable().ajax.reload();
    });
}

$("#btnSubmit").on('click', function (e) {
    e.preventDefault();

    $('#sub-btn').submit();
    //submit(e);    
})

function editRow(sender) {
    var urlStructure = $(sender).attr('data-url-structure');
    var urlData = $(sender).attr('data-url-data');
    var messageTypeId = $(sender).attr('data-id');

    loadModal(urlStructure, messageTypeId, urlData, false);
    return false;
}