//This file is to override the CDN toolbar settings.
CKEDITOR.editorConfig = function(config) {
    config.toolbarGroups = [
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'links', groups: ['links'] },
        { name: 'insert', groups: ['insert'] },
        { name: 'styles', groups: ['styles'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'others', groups: ['others'] },
        { name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'Source,Save,Templates,NewPage,Preview,Print,HiddenField,ImageButton,Button,Select,Textarea,TextField,Radio,Checkbox,Form,Strike,Subscript,Superscript,RemoveFormat,Language,Image,Flash,Table,HorizontalRule,Smiley,SpecialChar,PageBreak,Iframe,ShowBlocks,Maximize,About';
};

function Private_Checked() {
    $("#hotelFilter").click(function() {
        $('#CfirstAndOr').attr("disabled", $(this).is(':checked'));
    });
}

function Private_Checked2() {
    $("#travelFilter").click(function() {
        $('#secondAndOr').attr("disabled", $(this).is(':checked'));
    });
}

$(document).ready(function () {

    $('#table').DataTable({
        "pagingType": "full_numbers",
        columnDefs: [{
            targets: [0],
            orderData: [0, 1]
        }, {
            targets: [1],
            orderData: [1, 0]
        }, {
            targets: [4],
            orderData: [4, 0]
        }]
    });
});