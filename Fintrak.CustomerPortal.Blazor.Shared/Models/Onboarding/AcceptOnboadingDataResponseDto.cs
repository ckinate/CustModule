using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{
	public class AcceptOnboadingDataResponseDto
	{
		public int BackendId { get; set; }

		public string Code { get; set; }

		public string CustomCode { get; set; }

		public CustomerStage Stage { get; set; }
	}
}
