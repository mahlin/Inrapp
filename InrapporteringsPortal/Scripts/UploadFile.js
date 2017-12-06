var $ = jQuery;

$(document).ready(function () {

    
    $('.fileinput-button').hide();
    $('.start').hide();
    //$('.start').prop('disabled', true);

    var x = $('#FilAntal').val();


    //$(window).load(function () {

    //});

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
            $('.fileinput-button').show();
            $('.start').show();
            $('.start').prop('disabled', true);
            break;
        case "2":
            $('#registerInfo').html(regInfoTexts[1].Value);
            $('.fileinput-button').show();
            $('.start').show();
            $('.start').prop('disabled', true);
            break;
        case "3":
            $('#registerInfo').html(regInfoTexts[2].Value);
            $('.fileinput-button').show();
            $('.start').show();
            $('.start').prop('disabled', true);
            break;
        default:
            $('#registerInfo').html("");
            $('.fileinput-button').hide();
            $('.start').hide();
        }
        
    });

function disableFileInputButton() {
    this.element.find('.fileinput-button input')
        .prop('disabled', true)
        .parent().addClass('disabled');
}
