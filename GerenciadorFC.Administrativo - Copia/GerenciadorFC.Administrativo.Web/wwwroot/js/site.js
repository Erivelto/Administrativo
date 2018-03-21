// Write your JavaScript code.
(function ($) {
	var generateCustomerTable = $("#dataGrid")
		.dataTable({
			"processing": true,			
			"ordering": true,
			"paging": true,		
			"language": {
				"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json"
			},
			"bLengthChange": false,
			"pageLength": 20
		});
})(jQuery);