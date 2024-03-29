using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fintrak.CustomerPortal.Application.Users.Commands
{
    public class AcceptTermsCommand : IRequest<BaseResponse<bool>>
	{
	}

	public class AcceptTermsCommandHandler : IRequestHandler<AcceptTermsCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IConfiguration _configuration;

		public AcceptTermsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
		}

		public async Task<BaseResponse<bool>> Handle(AcceptTermsCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var result = await _identityService.AcceptTermsAsync(_currentUserService.UserId);
			if (!result)
			{
				response.Success = false;
				response.Message = "Fail to accept terms and conditions at this time.";
			}
			else
			{
				response.Result = true;
			}

			return response;
		}
	}
}
