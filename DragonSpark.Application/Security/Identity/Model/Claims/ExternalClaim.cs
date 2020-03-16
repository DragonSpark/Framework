using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model.Claims
{
	sealed class ExternalClaim : IExternalClaim
	{
		readonly string _source, _type;

		public ExternalClaim(string source, string type)
		{
			_source = source;
			_type   = type;
		}

		public Claim Get(ExternalLoginInfo parameter) => new Claim(_type, parameter.Principal.FindFirstValue(_source));
	}
}