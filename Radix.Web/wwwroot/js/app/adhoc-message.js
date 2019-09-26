var gridListVM;

gridListVM = {
    dt: null,
    init: function () {
        dt = $('#data-table').DataTable({
            "serverSide": true,
            "processing": true,
            "ajax": {
                "url": "/adhocMessage/data/"
            },
            "columns": [
                { "title": "Id", "data": "id", "visible": false },
                { "title": "Title", "data": "title", "searchable": true },
                { "title": "Date Created", "data": "dateCreated", "searchable": true },
                { "title": "Status", "data": "status", "searchable": true },                
                {
                    render: function (o, type, data) {
                        var links = "<a class='md-btn' id='edit' href='javascript:void();' data-url-structure='Adhoc/Edit' data-url-data='Adhoc/EditData/" + data.id;
                        links += "' data-id='" + data.id + "' onClick='return editRow(this)'><span class='glyphicon glyphicon-edit'></span></a>" + "&nbsp;";
                        links += "<a href='Adhoc/Delete/" + data.id + "'><span class='glyphicon glyphicon-trash'></span></a>"
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
