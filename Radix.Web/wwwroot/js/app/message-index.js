var gridListVM;

function additionalParameters(obj) {
    this.dateExtractedFrom = obj != null ? obj.dateExtractedFrom : '';
    this.dateExtractedTo = obj != null ? obj.dateExtractedTo : '';
    this.dateSentFrom = obj != null ? obj.dateSentFrom : '';
    this.dateSentTo = obj != null ? obj.dateSentTo : '';
    this.employerName = obj != null ? obj.employerName : '';
    this.employerCode = obj != null ? obj.employerCode : '';
}
gridListVM = {
    dt: null,
    init: function () {
        dt = $('#data-table').DataTable({
            "serverSide": true,
            "processing": true,
            "ajax": {
                "url": "/message/data/",
                "data": function (data) {
                    data.dateExtractedFrom = $('#dateExtractedFrom').val();
                    data.dateExtractedTo = $('#dateExtractedTo').val();
                    data.dateSentFrom = $('#dateSentFrom').val();
                    data.dateSentTo = $('#dateSentTo').val();
                    data.employerName = $('#employerName').val();
                    data.employerCode = $('#employerCode').val();
                }
            },
            "columns": [
                { "title": "Id", "data": "id", "searchable": false, "visible": false },
                { "title": "Message Code", "data": "messageCode", "searchable": true },
                //{ "title": "RSA PIN", "data": "pin", "searchable": true },
                { "title": "Full Name", "data": "fullname", "searchable": true },
                { "title": "Mobile", "data": "mobilePhone", "searchable": true },
                { "title": "Employer Name", "data": "employerName", "searchable": true },
                { "title": "Status", "data": "status", "searchable": true },
                {
                    render: function (o, type, data) {
                        var links = "<a class='md-btn' id='edit' href='javascript:void();' data-url-structure='Message/Edit' data-url-data='Message/EditData/" + data.id;
                        links += "' data-id='" + data.id + "' onClick='return editRow(this)'><span class='glyphicon glyphicon-edit'></span></a> " + "&nbsp;";
                        links += "<a href='#' data-id='" + data.id + "' id='send' data-url-structure='Message/SendMessage' onClick='return send(this)' onMouseOver='return clickSend(this)' data-toggle='confirmation'><span class='fa fa-refresh'></span></a> ";
                        links += "<a href='message/Delete/" + data.id + "'><span class='glyphicon glyphicon-trash'></span></a>"
                        return links;
                    }
                }
            ],
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "columnDefs": [
                { "width": "12%", "targets": -1, "sortable": false }
            ]
        });
    }
}
// initialize the datatables
gridListVM.init();

function message(obj) {
    this.id = obj != null ? obj.id : 0;
    this.messageCode = obj != null ? obj.messageCode : '';
    this.recordDate = obj != null ? obj.recordDate : '';
    this.RSAPIN = obj != null ? obj.RSAPIN : '';
    this.fullName = obj != null ? obj.fullName : '';
    this.mobilePhone = obj != null ? obj.mobilePhone : '';
    this.email = obj != null ? obj.email : '';
    this.smsMessage = obj != null ? obj.smsMessage : '';
    this.myEmail = obj != null ? obj.myEmail : '';
    this.isSent = obj != null ? obj.isSent : 0;
    this.dateSent = obj != null ? obj.dateSent : '';
    this.status = obj != null ? obj.status : '';
    this.updateDate = obj != null ? obj.updateDate : '';
    this.messageId = obj != null ? obj.messageId : '';
}

var infoViewModel = {
    message: new message(null),
    additionalParameters: new additionalParameters(null),
    messageTypes: []
}

function loadModal(urlStructure, messageId, urlData, isNew) {
    //formHelper.callModal('message-modal');

    $.ajax({
        url: urlStructure + '/' + messageId,
        dataType: "html",
        beforeSend: function () {
            formHelper.beforeSend();
        },
        success: function (data) {
            $("div#create-form").empty();
            $("div#create-form").html(data);
            $(document).ready(function () {
                $('#emailMessage').summernote({
                    height: 150,                 // set editor height
                    minHeight: null,             // set minimum height of editor
                    maxHeight: null,             // set maximum height of editor
                    focus: true                  // set focus to editable area after initializing summernote
                });
                $("#emailMessage").on('summernote.blur', function () {
                    $('#emailMessage').html($('#emailMessage').summernote('code'));
                });
            });
        },
        complete: function () {
            infoViewModel.messageTypes = new GetDataFromServer().loadData('MessageType/GetAll', null, 'messageTypes', false);

            if (isNew) {
                infoViewModel.message = new message(null);
            }
            else {
                infoViewModel.message = new GetDataFromServer().loadData(urlData, null, 'message', false);
            }

            app = new Vue({
                el: "#create-form",
                data: {
                    loading: false,
                    vm: infoViewModel,
                    emailMessage: ''
                }
            })
            validate();
        }
    });
}

$('#loadForm').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    $("#message-modal").modal();
    loadModal(url, 0, null, true);
});

function validate(e) {
    $("form[id='message-form']").validate({
        rules: {
            fullName: "required",
            pin: "required",
            email: {
                required: true,
                // Specify that email should be validated
                // by the built-in "email" rule
                email: true
            },
            smsMessage: "required",
            emailMessage: "required",
            mobilePhone: {
                required: true,
                minlength: 12
            }
        },
        messages: {
            fullName: "<strong>Full Name </> required",
            pin: "<strong>Pin </> required",
            email: {
                required: "<strong>Email </strong> required",
            },
            mobilePhone: {
                required: "<strong>Mobile Phone </> required",
                minlength: "Your mobile must be at least 12 numbers long"
            },
            smsMessage: "<strong>SMS Message </> required",
            emailMessage: "<strong>Email Message </> required",
        },
        submitHandler: function (e) {
            var data = app.vm.message;
            var message = $('#emailMessage').summernote('code');
            console.log(message)

            formHelper.saveData({
                data: data,
                url: $('#message-form').attr('action'),
                token: $("input[name = '__RequestVerificationToken']").val()
            });
            $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
                if (app.vm.message.id == 0) {
                    app.vm.message = new message(null);
                    $('#message-form').bootstrapValidator('resetForm', true);
                }
                $('#data-table').DataTable().ajax.reload();
            });
        }
    });

}

$('#btnSubmit').on('click', function (e) {
    e.preventDefault();
    $('#sub-btn').submit();
});

$('#searchBtnSubmit').on('click', function (e) {
    e.preventDefault();
    $("#data-table").dataTable().fnDestroy();
    gridListVM.init();
});

function editRow(sender) {
    var urlStructure = $(sender).attr('data-url-structure');
    var urlData = $(sender).attr('data-url-data');
    var messageId = $(sender).attr('data-id');
    $("#message-modal").modal();
    loadModal(urlStructure, messageId, urlData, false);
    return false;
}

$(document).on('click', '#loadSearch', function (e) {
    $('#search-modal').modal('show');

    var url = $(this).attr('data-url');
    var call = new MyAjaxCall();
    call.load(url, '.modal-body-search', null, true);
});

function send(sender) {
    $('[data-toggle="confirmation"]').confirmation({
        rootSelector: '[data-toggle=confirmation]',
        title: 'Do you want to Resend?',
        placement: 'left',
        onConfirm: function () {
            var urlStructure = $(sender).attr('data-url-structure');
            var dataId = $(sender).attr('data-id');
            console.log(urlStructure, dataId);
            var data = {
                id: parseInt(dataId)
            }
            var call = new MyAjaxCall();
            call.load(urlStructure, null, data, true);
            $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function (data) {
                $.notify(data);
                $('#data-table').DataTable().ajax.reload();
            });
        },
        onCancel: function () {
            return false;
        }
    });
}

function clickSend(sender){
    $('#send').trigger('click');
}   