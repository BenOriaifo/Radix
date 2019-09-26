function MyAjaxCall() {

    this.load = function (url, displayGrid, data, isAsync) {
        $.ajax({
            url: url,
            async: isAsync,
            dataType: "html",
            data: data,
            beforeSend: function () {
                $(displayGrid).empty().removeClass('auto-height');
                $(displayGrid).html('<img id="loader-img" alt="" src="images/spinner.gif" width="50" height="50" style="position: absolute; right: 50%; top: 30vh;" />');
            },
            success: function (data) {
                $(displayGrid).html(data).addClass('auto-height');
                $(document).trigger('iRadixNotification.submitEvent');
                if (!data.success) {
                    $(document).trigger('iRadixNotification.submitEvent', data);
                }
            }
        });
    }
}

function GetDropDownData() {

    this.load = function (url, viewModel, dataToSend, viewModelArrayProperty, returnData, isAsync) {
        $.ajax({
            async: isAsync,
            url: url,
            contentType: "application/json",
            type: "GET",
            data: dataToSend,
            success: function (data) {
                var parentData = data[returnData];
                if (parentData) {
                    viewModel[viewModelArrayProperty] = [];
                    parentData.forEach(function (value) {
                        viewModel[viewModelArrayProperty].push(new dropDownData(value.id, value.name));
                    });
                }
            }
        });
    }
}

function GetDataFromServer() {

    this.loadData = function (url, dataToSend, returnData, isAsync) {
        var result = [];
        $.ajax({
            async: isAsync,
            url: url,
            contentType: "application/json",
            type: "GET",
            data: dataToSend,
            success: function (data) {
                if (returnData != null) {
                    var parentData = data[returnData];
                    if (parentData) {
                        result = parentData
                    }
                }
                else {
                    result = data;
                }
           }
        });
        return result;
    }
}
