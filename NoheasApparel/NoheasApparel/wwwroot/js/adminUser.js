
//declaring data table variable
var dataTBL;

$(document).ready(function () {
    loadTableData();

});

function loadTableData()
{
    dataTBL = $('#dataTBL').DataTable({
        responsive:true,
        'ajax': { url: '/admin/user/getusers' },
        'columns': [
            //setting up all datas into table columns
            { data: 'name', "width": "15%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'role', "width": "15%" },
          
            {
                data:
                {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockoout = new Date(data.lockoutEnd).getTime();
                    if (lockoout > today) {
                        //user is currently locked
                        return `<div class="w-75 btn-group" role="group"> 
                                    <a onClick=UnlocklLock('${data.id}') class="btn btn-danger mx-2"> <i class="fa-solid fa-lock-open"></i> Unlock</a>
                                </div>`;
                    }
                    else {
                        return `<div class="w-75 btn-group" role="group"> 
                                    <a onClick=UnlocklLock('${data.id}') class="btn btn-success mx-2"><i class="fa-solid fa-lock"></i> Lock</a>
                                </div>`;
                    }
                },"width": "25%"
                    //return `<div class="w-75 btn-group" role="group">
                    //<a href="/admin/product/addedit?id=${data}" class="btn btn-primary mx-2"><i class="fa-solid fa-pencil"></i>Edit</a>
                    //  <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="fa-regular fa-trash-can"></i>Delete</a>
                    //</div>`
                    //}, "width": "20%"
                
            }
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
                    dataTBL.ajax.reload();


                }
            });
        }
    })
}


function UnlocklLock(id)
{
    $.ajax({
        url: '/admin/user/UnlockLock',
        type: 'POST',
        data: JSON.stringify(id),
        contentType:'application/json',
        success: function (data) {
            if (data.success)
            {
                dataTBL.ajax.reload();
            }


        }
    });
}