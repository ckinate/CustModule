var invoiceInterop = invoiceInterop || {};

invoiceInterop.init = function () {
	var table = $('#kt_datatable');

	// begin first table
	table.DataTable({
		responsive: true,

		// DOM Layout settings
		dom: `<'row'<'col-sm-12'tr>>
			<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

		lengthMenu: [5, 10, 25, 50],

		pageLength: 10,

		language: {
			'lengthMenu': 'Display _MENU_',
		},

		// Order settings
		order: [[1, 'desc']],
		columnDefs: [],
	});

};