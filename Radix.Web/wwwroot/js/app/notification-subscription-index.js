var gridListVM;

gridListVM = {
    dt: null,
    init: function () {
        dt = $('#data-table').DataTable({
            "serverSide": true,
            "processing": true,
            "ajax": {
                "url": "/notificationsubscription/data/"
            },
            "columns": [
                { "title": "Id", "data": "id", "searchable": false, "visible": false },
                { "title": "Full Name", "data": "fullName", "searchable": true },
                { "title": "Pin", "data": "pin", "searchable": true },
                { "title": "Email", "data": "email", "searchable": true },
                { "title": "Mobile", "data": "mobile", "searchable": true },
                {
                    render: function (o, type, data) {
                        var links = "<a class='md-btn' id='edit' href='javascript:void();' data-url-structure='NotificationSubscription/Edit' data-url-data='NotificationSubscription/EditData/" + data.id;
                        links += "' data-id='" + data.id + "' onClick='return editRow(this)'><span class='glyphicon glyphicon-edit'></span></a>" + "&nbsp;";
                        links += "<a href='NotificationSubscription/Delete/" + data.id + "'><span class='glyphicon glyphicon-trash'></span></a>"
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

function notificationSubscription(obj) {
    this.id = obj != null ? obj.id : 0;
    this.pin = obj != null ? obj.pin : '';
    this.fullName = obj != null ? obj.fullName : '';
    this.mobile = obj != null ? obj.mobile : '';
    this.email = obj != null ? obj.email : '';
    this.notificationPreferrences = [];
}

function notificationPreferrence(obj) {
    this.id = obj != null ? obj.id : 0;
    this.NotificationSubscriptionId = obj != null ? obj.NotificationSubscriptionId : '';
    this.MessageTypeId = obj != null ? obj.MessageTypeId : '';
}

var infoViewModel = {
    notificationSubscription: new notificationSubscription(null),
    notificationPreferrence: new notificationPreferrence(null),
    messageTypes: []
}

function loadModal(urlStructure, notificationId, urlData, isNew) {

    $.ajax({
        url: urlStructure + '/' + notificationId,
        dataType: "html",
        beforeSend: function () {
            formHelper.beforeSend();
        },
        success: function (data) {
            $("#create-form").empty();
            $("#create-form").html(data);
        },
        complete: function () {
            infoViewModel.messageTypes = new GetDataFromServer().loadData('MessageType/GetAll', null, 'messageTypes', false);

            if (isNew) {
                infoViewModel.notificationSubscription = new notificationSubscription(null);
                infoViewModel.notificationPreferrence = new notificationPreferrence(null);
            }
            else {
                infoViewModel.notificationSubscription = new GetDataFromServer().loadData(urlData, null, 'notificationSubscription', false);
            }

            app = new Vue({
                el: "#create-form",
                data: {
                    loading: false,
                    vm: infoViewModel
                },
                methods: {
                    saveNotificationPreferences: function (e) {
                        var msgId = $("#messageId").val();

                        infoViewModel.notificationPreferrence = {
                            MessageTypeId: msgId,
                            NotificationSubscriptionId: infoViewModel.notificationSubscription.id
                        }
                        infoViewModel.notificationSubscription.notificationPreferrences = new Array();
                        infoViewModel.notificationSubscription.notificationPreferrences.push(infoViewModel.notificationPreferrence);
                    }
                }
            });

            validate();
        }
    });
}

function validate(e) {
    $("form[id='notification-form']").validate({
        rules: {
            fullName: "required",
            pin: "required",
            email: {
                required: true,
                // Specify that email should be validated
                // by the built-in "email" rule
                email: true
            },
            mobile: {
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
            mobile: {
                required: "<strong>Mobile Phone </> required",
                minlength: "Your mobile must be at least 12 numbers long"
            },
            smsMessage: "<strong>SMS Message </> required",
            emailMessage: "<strong>Email Message </> required",
        },
        submitHandler: function (e) {
            var data = app.vm.notificationSubscription;

            formHelper.saveData({
                data: data,
                url: $('#notification-form').attr('action'),
                token: $("input[name = '__RequestVerificationToken']").val()
            });
            $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
                if (app.vm.notificationSubscription.id == 0) {
                    app.vm.notificationSubscription = new notificationSubscription(null);
                    $('#notification-form').bootstrapValidator('resetForm', true);
                }
                $('#data-table').DataTable().ajax.reload();
            });
        }
    });
}

$('#loadForm').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    $("#notification-modal").modal();
    loadModal(url, 0, null, true);
});

$(document).on('click', '#btnSubmit', function (e) {
    e.preventDefault();
    $('#sub-btn').submit();
});

function editRow(sender) {
    var urlStructure = $(sender).attr('data-url-structure');
    var urlData = $(sender).attr('data-url-data');
    var notificationId = $(sender).attr('data-id');
    $("#notification-modal").modal();
    loadModal(urlStructure, notificationId, urlData, false);
    return false;
}
