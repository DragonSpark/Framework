using DragonSpark.Compose;
using DragonSpark.Services.Communication;

namespace DragonSpark.Services.Security
{
	sealed class AppServiceAuthSession : ResponseState
	{
		const string Name = nameof(AppServiceAuthSession);

		public static AppServiceAuthSession Default { get; } = new AppServiceAuthSession();

		AppServiceAuthSession() : base(Name, Start.A.Selection(new RequestStateValue(Name))
		                                          .Then()
		                                          .Or.UseWhenAssigned(AuthenticationSessionToken.Default.Get)) {}
	}
}