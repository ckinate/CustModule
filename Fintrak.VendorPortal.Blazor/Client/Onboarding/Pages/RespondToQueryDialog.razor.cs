using Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.VendorPortal.Blazor.Shared.Models.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Reflection.Metadata;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Pages
{
	public partial class RespondToQueryDialog
	{
        [Inject]
        public IOnboardingService? OnboardingService { get; set; }

        private DotNetObjectReference<RespondToQueryDialog> dotNetRef;

        public bool ShowDialog { get; set; }

        private EditContext EditContext;
		public ResponseToQueryDto PageModel { get; set; } = new();
        public ResponseToQueryValidator PageModelValidator { get; set; }

        public string QueryMessage { get; set; }

        [Parameter]
		public EventCallback OnQueryResponse { get; set; }

        protected override async Task OnInitializedAsync()
        {

			dotNetRef = DotNetObjectReference.Create(this);

            PageModel = new();

		}

        public void Show(QueryDto query)
		{
			ShowDialog = true;
			BlockPage();

            QueryMessage = query.QueryMessage;
            PageModel = new ResponseToQueryDto
			{
				QueryId = query.Id.Value
			};

			PageModelValidator = new();

            //EditContext = new EditContext(PageModel);

            //EditContext.OnFieldChanged += HandleFieldChanged;

            UnBlockPage();
			StateHasChanged();
		}

        private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            StateHasChanged();
        }

        async Task OnSaveClick()
		{
			SpinnerService.Show();

			var response = await OnboardingService.ResponseToQuery(PageModel);

			SpinnerService.Hide();

			if (response.Success)
			{
                ShowDialog = false;
                StateHasChanged();

                await OnQueryResponse.InvokeAsync();
            }
			else
			{

			}		
		}

		public void Close()
		{
			ShowDialog = false;
			StateHasChanged();
		}
	}
}
