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
        ajax: LocationsDataTableAjaxInfo,
        columnDefs:LocationsDataTableColumnDefinition,
        columns: LocationsDataTableColumn
    });

    $("#Locations").on("click", ".Remove", function () {
        var LocationId = $(this).attr('LocationId');
        var Address = $(this).attr('Address');
        ConfirmBox(Locations.RemoveLocationMessage.replace(/_Address_/g, Address), RemoveLocationAjaxRequest, LocationId, Address);
    });

});

function LocationsDataTableRenderFunction(data, type, Location) {
    Col = Locations.LocationId + ' : ' + Location.LocationId + '<br>' +
          Locations.Address + ' : ' + Location.Address + '<br>' +
          '<br>' +
        '<div class="btn-group drop" >' +
        '<button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></button>' +
        '<div class="dropdown-menu">' +
        '<a class="dropdown-item" href="/Locations/Update/' + Location.LocationId + '">' + Locations.LocationsTableUpdateInformationLabel + '</a>' +
        '<a class="dropdown-item" href="/Locations/Details/' + Location.LocationId + '">' + Locations.LocationsTableDetailsLabel + '</a>' +
        '<a class="dropdown-item" href="Employees?LocationId=' + Location.LocationId + '">' + Locations.LocationsTableEmployeesLabel + '</a>' +
        '<a class="dropdown-item Remove" href="#" LocationId="' + Location.LocationId + '" Address="' + Location.Address + '" >' + Locations.LocationsTableRemoveLocationLabel + '</a>' +
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










