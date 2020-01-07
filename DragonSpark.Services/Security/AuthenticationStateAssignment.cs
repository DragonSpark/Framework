using DragonSpark.Compose;
using DragonSpark.Services.Communication;

namespace DragonSpark.Services.Security
{
	sealed class AuthenticationStateAssignment : ResponseStateAssignment
	{
		public static AuthenticationStateAssignment Default { get; } = new AuthenticationStateAssignment();

		AuthenticationStateAssignment() : base(RequestStateSelector.Default
		                                                           .Select(AppServiceAuthSession.Default)
		                                                           .Get) {}
	}
}