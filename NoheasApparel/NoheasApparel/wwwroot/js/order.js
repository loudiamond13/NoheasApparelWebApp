

//declaring data table variable
var dataTBL;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("Completed")) {
        loadTableData("Order?status=Completed");
    }
    else if (url.includes("Pending"))
    {
        loadTableData("Order?status=Pending");
    }
    else if (url.includes("Inprogress")) {
        loadTableData("Order?status=Inprogress");
    }
    else if (url.includes("Rejected")) {
        loadTableData("Order?status=Rejected");
    }
    else  {
        loadTableData("Order?status=All");
    }
});

function loadTableData(url)
{
    dataTBL = $('#dataTBL').DataTable({
        responsive:true,
        'ajax': { url: '/admin/order/' + url },
        'columns': [
            //setting up all datas into table columns
            { data: 'orderHeaderID', "width": "10%" },
            { data: 'name', "width": "15%" },
            { data: 'noheasApparelUser.email', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'orderTotal', "width": "10%" },
            { data: 'orderStatus', "width": "10%" },
            {
                data: 'orderHeaderID',
                "render": function (data)
                {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/order/details/${data}" class="btn btn-primary mx-2"><i class="fa-solid fa-pencil"></i>Details</a>
                    </div>`
                }, "width": "10%"
            }
        ]
    });


}





