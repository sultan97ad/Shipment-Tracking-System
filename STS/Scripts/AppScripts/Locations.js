$(document).ready(function () {
    var LocationsDataTableLocale = {
        emptyTable: Locations.LocationsEmptyTable,
        sInfo: Locations.LocationsTableInfo,
        sInfoEmpty: "",
        lengthMenu: Locations.LocationsTableLengthMenu,
        loadingRecords: Locations.LocationsTableLoadingRecords,
        processing: Locations.LocationsTableProcessing,
        search: Locations.LocationsTableSearchLabel,
        zeroRecords: Locations.LocationsEmptyTable,
        paginate: {
            first: Locations.LocationsTableFirst,
            last: Locations.LocationsTableLast,
            next: Locations.LocationsTableNext,
            previous: Locations.LocationsTablePrevious
        }
    };

    var LocationsDataTableAjaxInfo = {
        url: "/Locations/LoadData",
        type: 'POST',
        datatype: "json"
    };

    var LocationsDataTableColumnDefinition = [
        {
            targets: [0],
            orderable: false,
            searchable: false
        }
    ];

    var LocationsDataTableColumn = [
        {
            data: "Address",
            render: LocationsDataTableRenderFunction
        }
    ];

    $("#Locations").DataTable({
        language: LocationsDataTableLocale,
        proccessing: true,
        serverSide: true,
        bSort: false,
        ajax: LocationsDataTableAjaxInfo,
        columnDefs:LocationsDataTableColumnDefinition,
        columns: LocationsDataTableColumn
    });

    $("#Locations").on("click", ".Remove", function () {
        var LocationId = $(this).attr('LocationId');
        var Address = $(this).attr('Address');
        ConfirmBox(Locations.RemoveLocationMessage.replace(/_Address_/g, Address), RemoveLocationAjaxRequest, LocationId, Address);
    });

    var InitialLocation = {
        latitude: $('#latitude').val(),
        longitude: $('#longitude').val()
    };

    var inputBinding = {
        latitudeInput: $('#latitude'),
        longitudeInput: $('#longitude')
    };

    $('#FormMapPlaceHolder').locationpicker({
        location:InitialLocation ,
        radius: 20,
        inputBinding:inputBinding,
        enableAutocomplete: true
    });

    $('#DetailsMapPlaceHolder').locationpicker({
        location: InitialLocation,
        radius: 20,
        markerDraggable: false
    });
});

function LocationsDataTableRenderFunction(data, type, Location) {
  Col = '<div class="p-3">' +
        '<div class="pb-3"><i class="fas fa-map-marker-alt"></i></div>' +
        '<label>' + Locations.LocationId + ':</label><text class="text-muted"> ' + Location.LocationId + '</text><br>' +
        '<label>' + Locations.Address + ':</label><text class="text-muted"> ' + Location.Address + '</text><br>' +
        '<div class="btn-group drop mt-3" >' +
        '<button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></button>' +
        '<div class="dropdown-menu border">' +
        '<a class="dropdown-item" href="/Locations/Update/' + Location.LocationId + '"><i class="fas fa-pen-square px-2"></i><text>' + Locations.LocationsTableUpdateInformationLabel + '</text></a>' +
        '<a class="dropdown-item" href="/Locations/Details/' + Location.LocationId + '"><i class="fas fa-file-alt px-2"></i><text>' + Locations.LocationsTableDetailsLabel + '</text></a>' +
        '<a class="dropdown-item" href="Employees?LocationId=' + Location.LocationId + '"><i class="fas fa-users px-2"></i><text>' + Locations.LocationsTableEmployeesLabel + '</text></a>' +
        '<a class="dropdown-item Remove" LocationId="' + Location.LocationId + '" Address="' + Location.Address + '" type="button"><i class="fas fa-trash-alt px-2"></i><text>' + Locations.LocationsTableRemoveLocationLabel + '</text></a>' +
        '</div>' +
        '</div>' +
        '</div>';
    return Col;
}

function RemoveLocationAjaxRequest(LocationId) {
    $.ajax({
        url: '/Operations-api/Locations/Remove/' + LocationId ,
        method: "POST",
        success: AjaxRequestOperationSuccess,
        error: AjaxRequestError
    });
}

function AjaxRequestOperationSuccess(Message) {
    $('#Locations').DataTable().ajax.reload();
    toastr.success(Message);
}

function AjaxRequestError(xhr) {
    if (xhr.status == 404) {
        toastr.error(Locations.LocationIdNotFound);
    }
    else {
        var Message = $.parseJSON(xhr.responseText).Message;
        toastr.error(Message);
    }
}

function ConfirmBox(Message, callback, Param1 , Param2) {
    bootbox.confirm({
        message: Message,
        centerVertical: true,
        buttons: {
            cancel: {
                label: Locations.CancelButton
            },
            confirm: {
                label: Locations.ConfirmButton
            }
        },
        callback: function (confirm) {
            if (confirm) {
                callback(Param1 , Param2);
            }
        }
    });
}










