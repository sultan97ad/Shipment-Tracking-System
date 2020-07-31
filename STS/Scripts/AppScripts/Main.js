$(document).ready(function () {
    var MainDataTableLocale = {
        emptyTable: Main.TrackingRecordsEmptyTable,
        loadingRecords: Main.TrackingRecordsTableLoadingRecords,
        processing: Main.TrackingRecordsTableProcessing,
        zeroRecords: Main.TrackingRecordsEmptyTable,
    };

    var MainDataTableAjaxInfo = {
        url: "GetTrackingRecords/" + $("#TrackingRecords").attr("TrackingNumber"),
        type: 'POST',
        datatype: "json",
        dataSrc: "[]"
    };

    var MainDataTableColumnDefinition = [
        {
            targets: [0],
            orderable: false,
            searchable: false
        }
    ];

    var MainDataTableColumn = [
        {
            render: MainDataTableRenderFunction
        }
    ];

        $("#TrackingRecords").DataTable({
            language: MainDataTableLocale,
            searching: false,
            paging: false,
            ordering: false,
            info: false,
            ajax: MainDataTableAjaxInfo,
            columnDefs: MainDataTableColumnDefinition,
            columns: MainDataTableColumn
        });
});

function MainDataTableRenderFunction(data, type, TrackingRecord) {
    Col = TrackingRecord.DateTime + '<br>' + TrackingRecord.Location + '<br>' + TrackingRecord.Statement;
    return Col;
}










