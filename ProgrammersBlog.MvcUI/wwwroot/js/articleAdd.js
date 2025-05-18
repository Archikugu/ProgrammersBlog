$(document).ready(function () {
    //trumbowyg
    $('#text-editor').trumbowyg({
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['foreColor', 'backColor'],
            ['emoji'],
            ['fontfamily'],
            ['fontsize'],
            ['fullscreen']
        ]
    });
    //trumbowyg

    //TomSelect
    new TomSelect('#categoryList', {
        placeholder: "Please select a category...",
        allowEmptyOption: true,
        create: false
    });
    //TomSelect

    //Datepicker
    $(function () {
        $("#datepicker").datepicker({
            duration: 1000,
            maxDate: +3,
            minDate: -3,
            dateFormat: "dd.mm.yy",
        });
    });
    //Datepicker
});
