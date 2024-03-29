using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Application.Common.Extensions
{
	public static class EnumExtensions
    {
        public static OnboardingStatus GetCustomerStatus(this Domain.Enums.OnboardingStatus status)
        {
            if (status == Domain.Enums.OnboardingStatus.NotStarted)
                return OnboardingStatus.NotStarted;
            else if (status == Domain.Enums.OnboardingStatus.Submitted)
                return OnboardingStatus.Submitted;
            else if (status == Domain.Enums.OnboardingStatus.Processing)
                return OnboardingStatus.Processing;
            else if (status == Domain.Enums.OnboardingStatus.Queried)
                return OnboardingStatus.Queried;
            else if (status == Domain.Enums.OnboardingStatus.Completed)
                return OnboardingStatus.Completed;
            else
                throw new NotImplementedException();
        }

        public static Domain.Enums.OnboardingStatus GetCustomerDomainStatus(this OnboardingStatus status)
        {
            if (status == OnboardingStatus.NotStarted)
                return Domain.Enums.OnboardingStatus.NotStarted;
            else if (status == OnboardingStatus.Submitted)
                return Domain.Enums.OnboardingStatus.Submitted;
            else if (status == OnboardingStatus.Processing)
                return Domain.Enums.OnboardingStatus.Processing;
            else if (status == OnboardingStatus.Queried)
                return Domain.Enums.OnboardingStatus.Queried;
            else if (status == OnboardingStatus.Completed)
                return Domain.Enums.OnboardingStatus.Completed;
            else
                throw new NotImplementedException();
        }

        public static string GetCustomerStatusDisplay(this Domain.Enums.OnboardingStatus status)
        {
            if (status == Domain.Enums.OnboardingStatus.NotStarted)
                return "Not Started";
            else if (status == Domain.Enums.OnboardingStatus.Submitted)
                return "Submitted";
            else if (status == Domain.Enums.OnboardingStatus.Processing)
                return "Processing";
            else if (status == Domain.Enums.OnboardingStatus.Queried)
                return "Queried";
            else if (status == Domain.Enums.OnboardingStatus.Completed)
                return "Completed";
            else
                throw new NotImplementedException();
        }

		public static OnboardingProductStatus GetProductStatus(this Domain.Enums.OnboardingProductStatus status)
		{
			if (status == Domain.Enums.OnboardingProductStatus.NotStarted)
				return OnboardingProductStatus.NotStarted;
			else if (status == Domain.Enums.OnboardingProductStatus.Submitted)
				return OnboardingProductStatus.Submitted;
			else if (status == Domain.Enums.OnboardingProductStatus.Processing)
				return OnboardingProductStatus.Processing;
			else if (status == Domain.Enums.OnboardingProductStatus.Queried)
				return OnboardingProductStatus.Queried;
			else if (status == Domain.Enums.OnboardingProductStatus.Completed)
				return OnboardingProductStatus.Completed;
			else
				throw new NotImplementedException();
		}

		public static Domain.Enums.OnboardingProductStatus GetProductDomainStatus(this OnboardingProductStatus status)
		{
			if (status == OnboardingProductStatus.NotStarted)
				return Domain.Enums.OnboardingProductStatus.NotStarted;
			else if (status == OnboardingProductStatus.Submitted)
				return Domain.Enums.OnboardingProductStatus.Submitted;
			else if (status == OnboardingProductStatus.Processing)
				return Domain.Enums.OnboardingProductStatus.Processing;
			else if (status == OnboardingProductStatus.Queried)
				return Domain.Enums.OnboardingProductStatus.Queried;
			else if (status == OnboardingProductStatus.Completed)
				return Domain.Enums.OnboardingProductStatus.Completed;
			else
				throw new NotImplementedException();
		}

		public static string GetProductStatusDisplay(this Domain.Enums.OnboardingProductStatus status)
		{
			if (status == Domain.Enums.OnboardingProductStatus.NotStarted)
				return "Not Started";
			else if (status == Domain.Enums.OnboardingProductStatus.Submitted)
				return "Submitted";
			else if (status == Domain.Enums.OnboardingProductStatus.Processing)
				return "Processing";
			else if (status == Domain.Enums.OnboardingProductStatus.Queried)
				return "Queried";
			else if (status == Domain.Enums.OnboardingProductStatus.Completed)
				return "Completed";
			else
				throw new NotImplementedException();
		}

		public static Domain.Enums.StaffSize GetDomainStaffSize(this StaffSize size)
        {
            if (size == StaffSize.LessThanTen)
                return Domain.Enums.StaffSize.LessThanTen;
            else if (size == StaffSize.LessThanFifty)
                return Domain.Enums.StaffSize.LessThanFifty;
            else if (size == StaffSize.AboveFifty)
                return Domain.Enums.StaffSize.AboveFifty;
            else
                throw new NotImplementedException();
        }

        public static StaffSize GetDomainStaffSize(this Domain.Enums.StaffSize size)
        {
            if (size == Domain.Enums.StaffSize.LessThanTen)
                return StaffSize.LessThanTen;
            else if (size == Domain.Enums.StaffSize.LessThanFifty)
                return StaffSize.LessThanFifty;
            else if (size == Domain.Enums.StaffSize.AboveFifty)
                return StaffSize.AboveFifty;
            else
                throw new NotImplementedException();
        }

        public static Domain.Enums.AccountType GetDomainAccountType(this AccountType type)
        {
            if (type == AccountType.Fee)
                return Domain.Enums.AccountType.Fee;
            else if (type == AccountType.Commission)
                return Domain.Enums.AccountType.Commission;
            else
                throw new NotImplementedException();
        }

        public static AccountType GetDomainAccountType(this Domain.Enums.AccountType type)
        {
            if (type == Domain.Enums.AccountType.Fee)
                return AccountType.Fee;
            else if (type == Domain.Enums.AccountType.Commission)
                return AccountType.Commission;
            else
                throw new NotImplementedException();
        }
    }
}
