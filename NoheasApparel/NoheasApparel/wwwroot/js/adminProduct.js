
//declaring data table variable
var dataTBL;

$(document).ready(function () {
    loadTableData();

});

function loadTableData()
{
    dataTBL = $('#dataTBL').DataTable({
        responsive:true,
        'ajax': { url: '/admin/product/getproducts' },
        'columns': [
            //setting up all datas into table columns
            { data: 'productName', "width": "25%" },
            { data: 'productPrice', "width": "15%" },
            { data: 'brand.brandName', "width": "10%" },
            { data: 'gender.genderName', "width": "10%" },
            { data: 'category.categoryName', "width": "10%" },
            { data: 'productColor', "width": "10%" },
            {
                data: 'productID',
                "render": function (data)
                {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/product/addedit?id=${data}" class="btn btn-primary mx-2"><i class="fa-solid fa-pencil"></i>Edit</a>
                      <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="fa-regular fa-trash-can"></i>Delete</a>
                    </div>`
                }, "width": "20%"
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
            })
        }
    })
}


function DeleteCategory(url)
{
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            type: 'DELETE'
                url: url
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success",
               
            });
        }
    });
}