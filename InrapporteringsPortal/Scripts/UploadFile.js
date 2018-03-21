var $ = jQuery;

$(document).ready(function () {

    $(function () {
        $('#ddlRegister > option').each(function () {
            $(this).attr("title", $(this).text());
            //alert($(this).text() + ' ' + $(this).val() + ' ' + $(this).attr("title"));
        });
    });

    $('.fileinput-button').hide();
    $('.start').hide();

    window.filelist = [];
    $('#fileupload').fileupload({
        // your fileupload options
    }).on("fileuploadadd",
        function(e, data) {
            for (var i = 0; i < data.files.length; i++) {
                filelist.push(data.files[i]);
            }
            checkOkToUpload();
        })
        .on("fileuploadfail",
        function (e, data) {
            for (var i = 0; i < data.files.length; i++) {
                filelist.splice($.inArray(data.files[i], filelist), 1);
                //filelist.splice(data.files[i]);
            }
            //TODO - detta görs även i jquery.fileupload.js => dubblerad kod, fixa
            checkOkToUpload();
            ////Check if desired number of files reached and no errors found => enable upload
            //var errorExists = false;
            //for (var i = 0; i < filelist.length; i++) {
            //    if (filelist[i].error) {
            //        errorExists = true;
            //    }
            //}
            //var selectedRegister = $('#ddlRegister').val();
            //var numberOfFilesForSelectedRegister = 0;
            ////get number of required files for chosen register
            //registerLista.forEach(function(register, index) {
            //    if (selectedRegister === register.Id.toString()) {
            //        numberOfFilesForSelectedRegister = register.AntalFiler;
            //    }
            //});
            //if (filelist.length === numberOfFilesForSelectedRegister && !errorExists) {
            //    $('.start').prop('disabled', false);
            //    $('.start').show();
            //    $('.fileinput-button')
            //        .prop('disabled', true)
            //        .parent().addClass('disabled');
            //    //this.element.find('.fileinput-button input')
            //    //    .prop('disabled', true)
            //    //    .parent().addClass('disabled');
            //}
            
        })
        .on('fileuploaddone',
        function (e, data) {
            //reset filelist
            filelist = [];
        });

    $('#btnSubmit').click(function() {
        var jqXHR = $('#fileupload').fileupload('send', { files: filelist })
            .success(function (result, textStatus, jqXHR) {
                $("#filTabell tbody tr.template-upload").remove();
                $("#updateHistoryForm").submit();
            })
            .error(function (jqXHR, textStatus, errorThrown) {/* ... */ })
            .complete(function(result, textStatus, jqXHR) {
            });;
    });

});

//$(document).on('click','#testBtn',function() {
//    //alert('Update history, testBtn');
//    //    //$.get("/Home/Contact", null, function (data) {
//    //    //    alert(data);
//    //});

//        $.ajax({
//            // edit to add steve's suggestion.
//            //url: "/ControllerName/ActionName",
//            url: '<%= Url.Action("Contact", "Home") %>',
//            success: function (data) {
//                // your data could be a View or Json or what ever you returned in your action method 
//                // parse your data here
//                alert(data);
//            }
//        });
//});

$(document).on('submit', '#updateHistoryForm', function () {
    $.ajax({ // create an AJAX call...'        
        data: $(this).serialize(), // get the form data
        cache: false,
        type: 'post', // GET or POST
        url: $('#StartUrl').val() + '/FileUpload/RefreshFilesHistory', // the file to call
        success: function (response) { // on success..
            $("#updateHistoryForm").html(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('error:' + errorThrown);
        }
    });
    return false; // cancel original event to prevent form submitting
});


$(document).on('change','#ddlRegister',
    function() {
        var selectedRegister = $('#ddlRegister').val();

        $('#fileupload').fileupload(
            'option',
            'selectedRegister',
            selectedRegister
        );

        filelist = [];
        $("#filTabell tbody tr").remove();
        $("#thText").text("Filer för uppladdning");
        $("#SelectedRegisterId").val(selectedRegister);

        registerLista.forEach(function (register, index) {
            if (selectedRegister === register.Id.toString()) {
                $('#registerInfo').html(register.InfoText);

                //Check if organisation is supposed to leave files per unit
                if (register.RapporterarPerEnhet) {
                    //Populate unit-dropdown
                    var vals = {};
                    register.Organisationsenheter.forEach(function(unit, index) {
                        vals[unit.Key] = unit.Value;
                    });

                    var $ddlUnits = $("#ddlUnits");
                    $ddlUnits.empty();
                    $ddlUnits.append("<option> - Välj enhet - </option>");   
                    $.each(vals, function (index, value) {
                        $ddlUnits.append("<option value=" + index + " >" + value + "</option>");
                        //alert(index + ' : ' + value);
                    });

                    $('#enhetsInfo').show();
                    $('.fileinput-button').hide();
                    $('#fileinputButton').prop('disabled', true);
                    $('#fileinputButton').addClass('disabled');
                    //$('.fileinput-button')
                    //    .prop('disabled', true)
                    //    .parent().addClass('disabled');
                    $('.start').hide();
                } else {
                    $('#enhetsInfo').hide();
                    $('.fileinput-button').show();
                    $('#fileinputButton').prop('disabled', false);
                    $('#fileinputButton').removeClass('disabled');
                    //$('.fileinput-button')
                    //    .prop('disabled', false)
                    //    .parent().removeClass('disabled');
                    $('.start').hide();
                }
            } else if (selectedRegister === ""){
                $('#registerInfo').html("");
                $('.fileinput-button').hide();
                $('.start').hide();
            }
        });

        $(document).on('change', '#ddlUnits', function () {
            var selectedIndex = $('#ddlUnits').prop('selectedIndex');
            $("#SelectedUnitId").val($('#ddlUnits').val());
            //alert($('#ddlUnits').prop('selectedIndex'));
            if (selectedIndex === 0) {
                $('.fileinput-button').hide();
                $('#fileinputButton').prop('disabled', true);
                $('#fileinputButton').addClass('disabled');
                //$('.fileinput-button')
                //    .prop('disabled', true)
                //    .parent().addClass('disabled');
                $('.start').hide();
            } else {
                $('.fileinput-button').show();
                $('#fileinputButton').prop('disabled', false);
                $('#fileinputButton').removeClass('disabled');
                //$('.fileinput-button')
                //    .prop('disabled', false)
                //    .parent().removeClass('disabled');
            }
        });
    });

function checkOkToUpload() {
    //Check if desired number of files reached and no errors found => enable upload
    var errorExists = false;
    for (var i = 0; i < filelist.length; i++) {
        if (filelist[i].error) {
            errorExists = true;
        }
    }
    var selectedRegister = $('#ddlRegister').val();
    var numberOfFilesForSelectedRegister = 0;
    //get number of required files for chosen register
    registerLista.forEach(function (register, index) {
        if (selectedRegister === register.Id.toString()) {
            numberOfFilesForSelectedRegister = register.AntalFiler;
        }
    });
    if (filelist.length === numberOfFilesForSelectedRegister && !errorExists) {
        $('.start').prop('disabled', false);
        $('.start').show();
        $('#fileinputButton').prop('disabled', true);
        $('#fileinputButton').addClass('disabled');
    } else {
        $('.start').prop('disabled', true);
        $('.start').hide();
        $('#fileinputButton').prop('disabled', false);
        $('#fileinputButton').removeClass('disabled');
    }
}

//function disableFileInputButton() {
//    $('.fileinput-button')
//        .prop('disabled', true)
//        .parent().addClass('disabled');
//}

//function isIE() {
//    var ua = window.navigator.userAgent;
//    var msie = ua.indexOf('MSIE '); // IE 10 or older
//    var trident = ua.indexOf('Trident/'); //IE 11

//    return (msie > 0 || trident > 0);
//}
