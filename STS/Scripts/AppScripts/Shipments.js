$(document).ready(function () {
    var ShipmentsDataTableLocale = {
        emptyTable: Shipments.ShipmentsEmptyTable,
        sInfo: Shipments.ShipmentsTableInfo,
        sInfoEmpty: "",
        lengthMenu: Shipments.ShipmentsTableLengthMenu,
        loadingRecords: Shipments.ShipmentsTableLoadingRecords,
        processing: Shipments.ShipmentsTableProcessing,
        search: Shipments.ShipmentsTableSearchLabel,
        zeroRecords: Shipments.ShipmentsEmptyTable,
        paginate: {
            first: Shipments.ShipmentsTableFirst,
            last: Shipments.ShipmentsTableLast,
            next: Shipments.ShipmentsTableNext,
            previous: Shipments.ShipmentsTablePrevious
        }
    };

    var ShipmentsDataTableAjaxInfo = {
        url: "/Shipments/LoadData",
        type: 'POST',
        datatype: "json"
    };

    var ShipmentsDataTableColumnDefinition = [
        {
            targets: [0],
            orderable: false,
            searchable: false
        }
    ];

    var ShipmentsDataTableColumn = [
        {
            data: "TrackingNumber",
            render: ShipmentsDataTableRenderFunction
        }
    ];

    $("#Shipments").DataTable({
        language: ShipmentsDataTableLocale,
        proccessing: true,
        serverSide: true,
        ajax: ShipmentsDataTableAjaxInfo,
        columnDefs: ShipmentsDataTableColumnDefinition,
        columns: ShipmentsDataTableColumn
    });

    $("#Shipments").on("click", ".Departed", function () {
        var TrackingNumber = $(this).attr('TrackingNumber');
        ConfirmBox(Shipments.ShipmentDepartedMessage.replace(/_TrackingNumber_/g, TrackingNumber), DepartedAjaxRequest, TrackingNumber);
    });

    $("#Shipments").on("click", ".Collected", function () {
        var TrackingNumber = $(this).attr('TrackingNumber');
        PromptBox(Shipments.InsertTheNameOfShipmentCollector, EmptyCollectorNameError , CollectedAjaxRequest, TrackingNumber);
    });

    $("#Shipments").on("click", ".SendSMS", function () {

    });

    $("#ShipmentArrival").on("click", function () {
        PromptBox(Shipments.InsertTrackingNumberOfArrivedShipment, EmptyTrackingNumberError, ShipmentPreviewAjaxRequest);
    });
});



function ShipmentsDataTableRenderFunction(data, type, Shipment) {
    Col =    Shipments.TrackingNumber + ' : ' + Shipment.TrackingNumber + '<br>' +
             Shipments.ReceiverName + ' : ' + Shipment.ReceiverName + '<br>' +
             Shipments.Destination + ' : ' + Shipment.Destination + '<br>' +
             Shipments.Status + ' : ' + Shipment.Status + '<br>' +
             Shipments.HoldSince + ' : ' + Shipment.HoldSince + ' ' + Shipments.Days + '<br>' +
            '<br>' +
            '<div class="btn-group drop">' +
            '<button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></button>' +
            '<div class="dropdown-menu">' +
            '<a class="dropdown-item" href="/Shipments/Update/' + Shipment.TrackingNumber + '">' + Shipments.ShipmentsTableUpdateInformationLabel + '</a>' +
            '<a class="dropdown-item" href="/Shipments/Details/' + Shipment.TrackingNumber + '">' + Shipments.ShipmentsTableDetailsLabel + '</a>';
        if (IsWaitingShipping(Shipment)) {
            Col += '<a class="dropdown-item Departed" TrackingNumber="' + Shipment.TrackingNumber + '" href="#">' + Shipments.ShipmentsTableDepartedLabel + '</a>';
        } else {
            Col += '<a class="dropdown-item Collected" TrackingNumber="' + Shipment.TrackingNumber + '" href="#">' + Shipments.ShipmentsTableCollectedLabel + '</a>' +
                   '<a class="dropdown-item SendSMS" TrackingNumber="' + Shipment.TrackingNumber + '" href="#">' + Shipments.ShipmentsTableSendSMSLabel + '</a>';
        }
        Col += '</div>' +
               '</div>';
        return Col;
}

function ShipmentPreview(Shipment) {
    ConfirmBox(Shipments.ShipmentPreviewMessage.replace(/_TrackingNumber_/g, Shipment.TrackingNumber).replace(/_ReceiverName_/g, Shipment.ReceiverName).replace(/_Destination_/g, Shipment.Destination), ShipmentArrivedAjaxRequest, Shipment.TrackingNumber);
}

function DepartedAjaxRequest(TrackingNumber) {
        $.ajax({
            url: '/Operations-api/Shipments/' + TrackingNumber,
            method: "POST",
            success: AjaxRequestOperationSuccess,
            error: AjaxRequestError
        });
}

function CollectedAjaxRequest(CollectorName , TrackingNumber) {
    $.ajax({
        url: '/Operations-api/Shipments/Collected/' + TrackingNumber + '?CollectorName='+ CollectorName,
        method: "POST",
        success: AjaxRequestOperationSuccess,
        error: AjaxRequestError
    });
}

function ShipmentPreviewAjaxRequest(TrackingNumber) {
    $.ajax({
        url: '/Operations-api/Shipments/Arrived/' + TrackingNumber ,
        method: "GET",
        success: ShipmentPreview,
        error: AjaxRequestError
    });
}

function ShipmentArrivedAjaxRequest(TrackingNumber) {
    $.ajax({
        url: '/Operations-api/Shipments/Arrived/'+ TrackingNumber ,
        method: "POST",
        success: AjaxRequestOperationSuccess,
        error: AjaxRequestError
    });
}

function AjaxRequestOperationSuccess(Message) {
     $('#Shipments').DataTable().ajax.reload();
    toastr.success(Message);
}

function AjaxRequestError(xhr) {
    if (xhr.status == 404) {
        toastr.error(Shipments.TrackingNumberNotFound);
    }
    else
    {
        var Message = $.parseJSON(xhr.responseText).Message;
        toastr.error(Message);
    }
}

function EmptyTrackingNumberError() {
    toastr.error(Shipments.TrackingNumberRequired);
}

function EmptyCollectorNameError() {
    toastr.error(Shipments.CollectorNameRequired);
}

function ConfirmBox(Message, callback, Param) {
    bootbox.confirm({
        message: Message,
        centerVertical: true,
        buttons: {
            cancel: {
                label: Shipments.CancelButton
            },
            confirm: {
                label: Shipments.ConfirmButton
            }
        },
        callback: function (confirm) {
            if (confirm) {
                callback(Param);
            }
        }
    });
}

function PromptBox(Title, NoInputcallback, callback, Param = null) {

    bootbox.prompt({
        title: Title,
        buttons: {
            cancel: {
                label: Shipments.CancelButton
            },
            confirm: {
                label: Shipments.SubmitButton
            }
        },
        centerVertical: true,
        callback: function (Input) {
            if (Input != null) {
                if (Input == '') {

                    NoInputcallback()

                } else {

                    if (Param != null) {

                        callback(Input, Param)

                    } else {

                        callback(Input)

                    }

                }
            }
        }
    });

}

    function IsWaitingShipping(Shipment) {

        return Shipment.Status == Shipments.WaitingShipping;

}

function Print(printSectionId) {
    var innerContents = document.getElementById(printSectionId).innerHTML;
    var popupWinindow = window.open();
    popupWinindow.document.open();
    popupWinindow.document.write('<html><head></head><body onload="window.print()">' + innerContents + '</html>');
    popupWinindow.document.close();
} 





