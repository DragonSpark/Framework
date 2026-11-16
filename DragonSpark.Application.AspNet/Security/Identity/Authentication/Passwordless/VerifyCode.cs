using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public sealed class VerifyCode<T> : IVerifyCode<T> where T : class
{
	public static VerifyCode<T> Default { get; } = new();

	VerifyCode() : this(TokenOptions.DefaultEmailProvider) {}

	readonly string _provider;

	public VerifyCode(string provider) => _provider = provider;

	public async ValueTask<bool> Get(VerifyCodeInput<T> parameter)
	{
		var (users, user, purpose, input) = parameter;
		var result = await users.VerifyUserTokenAsync(user, _provider, purpose, input).Off();
		return result;
	}
}