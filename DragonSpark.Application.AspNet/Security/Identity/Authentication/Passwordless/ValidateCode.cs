using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

sealed class ValidateCode<T> : IDepending<ValidateSignInInput<T>> where T : class
{
	public static ValidateCode<T> Default { get; } = new();

	ValidateCode() : this(VerifyCode<T>.Default) {}

	readonly IVerifyCode<T> _verify;

	public ValidateCode(IVerifyCode<T> verify) => _verify = verify;

	public async ValueTask<bool> Get(ValidateSignInInput<T> parameter)
	{
		var ((_, users), (user, input, _, _)) = parameter;
		var result = await _verify.Off(new(users, user, input));
		if (result && user is IdentityUser { EmailConfirmed: false } u)
		{
			u.EmailConfirmed = true;
			u.Modified       = Time.Default;
			await users.UpdateAsync(user).Off();
		}

		return result;
	}
}