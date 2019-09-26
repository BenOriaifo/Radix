var gridListVM;

gridListVM = {
    dt: null,
    init: function () {
        dt = $('#data-table').DataTable({
            "serverSide": true,
            "processing": true,
            "ajax": {
                "url": "/ServiceConfiguration/data/"
            },
            "columns": [
                { "title": "Id", "data": "id", "visible": false },
                { "title": "Message Type", "data": "messageType", "searchable": true },
                { "title": "Maximum Records", "data": "maxRecordsToFetch", "searchable": true },
                { "title": "Wait Time", "data": "waitTime", "searchable": true },
                {
                    render: function (o, type, data) {
                        var links = "<a class='md-btn' id='edit' href='javascript:void();' data-url-structure='ServiceConfiguration/Edit' data-url-data='ServiceConfiguration/EditData/" + data.id;
                        links += "' data-id='" + data.id + "' onClick='return editRow(this)'><span class='glyphicon glyphicon-edit'></span></a>" + "&nbsp;";
                        links += "<a href='ServiceConfiguration/Delete/" + data.id + "'><span class='glyphicon glyphicon-trash'></span></a>"
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

function serviceConfiguration(obj) {
    this.id = obj != null ? obj.id : 0;
    this.messageTypeId = obj != null ? obj.messageTypeId : '';
    this.maximumRecordsToFetch = obj != null ? obj.maximumRecordsToFetch : '';
    this.waitTime = obj != null ? obj.waitTime : '';
}

var infoViewModel = {
    serviceConfiguration: new serviceConfiguration(null),
    messageTypes: []
}

function loadModal(urlStructure, serviceId, urlData, isNew) {

    $.ajax({
        url: urlStructure + '/' + serviceId,
        dataType: "html",
        beforeSend: function () {
            formHelper.beforeSend();
        },
        success: function (data) {
            $("div#create-form").empty();
            $("div#create-form").html(data);
        },
        complete: function () {
            infoViewModel.messageTypes = new GetDataFromServer().loadData('MessageType/GetAll', null, 'messageTypes', false);
            if (isNew) {
                infoViewModel.serviceConfiguration = new serviceConfiguration(null);
            }
            else {
                infoViewModel.serviceConfiguration = new GetDataFromServer().loadData(urlData, null, 'serviceConfiguration', false);
            }

            app = new Vue({
                el: "#create-form",
                data: {
                    loading: false,
                    vm: infoViewModel,
                }
            })
            validate();
        }
    });
}

$('#loadForm').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    $("#service-modal").modal();
    loadModal(url, 0, null, true);
});

function validate(e) {
    $("form[id='service-form']").validate({
        rules: {
            messageTypeId: "required",
            maximumRecordsToFetch: "required",
            waitTime: "required"
        },
        messages: {
            messageTypeId: "<strong>Message Type </> required",
            maximumRecordsToFetch: "<strong>Maximum Records </> required",
            waitTime: "<strong>Wait Time </> required"
        },
        submitHandler: function (e) {
            var data = app.vm.serviceConfiguration;

            formHelper.saveData({
                data: data,
                url: $('#service-form').attr('action'),
                token: $("input[name = '__RequestVerificationToken']").val()
            });
            $(document).off('iRadixNotification.submitEvent').on('iRadixNotification.submitEvent', function () {
                if (app.vm.serviceConfiguration.id == 0) {
                    app.vm.serviceConfiguration = new serviceConfiguration(null);
                    $('#service-form').bootstrapValidator('resetForm', true);
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

function editRow(sender) {
    var urlStructure = $(sender).attr('data-url-structure');
    var urlData = $(sender).attr('data-url-data');
    var serviceId = $(sender).attr('data-id');
    $("#service-modal").modal();
    loadModal(urlStructure, serviceId, urlData, false);
    return false;
}