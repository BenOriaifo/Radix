var formHelper = (function () {
    function successAlert() {
        $.notify("Successful", "success");
        return false
    }

    function errorAlert() {
        $.notify("Failed", "error");
        return false
    }

    function showErrorAlert(message) {
        $.gritter.add({
            title: "<i class='fa fa-thumbs-down'></i>",
            text: message,
            sticky: false,
            time: '2000',
            class_name: "gritter-failed"
        });
        return false
    }

    function clearFields(formId) {
        $form = $("#" + formId);
        $form.bootstrapValidator('resetForm', true);
    }

    function saveData(param) {

        $.ajax({
            url: param.url,
            async: true,
            contentType: "application/json; charset=utf-8",/*"application/x-www-form-urlencoded; charset=utf-8",*/
            dataType: "json",
            headers: { "RequestVerificationToken": param.token },
            data: JSON.stringify(param.data),
            type: "POST",
            beforeSend: function (xhr) {
                $('#btnSubmit').addClass('disabled');
                $('#btnSubmit').html('Processing...');
            },
            success: function (data) {
                if (data.success) {
                    successAlert();
                    $(document).trigger('iRadixNotification.submitEvent');
                }
                else if (!data.success) {
                    $('#btnSubmit').prop('disabled', false);
                    errorAlert("Error", data.reason);
                }
            },
            complete: function () {
                $('#btnSubmit').removeClass('disabled');
                $('#btnSubmit').html('Submit');
                $('#form').find('button.btn.btn-success').removeAttr('disabled');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                errorAlert("Error", "jqXHR: " + jqXHR + "textStatus :" + textStatus + "errorThrown: " + errorThrown);
            }
        });
    }

    function saveDataMultiple(url, batchItems, fileInput) {

       $.ajax({
            url: url,
            async: true,
            contentType: "application/json",
            data: JSON.stringify(app.vm.info),
            type: "POST",
            beforeSend: function () {
                app.loading = true;
            },
            success: function (data) {
                app.loading = false;
                if (data.success) {
                    successAlert();
                
                    // Draw once all updates are done
                    myDataTable.destroy();
                    $('#' + batchItems).empty();
                    $('#' + fileInput).val('');

                    app.submitBtn = false;

                    app.vm.info.coInsurances = [];
                    app.vm.info.extensionDiscounts = [];
                }
                else if (!data.success) {
                    errorAlert("Error", data.reason);
                }
            },
            complete: function () {
                app.loading = false;
                $('#form').find('button.btn.btn-success').removeAttr('disabled');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                errorAlert("Error", "jqXHR: " + jqXHR + "textStatus :" + textStatus + "errorThrown: " + errorThrown);
            }
        });
    }

    function callModal(modalName) {
        $('#' + modalName).modal()
    }

    function beforeSend() {
        $('div#create-form').empty();
        $('div#create-form').html('<img id="modal-loader-img" alt="" src="images/spinner.gif" style="position: absolute; right: 46%; top: 0px; width:19px;" />');
    }
    return {
        saveData: saveData,
        saveDataMultiple: saveDataMultiple,
        callModal: callModal,
        clearFields: clearFields,
        successAlert: successAlert,
        errorAlert: errorAlert,
        beforeSend: beforeSend,
        showErrorAlert: showErrorAlert
    }
})();