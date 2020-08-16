$(document).ready(function () {
    var MainDataTableLocale = {
        emptyTable: Main.ReportsEmptyTable,
        loadingRecords: Main.ReportsTableLoadingRecords,
        processing: Main.ReportsTableProcessing,
        zeroRecords: Main.ReportsEmptyTable,
    };

    var MainDataTableAjaxInfo = {
        url: "GetReports/" + $("#Reports").attr("TrackingNumber"),
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

    $("#Reports").DataTable({
            language: MainDataTableLocale,
            searching: false,
            paging: false,
            ordering: false,
            info: false,
            ajax: MainDataTableAjaxInfo,
            columnDefs: MainDataTableColumnDefinition,
            columns: MainDataTableColumn
    });

    var InitialLocation = {
        latitude: $('#latitude').val(),
        longitude: $('#longitude').val()
    };

    var inputBinding = {
        latitudeInput: $('#latitude'),
        longitudeInput: $('#longitude')
    };

    $('#DeliveryLocationMapPlaceHolder').locationpicker({
        location: InitialLocation,
        radius: 20,
        inputBinding: inputBinding,
        enableAutocomplete: true
    });
});

function MainDataTableRenderFunction(data, type, Report) {
    Col = Report.DateTime + '<br>' + Report.Location + '<br>' + Report.Statement;
    return Col;
}










