using Fintrak.CustomerPortal.Application.Common.Interfaces;

namespace Fintrak.CustomerPortal.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
