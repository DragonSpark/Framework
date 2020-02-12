using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Testing.Objects
{
	sealed class ExternalSignIn : IExternalSignin
	{
		public static ExternalSignIn Default { get; } = new ExternalSignIn();

		ExternalSignIn() {}

		public ValueTask<SignInResult> Get(ExternalLoginInfo parameter) => SignInResult.Success.ToOperation();
	}
}
