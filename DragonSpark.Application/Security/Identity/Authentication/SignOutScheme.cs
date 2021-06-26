using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public sealed class SignOutScheme : IOperation
	{
		readonly IHttpContextAccessor _accessor;
		readonly string               _scheme;

		public SignOutScheme(IHttpContextAccessor accessor) : this(accessor, IdentityConstants.ExternalScheme) {}

		public SignOutScheme(IHttpContextAccessor accessor, string scheme)
		{
			_accessor = accessor;
			_scheme   = scheme;
		}

		public ValueTask Get() => _accessor.HttpContext.Verify().SignOutAsync(_scheme).ToOperation();
	}
}