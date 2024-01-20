var updatedRow;
var table;
var datatable;
var exportedCol = [];
//SweetAlert
function showAlert(message = "Saved Successsfully") {

    Swal.fire({
        icon: "success",
        title: "success...",
        text: message,
        customClass: {
            confirmButton: "btn btn-primary"
        }

    });
}
function ShowErrorAlert(message = "Something went wrong!") {
    Swal.fire({
        icon: "error",
        title: "Oops...",
        text: message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}
//Modal
function onModalBegin() {
    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
}

function onModalSuccess(row) {
    showAlert();
    $('#modal').modal("hide");

    if (updatedRow !== undefined) {
        datatable.row(updatedRow).remove().draw();
        updatedRow = undefined;
    }

    var newRow = $(row);
    datatable.row.add(newRow).draw();

    KTMenu.init();
    KTMenu.initHandlers();
}
function onModalComplete() {
    $('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
}
//Datatables
var headers = $('th');
$.each(headers, function (i) {
    if (!$(this).hasClass('js-no-export'))
        exportedCol.push(i);


})
// Class definition
var KTDatatables = function () {

    var initDatatable = function () {


        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-datatables').data('doucment-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();

$(document).ready(function () {

    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });

    let message = $('#message').text().trim()
    if (message.length > 0) {
        showAlert();
    }

    /*start Modal */
    $('body').delegate('.js-render-modal', 'click', function () {
        let btn = $(this)
        let modal = $('#modal')

        modal.find('#modalLabel').text(btn.data('title'))

        if (btn.data('update') != undefined) {
            updatedRow = btn.parents('tr');
        }
        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                ShowErrorAlert();
            }
        })

        modal.modal('show')
    })
    /*End Modal */



    /* start Toogle action*/

    $('body').delegate('.js-toggle-status', 'click', function () {
        let btn = $(this)


        bootbox.confirm({
            message: 'Are you sure for toggling status of this category ?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {

                    $.post({

                        url: btn.data('url'),
                        success: function (lastUpdatedOn) {
                            let status = btn.parents('tr').find('.js-status').text().trim()
                            let newStatus = status === 'Available' ? 'Deleted' : 'Available'

                            btn.parents('tr').find('.js-status').html(newStatus).toggleClass('badge-light-danger badge-light-success')
                            btn.parents('tr').find('.js-update-date').html(lastUpdatedOn);
                            btn.parents('tr').addClass('animate__animated animate__flash')

                            setTimeout(function () {
                                btn.parents('tr').removeClass('animate__animated animate__flash')
                            }, 2000)
                            showAlert("Category is toggled successfully");
                        },
                        error: function () {

                        }

                    })
                }
            }
        });


    })

    /*End toggle action */

})







