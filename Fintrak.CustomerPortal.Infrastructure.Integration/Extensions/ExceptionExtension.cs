using System;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Extensions
{
    public static class ExceptionExtension
    {
        public static string GetDetails(this Exception ex)
        {
            string error = $"Details: {ex.Message} <br/>";

            error += BuildErrors(ex);

            return error;
        }

        private static string BuildErrors(Exception ex)
        {
            string error = $"{ex.Message} <br/>";

            if (ex.InnerException is not null)
                error += $"{BuildErrors(ex.InnerException)} <br/>";

            return error;
        }
    }
}
