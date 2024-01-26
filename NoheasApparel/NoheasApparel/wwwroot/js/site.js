

// Initialization for ES Users
/*import { Carousel, initMDB } from "mdb-ui-kit";*/

/*initMDB({ Carousel });*/

// navbar button
$(document).ready(function () {

    $('.navbarrr').on('click', function () {

        $('.animated-icon1').toggleClass('open');
    });
});


    var messageTimer = setTimeout(function () {

        $('#alertMSG').fadeOut("slow");
    }, 5000)



    //yes or no confirmation for deletion
var clickedID;

$("#btnModal").on('show.bs.modal', function (e)
{
    $(e.currentTarget).find('asp-route-id').val(getID);
    var button = $(this).find('#modalDeleteButton');
    console.log(clickedID)
    button.attr('formation', '/admin/category/delete/' + clickedID)
});

function getID(clicked_id)
{
    this.clickedID = clicked_id;
}


