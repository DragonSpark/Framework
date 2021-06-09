using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class CreateAction<T> : ICreateAction where T : IdentityUser
	{
		readonly ICreate<T>                            _create;
		readonly IExternalSignin                       _signin;
		readonly ISelect<SignInResult, IdentityResult> _adapter;

		public CreateAction(ICreate<T> create, IExternalSignin signin)
			: this(create, signin, AuthenticationResult.Default) {}

		public CreateAction(ICreate<T> create, IExternalSignin signin, ISelect<SignInResult, IdentityResult> adapter)
		{
			_create  = create;
			_signin  = signin;
			_adapter = adapter;
		}

		public async ValueTask<IdentityResult> Get(ExternalLoginInfo parameter)
		{
			var (_, call) = await _create.Get(parameter);
			var result = call.Succeeded ? _adapter.Get(await _signin.Await(parameter)) : call;
			return result;
		}
	}

	sealed class AuthenticationResult : ISelect<SignInResult, IdentityResult>
	{
		public static AuthenticationResult Default { get; } = new AuthenticationResult();

		AuthenticationResult() {}

		public IdentityResult Get(SignInResult parameter)
			=> parameter.Succeeded
				   ? IdentityResult.Success
				   : IdentityResult.Failed(new IdentityError
				   {
					   Description = parameter.IsLockedOut
						                 ? "User is Locked Out"
						                 : parameter.IsNotAllowed
							                 ? "Authentication is Not Allowed for this user"
							                 : "Two Factor Authentication is Required"
				   });
	}
}