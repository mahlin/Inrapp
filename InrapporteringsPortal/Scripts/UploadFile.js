var $ = jQuery;

$(document).ready(function () {

    $(window).load(function() {
    });

});

$(document).on('change','#ddlRegister',
    function () {
        var selectedRegister = $('#ddlRegister').val();
        //var selectText = $('#ddlRegister :selected').text();
        //Sätt info-text beroende av vilket register som valts
        //$.each(regInfoTexts, function (index, item) {
        //    // access the properties of each user
        //    alert("Nyckel" + item.Key + "Värde" + item.Value);
        //});
        switch (selectedRegister) {
        case "1":
            $('#registerInfo').html(regInfoTexts[0].Value);
            break;
        case "2":
            $('#registerInfo').html(regInfoTexts[1].Value);
            break;
        case "3":
            $('#registerInfo').html(regInfoTexts[2].Value);
            break;
        default:
            $('#registerInfo').html(regInfoTexts[0].Value);
        }
    });



//function SelectedIndexChanged()
//{
//    //Form post  
//    document.demoForm.submit();  
//}
