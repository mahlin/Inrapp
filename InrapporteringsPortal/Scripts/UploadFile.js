﻿var $ = jQuery;

$(document).ready(function() {


    $('.fileinput-button').hide();
    $('.start').hide();

    var filelist = [];
    $('#fileupload').fileupload({
        // your fileupload options
    }).on("fileuploadadd",
        function(e, data) {
            for (var i = 0; i < data.files.length; i++) {
                filelist.push(data.files[i]);
            }
        });

    $('#btnSubmit').click(function() {
        var jqXHR = $('#fileupload').fileupload('send', { files: filelist })
            .success(function (result, textStatus, jqXHR) {
                $("#filTabell tbody tr.template-upload").remove();
            })
            .error(function (jqXHR, textStatus, errorThrown) {/* ... */ })
            .complete(function(result, textStatus, jqXHR) {
            });;
    });

    $(window).load(function () {

    });

});


$(document).on('change','#ddlRegister',
    function () {
        var selectedRegister = $('#ddlRegister').val();
        //$('.cancel').trigger("click");
        $("#filTabell tbody tr").remove();
        $("#thText").text("Filer för uppladdning");
        $("#SelectedRegisterId").val(selectedRegister);
        switch (selectedRegister) {
        case "1":
            $('#registerInfo').html(regInfoTexts[0].Value);
            $('.fileinput-button').show();
            $('.fileinput-button input')
                .prop('disabled', false)
                .parent().removeClass('disabled');
            $('.start').show();
            $('.start').prop('disabled', true);
            break;
        case "2":
            $('#registerInfo').html(regInfoTexts[1].Value);
            $('.fileinput-button').show();
            $('.fileinput-button input')
                .prop('disabled', false)
                .parent().removeClass('disabled');
            $('.start').show();
            $('.start').prop('disabled', true);
            break;
        case "3":
            $('#registerInfo').html(regInfoTexts[2].Value);
            $('.fileinput-button').show();
            $('.fileinput-button input')
                .prop('disabled', false)
                .parent().removeClass('disabled');
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
