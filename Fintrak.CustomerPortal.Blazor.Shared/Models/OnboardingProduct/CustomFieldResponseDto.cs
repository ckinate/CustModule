using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct
{
    public class CustomFieldResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public int Order { get; set; }

        public bool IsCompulsory { get; set; }

        public string Response { get; set; }
    }
}
