$(document).ready(function () {
  

    //TomSelect
    new TomSelect('#categoryList', {
        placeholder: "Please select a category...",
        allowEmptyOption: true,
        create: false
    });
    
    new TomSelect('#filterByList', {
        placeholder: "Please select a filter type...",
        allowEmptyOption: true,
        create: false
    });

    new TomSelect('#orderByList', {
        placeholder: "Please select a order type...",
        allowEmptyOption: true,
        create: false
    });

    new TomSelect('#isAscendingList', {
        placeholder: "Please select a ascending type...",
        allowEmptyOption: true,
        create: false
    });
    //TomSelect

    //Datepicker
    $(function () {
        $("#startAtDatePicker").datepicker({
            duration: 1000,
            maxDate: 0,
            dateFormat: "dd.mm.yy",
        });
        $("#endAtDatePicker").datepicker({
            duration: 1000,
            maxDate: 0,
            dateFormat: "dd.mm.yy",
        });
    });
    //Datepicker
});
