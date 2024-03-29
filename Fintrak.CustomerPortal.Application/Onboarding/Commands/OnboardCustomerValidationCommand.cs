using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;

namespace Fintrak.CustomerPortal.Application.Onboarding.Commands
{
	public class OnboardCustomerValidationCommand : IRequest<BaseResponse<bool>>
	{
		public int CurrentStep { get; set; }
		public OnboardCustomerDto Item { get; set; }
    }
	public class OnboardCustomerValidationCommandHandler : IRequestHandler<OnboardCustomerValidationCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;

		public OnboardCustomerValidationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
		}

		public async Task<BaseResponse<bool>> Handle(OnboardCustomerValidationCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();
			response.Result = false;

			Customer entity = null;

			var user = await _identityService.GetUserAsync(_currentUserService.UserId);
			if (user == null)
			{
				response.Success = false;
				response.Message = "Unable to load customer's profile.";
				return response;
			}

			//process step 1
			var step1Response = await ProcessStep1(request, user, cancellationToken);
			
			return step1Response;
		}

		public async Task<BaseResponse<bool>> ProcessStep1(OnboardCustomerValidationCommand request, UserDto user, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool> ();
			response.Result = true;
			response.Success = true;
			response.Message = $"Validation successful";

			var entity = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == _currentUserService.UserId);

			if (request.CurrentStep == 1)
			{
				var entityWithEmailExist = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Item.Email);
				var entityWithRCExist = await _context.Customers.FirstOrDefaultAsync(c => c.RegistrationCertificateNumber == request.Item.RegistrationCertificateNumber);
				var entityWithTinExist = await _context.Customers.FirstOrDefaultAsync(c => c.TaxIdentificationNumber == request.Item.TaxIdentificationNumber);
				var entityWithPhoneExist = await _context.Customers.FirstOrDefaultAsync(c => c.OfficePhoneCallCode == request.Item.OfficePhoneCallCode && c.OfficePhoneNumber == request.Item.OfficePhoneNumber);
				var entityWithMobileExist = await _context.Customers.FirstOrDefaultAsync(c => c.MobilePhoneCallCode == request.Item.MobilePhoneCallCode && c.MobilePhoneNumber == request.Item.MobilePhoneNumber);

				if (entity == null)
				{
					if (entityWithEmailExist != null)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with email '{request.Item.Email}' already exist.";

						return response;
					}

					if (entityWithRCExist != null)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with RC '{request.Item.RegistrationCertificateNumber}' already exist.";

						return response;
					}

					if (entityWithTinExist != null)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with Tin '{request.Item.TaxIdentificationNumber}' already exist.";

						return response;
					}

					if (entityWithPhoneExist != null)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with Office Phone '{request.Item.OfficePhoneCallCode}-{request.Item.OfficePhoneNumber}' already exist.";

						return response;
					}

					if (entityWithMobileExist != null)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with Mobile Phone '{request.Item.MobilePhoneCallCode}-{request.Item.MobilePhoneNumber}' already exist.";

						return response;
					}
				}
				else
				{
					if (entityWithEmailExist != null && entityWithEmailExist.Id != entity.Id)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with email '{request.Item.Email}' already exist.";

						return response;
					}

					if (entityWithRCExist != null && entityWithRCExist.Id != entity.Id)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with RC '{request.Item.RegistrationCertificateNumber}' already exist.";

						return response;
					}

					if (entityWithTinExist != null && entityWithTinExist.Id != entity.Id)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with Tin '{request.Item.TaxIdentificationNumber}' already exist.";

						return response;
					}

					if (entityWithPhoneExist != null && entityWithPhoneExist.Id != entity.Id)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with Office Phone '{request.Item.OfficePhoneCallCode}-{request.Item.OfficePhoneNumber}' already exist.";

						return response;
					}

					if (entityWithMobileExist != null && entityWithMobileExist.Id != entity.Id)
					{
						response.Result = false;
						response.Success = false;
						response.Message = $"Customer with Mobile Phone '{request.Item.MobilePhoneCallCode}-{request.Item.MobilePhoneNumber}' already exist.";

						return response;
					}
				}
			}

			return response;
		}
	}

}
