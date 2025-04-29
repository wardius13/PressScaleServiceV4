$(function () {
    $("#jsGrid").jsGrid({
        width: "60%",
        height: "1200px",

        heading: true,
        filtering: false,
        inserting: false, 
        editing: false,   
        deleting: false,  
        sorting: true,
        paging: true,
        autoload: true,

        pageSize: 50,
        pageIndex: 1,

        controller: {
            loadData: function () {
                return $.ajax({
                    type: "GET",
                    url: "https://localhost:44363/api/meetdata",
                    dataType: "json"
                });
            }
        },

        fields: [
            { title: "Tijdstip", name: "Tijdstip", type: "text", width: 200 },
            { title: "Gewicht (kg)", name: "Gewicht", type: "number", width: 80 },
            { title: "Aantal Balen", name: "AantalBalen", type: "number", width: 80 },
            { title: "Materiaal ID", name: "MateriaalID", type: "number", width: 80 }
        ],

        noDataContent: "Geen data gevonden",
        loadMessage: "Even geduld..."
    });
});
