// Call the dataTables jQuery plugin
$(document).ready(function() {
    var oTable = $('#dataTable').DataTable();
    oTable.fnPageChange('first',false);
    
});
