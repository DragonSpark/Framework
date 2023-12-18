using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class StateViewMemoryKey : IFormatter<ClaimsPrincipal>
{
	public static StateViewMemoryKey Default { get; } = new();

	StateViewMemoryKey()
		: this(IsApplicationPrincipal.Default, StateViewKey.Default, AuthenticationIdentifier.Default) {}

	readonly ICondition<ClaimsPrincipal>       _application;
	readonly IFormatter<uint>                  _key;
	readonly ISelect<ClaimsPrincipal, string?> _identifier;

	public StateViewMemoryKey(ICondition<ClaimsPrincipal> application, IFormatter<uint> key,
	                          ISelect<ClaimsPrincipal, string?> identifier)
	{
		_application = application;
		_key         = key;
		_identifier  = identifier;
	}

	public string Get(ClaimsPrincipal parameter)
		=> _application.Get(parameter) ? _key.Get(parameter.Number().Value()) : _identifier.Get(parameter).Verify();
}