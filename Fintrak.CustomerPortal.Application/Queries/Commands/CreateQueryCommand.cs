using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Domain.Events.Invitations;
using Fintrak.CustomerPortal.Domain.Events.Queries;

namespace Fintrak.CustomerPortal.Application.Queries.Commands
{
	public class CreateQueryCommand : IRequest<BaseResponse<int>>
	{
		public CreateQueryDto Item { get; set; }
	}

	public class CreateQueryCommandValidator : AbstractValidator<CreateQueryCommand>
	{
		public CreateQueryCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
			RuleFor(v => v.Item.ResourceReference).MaximumLength(200).NotEmpty();
			RuleFor(v => v.Item.QueryMessage).MaximumLength(500).NotEmpty();
			RuleFor(v => v.Item.QueryInitiator).MaximumLength(200).NotEmpty();
		}
	}

	public class CreateQueryCommandHandler : IRequestHandler<CreateQueryCommand, BaseResponse<int>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IConfiguration _configuration;
		private readonly IIdentityService _identityService;

		public CreateQueryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_configuration = configuration;
			_identityService = identityService;
		}

		public async Task<BaseResponse<int>> Handle(CreateQueryCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<int>();

			//if (!ValidateCommandHash(request))
			//{
			//	throw new Exception("[CreateQueryCommand] - Request parameters not properly formated.");
			//}

			//var loginId = _currentUserService.UserId;
			var customer = await GetCustomer(request);
			if(customer == null)
			{
				throw new NotFoundException(nameof(Customer), request.Item.ResourceReference);
			}

			if(customer.Status == Domain.Enums.OnboardingStatus.Completed)
			{
				response.Success = false;
				response.Message = "Can't issue query to a customer that has complete onboarding process.";
				return response;
            }

            if (customer.Status == Domain.Enums.OnboardingStatus.Queried)
            {
                response.Success = false;
                response.Message = "Customer already has apending query.";
                return response;
            }

			//get admin info
			var user = await _identityService.GetUserAsync(customer.LoginId);
			if(user == null)
			{
				response.Success = false;
				response.Message = "Unable to load customer's profile.";
				return response;
			}

			var entity = new Query
			{
				ResourceType = GetDomainResourceType(request.Item.ResourceType),
				ResourceReference = request.Item.ResourceReference,
				QueryMessage = request.Item.QueryMessage,
				QueryInitiator = request.Item.QueryInitiator,
				CustomerId = customer.Id,
				IsPending = true,
				PreviousStatus = customer.Status,
				EntryDate = DateTime.Now 
			};

			entity.AddDomainEvent(new QueryCreatedEvent(entity, user.AdminName, user.Email));
			_context.Queries.Add(entity);

			customer.Status = Domain.Enums.OnboardingStatus.Queried;

			await _context.SaveChangesAsync(cancellationToken);

			response.Result = entity.Id;

			return response;
		}

		private Domain.Enums.ResourceType GetDomainResourceType(Blazor.Shared.Models.Enums.ResourceType resourceType)
		{
			if (resourceType == Blazor.Shared.Models.Enums.ResourceType.Customer)
			{
				return Domain.Enums.ResourceType.Customer;
			}
			else if (resourceType == Blazor.Shared.Models.Enums.ResourceType.CustomerProduct)
			{
				return Domain.Enums.ResourceType.CustomerProduct;
			}
			else
			{
				throw new NotImplementedException("ResourceType not implemented.");
			}
		}

		private bool ValidateCommandHash(CreateQueryCommand command)
		{
			var hashMode = _configuration["HashSettings:Mode"].ToString();
			var hashKey = _configuration["HashSettings:Key"].ToString();

			var hashInput = $"{command.Item.ResourceType}{command.Item.ResourceReference}{command.Item.QueryInitiator}";

			if (hashMode == "Sha256")
			{
				return HashUtility.ValidateSha256Hash(command.Item.Hash, hashInput);
			}
			else if (hashMode == "MD5")
			{
				return HashUtility.ValidateMD5Hash(command.Item.Hash, hashInput);
			}
			else
			{
				throw new NotImplementedException("Hash mode not implemented.");
			}
		}

		private async Task<Customer> GetCustomer(CreateQueryCommand request)
		{
			Customer customer = default!;

			if (request.Item.ResourceType == Blazor.Shared.Models.Enums.ResourceType.Customer)
			{
				var customerId = int.Parse(request.Item.ResourceReference);
				customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
			}
			else if (request.Item.ResourceType == Blazor.Shared.Models.Enums.ResourceType.CustomerProduct)
			{
				var customerProductId = int.Parse(request.Item.ResourceReference);
				var productCustomer = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.Id == customerProductId);

				if(productCustomer != null)
				{
					customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == productCustomer.CustomerId);
				}
			}

			return customer;
		}

	}
}
