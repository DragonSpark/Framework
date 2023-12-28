using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

public class AdditionalClaims : IKnownClaims
{
	readonly IKnownClaims  _previous;
	readonly Array<string> _additional;

	protected AdditionalClaims(IKnownClaims previous, Array<string> additional)
	{
		_previous   = previous;
		_additional = additional;
	}

	public IEnumerable<string> Get() => _previous.Get().Append(_additional);
}