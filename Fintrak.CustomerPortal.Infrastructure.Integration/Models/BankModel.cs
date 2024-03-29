using System.Collections.Generic;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Models
{
    public class BankListModel
    {
        public List<BankModel> Result { get; set; }

        public bool Success { get; set; }
    }

    public class BankModel
    {
        public int Id { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
    }
}
