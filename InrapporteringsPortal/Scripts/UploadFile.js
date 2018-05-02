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

                //Check if obligatory for this org to report for this register
                if (!register.Obligatorisk) {
                    var periodLista = register.Perioder;
                    addSelect("select-container", register.Perioder);
                    $('#ingetAttRapportera').show();
                } else {
                    $('#ingetAttRapportera').hide();
                }

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
                    $('#fileinputButton').parent().addClass('disabled');
                    $('#fileinputButton').prop('readonly', true);
                    $('#fileinputButton').addClass('readonly');
                    //$('.fileinput-button')
                    //    .prop('disabled', true)
                    //    .parent().addClass('disabled');
                    $('.start').hide();
                } else {
                    $('#enhetsInfo').hide();
                    $('.fileinput-button').show();
                    $('#fileinputButton').prop('disabled', false);
                    $('#fileinputButton').removeClass('disabled');
                    $('#filesExplorerOpener').prop('disabled', false);
                    $('#filesExplorerOpener').removeClass('disabled');
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
                $('#filesExplorerOpener').prop('disabled', true);
                $('#filesExplorerOpener').addClass('disabled');
                //$('.fileinput-button')
                //    .prop('disabled', true)
                //    .parent().addClass('disabled');
                $('.start').hide();
            } else {
                $('.fileinput-button').show();
                $('#fileinputButton').prop('disabled', false);
                $('#fileinputButton').removeClass('disabled');
                $('#filesExplorerOpener').prop('disabled', false);
                $('#filesExplorerOpener').removeClass('disabled');
                //$('.fileinput-button')
                //    .prop('disabled', false)
                //    .parent().removeClass('disabled');
            }
        });

        $(document).on('change', '#ddlPerioder', function () {
            var x = $('#ddlPerioder').val();
            var y = $('#SelectedRegisterId').val();
            alert("x: " + x);
            alert("y: " + y);
            $("#IngetAttRapporteraForPeriod").val($('#ddlPerioder').val());
            $("#IngetAttRapporteraForRegisterId").val($('#SelectedRegisterId').val());
            var z = $("#IngetAttRapporteraForPeriod").val();
            var n = $("#IngetAttRapporteraForRegisterId").val();
            alert("z: " + x);
            alert("n: " + y);
        });

    });


function addSelect(divname, perioder) {
    $('#IngetAttRapporteraForPeriod').val(perioder[0]);
    var newDiv = document.createElement('div');
    var html = ' <span style="white-space: nowrap">Inget att rapportera för period: &nbsp;&nbsp;<select id="ddlPerioder" class="form-control" style="width:95px;display:inline-block;padding-left:10px;">', i;
    for (i = 0; i < perioder.length; i++) {
        html += "<option value='" + perioder[i] + "'>" + perioder[i] + "</option>";
    }
    html += '</select></span>';
    newDiv.innerHTML = html;
    document.getElementById(divname).innerHTML = newDiv.innerHTML;
    //document.getElementById(divname).appendChild(newDiv);
}

function dateGenerate() {
    var date = new Date(), dateArray = new Array(), i;
    curYear = date.getFullYear();
    for (i = 0; i < 5; i++) {
        dateArray[i] = curYear + i;
    }
    return dateArray;
}

//function periodGenerate() {
//    var date = new Date(), dateArray = new Array(), i;
//    curYear = date.getFullYear();
//    for (i = 0; i < 5; i++) {
//        dateArray[i] = curYear + i;
//    }
//    return dateArray;
//}

function checkIfDisabled(event) {
    //alert('click egen check');
    var x = $('#fileinputButton');
    var y = $('#filesExplorerOpener');
    //if ($('#fileinputButton').attr('disabled', 'true')) {
    //    return false;
    //}
    if ($('#fileinputButton').hasClass('disabled')) {
        $('#filesExplorerOpener').prop('disabled', true);
        $('#filesExplorerOpener').addClass('disabled');
        return false;
    } else {
        $('#filesExplorerOpener').prop('disabled', false);
        $('#filesExplorerOpener').removeClass('disabled');

    }
    return true;
}

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
        $('#filesExplorerOpener').prop('disabled', true);
        $('#filesExplorerOpener').addClass('disabled');
    } else {
        $('.start').prop('disabled', true);
        $('.start').hide();
        $('#fileinputButton').prop('disabled', false);
        $('#fileinputButton').removeClass('disabled');
        $('#filesExplorerOpener').prop('disabled', false);
        $('#filesExplorerOpener').removeClass('disabled');
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
