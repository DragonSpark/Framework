using DragonSpark.Application.Security.Identity.Model.Claims;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class AppliedPrincipal : IAppliedPrincipal
	{
		readonly IConditional<string, Array<IExternalClaim>> _claims;

		[UsedImplicitly]
		public AppliedPrincipal(IEnumerable<IClaimRegistration> registrations)
			: this(registrations.GroupBy(x => x.Get())
			                    .AsValueEnumerable()
			                    .ToDictionary(x => x.Key, x => x.Result<IExternalClaim>())
			                    .ToStore()) {}

		public AppliedPrincipal(IConditional<string, Array<IExternalClaim>> claims) => _claims = claims;

		public ClaimsPrincipal Get(ExternalLoginInfo parameter)
		{
			if (_claims.Condition.Get(parameter.LoginProvider))
			{
				var claims = _claims.Get(parameter.LoginProvider)
				                    .Open()
				                    .Get(parameter)
				                    .Where(x => x != null)
				                    .ToArray();

				var identity = new ClaimsIdentity(claims);
				var result   = new ClaimsPrincipal(parameter.Principal.Identities.Append(identity).Open());
				return result;
			}

			return parameter.Principal;
		}
	}
}
