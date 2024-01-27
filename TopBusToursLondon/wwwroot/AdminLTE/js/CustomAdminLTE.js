// Add the following code if you want the name of the file appear on select
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
$(document).ready(function () {
    var table = $('#DT_load').DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth":false,
        "language": {
            "emptyTable": "no data found"
        },
        dom: 'Bfrtip',
        buttons: [

            'copy', 'excel', 'csv', 'pdf', 'print'
        ]
    });
    table.buttons().container().appendTo($("#printbar"));
});