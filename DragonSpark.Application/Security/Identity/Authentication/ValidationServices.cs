using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class ValidationServices : IValidationServices
	{
		readonly IAdapters                 _adapters;
		readonly IAuthenticationValidation _validation;

		public ValidationServices(IAdapters adapters, IAuthenticationValidation validation)
		{
			_adapters   = adapters;
			_validation = validation;
		}

		public Task<AuthenticationState> Get(Task<AuthenticationState> parameter) => _adapters.Get(parameter);

		public ValueTask<bool> Get(ClaimsPrincipal parameter) => _validation.Get(parameter);
	}
}