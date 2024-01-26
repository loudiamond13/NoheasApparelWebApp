


function validateInput() {
    let carrier = document.getElementById("carrier").value;
    let trackNum = document.getElementById("trackingNumber").value;

    
    if (carrier.toString == ''){
        Swal.fire("Error", "Please Enter A Tracking Number", "error")
        return false;
    }
    if (trackNum.toString() == '') {
        Swal.fire("Error", "Please Enter A Tracking Number", "error")
        return false;
    }
    else{
            return true;
    }

   

}


$(document).ready(function () {

    var shipDate = document.getElementById("shipDate");
    //check if shipDate value is '1/1/0001', clear it
    if (shipDate == '1/1/0001') {
        shipDate.value = "";

    }

    var payDate = document.getElementById("payDate");
    //check if payDate value is '1/1/0001', clear it
    if (payDate.value == '1/1/0001') {
        payDate.value = "";
    }

    var payDueDate = document.getElementById("payDueDate");
    if (payDueDate.value == '1/1/0001') {
        payDate.value = "";
    }
});