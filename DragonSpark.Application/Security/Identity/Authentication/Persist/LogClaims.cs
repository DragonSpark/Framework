using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class LogClaims : ICommand<LogClaimsInput>
{
	readonly LogAuthenticationMessage _message;

	public LogClaims(LogAuthenticationMessage message) => _message = message;

	public void Execute(LogClaimsInput parameter)
	{
		var (name, claims) = parameter;
		_message.Execute(name, claims.Open().AsValueEnumerable().Select(x => x.Type).ToArray());
	}
}