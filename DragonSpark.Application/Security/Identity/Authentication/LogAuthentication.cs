using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class LogAuthentication : ICommand<LogAuthenticationInput>
{
	readonly LogAuthenticationMessage _message;

	public LogAuthentication(LogAuthenticationMessage message) => _message = message;

	public void Execute(LogAuthenticationInput parameter)
	{
		var (name, claims) = parameter;
		_message.Execute(name, claims.Open().AsValueEnumerable().Select(x => $"{x.Issuer}:{x.Type}").ToArray());
	}
}