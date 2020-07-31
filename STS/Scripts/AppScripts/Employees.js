$(document).ready(function () {
    var EmployeesDataTableLocale = {
        emptyTable: Employees.EmployeesEmptyTable,
        sInfo: Employees.EmployeesTableInfo,
        sInfoEmpty: "",
        lengthMenu: Employees.EmployeesTableLengthMenu,
        loadingRecords: Employees.EmployeesTableLoadingRecords,
        processing: Employees.EmployeesTableProcessing,
        search: Employees.EmployeesTableSearchLabel,
        zeroRecords: Employees.EmployeesEmptyTable,
        paginate: {
            first: Employees.EmployeesTableFirst,
            last: Employees.EmployeesTableLast,
            next: Employees.EmployeesTableNext,
            previous: Employees.EmployeesTablePrevious
        }
    };

    var LocationId = $("#Employees").attr("LocationId");

    var EmployeesDataTableAjaxInfo = {
        url: LocationId != null ? "/Employees/LoadData?LocationId=" + LocationId : "/Employees/LoadData",
        type: 'POST',
        datatype: "json"
    };

    var EmployeesDataTableColumnDefinition = [
        {
            targets: [0],
            orderable: false,
            searchable: false
        }
    ];

    var EmployeesDataTableColumn = [
        {
            data: "Name",
            render: EmployeesDataTableRenderFunction
        }
    ];

    $("#Employees").DataTable({
        language: EmployeesDataTableLocale,
        proccessing: true,
        serverSide: true,
        ajax: EmployeesDataTableAjaxInfo,
        columnDefs:EmployeesDataTableColumnDefinition,
        columns: EmployeesDataTableColumn
    });

    $("#Employees").on("click", ".Remove", function () {
        var Id = $(this).attr('Id');
        var Email = $(this).attr('Email');
        ConfirmBox(Employees.RemoveEmployeeMessage.replace(/_Email_/g, Email), EmployeeRemoveAjaxRequest, Id, Email);
    });

});

function EmployeesDataTableRenderFunction(data, type, Employee) {
    Col = Employee.Name + '<br>' + Employee.Email + '<br>' + Employee.Location + '<br>' +
        '<div class="btn-group drop" >' +
        '<button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></button>' +
        '<div class="dropdown-menu">' +
        '<a class="dropdown-item" href="/Employees/Update/' + Employee.id + '">' + Employees.EmployeesTableUpdateInformationLabel + '</a>' +
        '<a class="dropdown-item" href="/Employees/Details/' + Employee.id + '">' + Employees.EmployeesTableDetailsLabel + '</a>' +
        '<a class="dropdown-item Remove" href="#" Id="' + Employee.id + '" Email="' + Employee.Email + '" >' + Employees.EmployeesTableRemoveEmployeeLabel + '</a>' +
        '</div>' +
        '</div>';
    return Col;
}

function EmployeeRemoveAjaxRequest(Id) {
    $.ajax({
        url: '/Operations-api/Employees/Remove/' + Id ,
        method: "POST",
        success: AjaxRequestOperationSuccess,
        error: AjaxRequestError
    });
}

function AjaxRequestOperationSuccess(Message) {
    $('#Employees').DataTable().ajax.reload();
    toastr.success(Message);
}

function AjaxRequestError(xhr) {
    if (xhr.status == 404) {
        toastr.error(Employees.EmployeeNotFound);
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
                label: Employees.CancelButton
            },
            confirm: {
                label: Employees.ConfirmButton
            }
        },
        callback: function (confirm) {
            if (confirm) {
                callback(Param1 , Param2);
            }
        }
    });
}










