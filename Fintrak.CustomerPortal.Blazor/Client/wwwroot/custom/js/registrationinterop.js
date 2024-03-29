var registrationInterop = registrationInterop || {};

registrationInterop.init = function (dotNetRef) {

	// Base elements
	var _wizardEl;
	var _formEl;
	var _wizardObj;
	var _validations = [];

	_wizardEl = KTUtil.getById('kt_wizard');
	_formEl = KTUtil.getById('kt_form');

	// Initialize form wizard
	_wizardObj = new KTWizard(_wizardEl, {
		startStep: 1, // initial active step number
		clickableSteps: false // to make steps clickable this set value true and add data-wizard-clickable="true" in HTML for class="wizard" element
	});

	// Validation before going to next page
	_wizardObj.on('change', function (wizard) {

		console.log('change getStep: ' + wizard.getStep());
		console.log('change getNewStep: ' + wizard.getNewStep());

		if (wizard.getStep() > wizard.getNewStep()) {
			return; // Skip if stepped back
		}

		dotNetRef.invokeMethodAsync("ValidateStep", wizard.getStep())
			.then(data => {
				if (data.valid) {
					wizard.goTo(wizard.getNewStep());
					KTUtil.scrollTop();
				}
				else {
					console.log(data);
					var errors = data.errors.join('\n');
					console.log('Errors:' + errors);

					var message = "Sorry, looks like there are some validation errors detected, please fill the form properly.\n" + errors;
					console.log('Message:' + message);

					Swal.fire({
						text: message,
						icon: "error",
						buttonsStyling: false,
						confirmButtonText: "Ok, got it!",
						customClass: {
							confirmButton: "btn font-weight-bold btn-light"
						}
					}).then(function () {
						KTUtil.scrollTop();
					});
				}
			});

		

		return false;  // Do not change wizard step, further action will be handled by he validator
	});

	// Change event
	_wizardObj.on('changed', function (wizard) {

		console.log('changed getStep: ' + wizard.getStep());
		console.log('changed getNewStep: ' + wizard.getNewStep());

		dotNetRef.invokeMethodAsync("SetCurrentStep", wizard.getStep(), wizard.getNewStep())
			.then(data => { });

		KTUtil.scrollTop();
	});

	// Submit event
	_wizardObj.on('submit', function (wizard) {
		Swal.fire({
			text: "All is good! Please confirm the form submission.",
			icon: "success",
			showCancelButton: true,
			buttonsStyling: false,
			confirmButtonText: "Yes, submit!",
			cancelButtonText: "No, cancel",
			customClass: {
				confirmButton: "btn font-weight-bold btn-primary",
				cancelButton: "btn font-weight-bold btn-default"
			}
		}).then(function (result) {
			if (result.value) {
				//_formEl.submit(); // Submit form

				dotNetRef.invokeMethodAsync("SubmitData")
					.then(data => {
						//if (data) {
							
						//}
						//else {
						//	Swal.fire({
						//		text: "Sorry, looks like there are some errors detected, please try again.",
						//		icon: "error",
						//		buttonsStyling: false,
						//		confirmButtonText: "Ok, got it!",
						//		customClass: {
						//			confirmButton: "btn font-weight-bold btn-light"
						//		}
						//	}).then(function () {
						//		KTUtil.scrollTop();
						//	});
						//}
					});

			} else if (result.dismiss === 'cancel') {
				Swal.fire({
					text: "Your form has not been submitted!.",
					icon: "error",
					buttonsStyling: false,
					confirmButtonText: "Ok, got it!",
					customClass: {
						confirmButton: "btn font-weight-bold btn-primary",
					}
				});
			}
		});
	});
};

registrationInterop.initSelectPicker = function () {
	$('#kt_select2_3').select2({
		placeholder: "Select a state",
	});
};
