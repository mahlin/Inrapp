/*
 * jQuery File Upload Validation Plugin 1.1.3
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2013, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* global define, require, window */

function CheckFileName(selectedRegister, fileName) {
    switch (selectedRegister) {
        case "1":
            var re = new RegExp(regFilMasks[0].Value, "i");
            if (re.test(fileName)) {
                return true;
            } else {
                return false;
            }
        case "2":
            var re = new RegExp(regFilMasks[1].Value, "i");
            if (re.test(fileName)) {
                return true;
            } else {
                return false;
            }
        case "3":
            var tmp = regFilMasks[2].Value;
            var re = new RegExp("\\LSS_.");
            if (match = re.exec(fileName))
                return true;
            else
                return false;
        default:
            return false;
    }
    return true;
}

//TODO - använd SelectedRegisterId istället?
function CheckKommunKodInFileName(fileName) {
    var chunkedFileName = fileName.split("_");
    var fileTypeA = [ 'SOL1', 'SOL2', 'KHSL1', 'KHSL2'];
    var fileTypeB = ['BU'];
    var fileTypeC = ['EKB'];
    
    if (fileTypeA.includes(chunkedFileName[0].toUpperCase())) {
        return CheckKommunKod(chunkedFileName[1]);
    }
    else if (fileTypeB.includes(chunkedFileName[0].toUpperCase())) {
        return CheckKommunKod(chunkedFileName[2]);
    }
    else if (fileTypeC.includes(chunkedFileName[0].toUpperCase())) {
        if (chunkedFileName[1].toUpperCase() === 'AO')
            return CheckKommunKod(chunkedFileName[2]);
        else
            return CheckKommunKod(chunkedFileName[1]);
    }
}

function CheckKommunKod(kommunKod) {
    var validKommunKod = $('#GiltigKommunKod').val();
    if (validKommunKod === kommunKod)
        return true;
    return false;
}

(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // Register as an anonymous AMD module:
        define([
            'jquery',
            './jquery.fileupload-process'
        ], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS:
        factory(require('jquery'));
    } else {
        // Browser globals:
        factory(
            window.jQuery
        );
    }
}(function ($) {
    'use strict';

    // Append to the default processQueue:
        $.blueimp.fileupload.prototype.options.processQueue.push(
            {
                action: 'validate',
                // Always trigger this action,
                // even if the previous action was rejected: 
                always: true,
                // Options taken from the global options map:
                acceptFileTypes: '@',
                maxFileSize: '@',
                minFileSize: '@',
                maxNumberOfFiles: '@',
                incorrectFileName: '@',
                incorrectKommunKodInFileName: '@',
            disabled: '@disableValidation'
        }
    );

    // The File Upload Validation plugin extends the fileupload widget
    // with file validation functionality:
    $.widget('blueimp.fileupload', $.blueimp.fileupload, {

        options: {
            
            // The regular expression for allowed file types, matches
            // against either file type or file name:
            acceptFileTypes: /(\.|\/)(txt|xls)$/i,
            // The maximum allowed file size in bytes:
            maxFileSize: 1000000000, // 1000 MB = 1GB
            // The minimum allowed file size in bytes:
            minFileSize: 1, // No minimal file size
            // The limit of files to be uploaded:
            //maxNumberOfFiles: 10,
            

            // Function returning the current number of files,
            // has to be overriden for maxNumberOfFiles validation:
            getNumberOfFiles: $.noop,

            // Error and info messages:
            messages: {
                maxNumberOfFiles: 'Maximalt antal filer har överskridits',
                acceptFileTypes: 'Felaktig filtyp',
                maxFileSize: 'Filen är för stor',
                minFileSize: ('Filen är tom'),
                incorrectFileName: ('Felaktigt filnamn'),
                incorrectKommunKodInFileName: ('Felaktig kommunkod i filnamnet')
            }
        },

        processActions: {

            validate: function (data, options) {
                if (options.disabled) {
                    return data;
                }
                var dfd = $.Deferred(),
                    settings = this.options,
                    file = data.files[data.index],
                    fileSize;
                if (options.minFileSize || options.maxFileSize) {
                    fileSize = file.size;
                }
                if ($.type(options.maxNumberOfFiles) === 'number' &&
                        (settings.getNumberOfFiles() || 0) + data.files.length >
                            options.maxNumberOfFiles) {
                    file.error = settings.i18n('maxNumberOfFiles');
                } else if (options.acceptFileTypes &&
                        !(options.acceptFileTypes.test(file.type) ||
                        options.acceptFileTypes.test(file.name))) {
                    file.error = settings.i18n('acceptFileTypes');
                } else if (fileSize > options.maxFileSize) {
                    file.error = settings.i18n('maxFileSize');
                } else if ($.type(fileSize) === 'number' &&
                    fileSize < options.minFileSize) {
                    file.error = settings.i18n('minFileSize');
                } else if (!CheckFileName(data.selectedRegister, file.name)) {
                    file.error = settings.i18n('incorrectFileName');
                } else if (!CheckKommunKodInFileName(file.name)) {
                    file.error = settings.i18n('incorrectKommunKodInFileName');
                } else {
                    delete file.error;
                }
                if (file.error || data.files.error) {
                    data.files.error = true;
                    dfd.rejectWith(this, [data]);
                } else {
                    dfd.resolveWith(this, [data]);
                }
                return dfd.promise();
            }

        }

    });

    }

));



