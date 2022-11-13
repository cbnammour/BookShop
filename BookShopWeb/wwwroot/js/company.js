﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $("#tablData").DataTable({
        "ajax": {
            "url": "/Admin/Company/getall"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a class="btn btn-primary mx-2" href="/Admin/Company/Upsert?id=${data}"><i class="bi bi-pencil-square"></i> &nbsp; Edit</a>
                            <a onClick=Delete('/Admin/Company/Delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> &nbsp; Delete</a>
                        </div>
                    `
                },
                "width": "15%"
            },
        ]
    });

}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.sucess(data.message);
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}