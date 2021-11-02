using DragonSpark.Model.Commands;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

public class CopyClaims : ICommand<Transfer>
{
	readonly IExtractClaims _extract;

	protected CopyClaims(IEnumerable<string> names) : this(new ClaimExtractor(names)) {}

	protected CopyClaims(IExtractClaims extract) => _extract = extract;

	public void Execute(Transfer parameter)
	{
		var (from, to) = parameter;

		var claims = _extract.Get(from);

		if (claims.Length > 0)
		{
			to.AddIdentity(new ClaimsIdentity(claims.Open()));
		}
	}
}