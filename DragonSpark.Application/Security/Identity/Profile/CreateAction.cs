using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class CreateRequest<T> : ICreateRequest where T : IdentityUser
	{
		readonly ICreate<T>                                    _create;
		readonly ISelecting<ExternalLoginInfo, IdentityResult> _select;

		public CreateRequest(ICreate<T> create, IExternalSignin signin)
			: this(create, signin.Then().Select(IdentityResults.Default).Out()) {}

		public CreateRequest(ICreate<T> create, ISelecting<ExternalLoginInfo, IdentityResult> select)
		{
			_create = create;
			_select = select;
		}

		public async ValueTask<IdentityResult> Get(ExternalLoginInfo parameter)
		{
			var (_, call) = await _create.Get(parameter);
			var result = call.Succeeded ? await _select.Await(parameter) : call;
			return result;
		}
	}

	sealed class IdentityResults : ISelect<SignInResult, IdentityResult>
	{
		public static IdentityResults Default { get; } = new IdentityResults();

		IdentityResults() {}

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