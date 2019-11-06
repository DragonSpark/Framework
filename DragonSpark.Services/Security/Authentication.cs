using DragonSpark.Model.Selection;
using DragonSpark.Services.Communication;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Services.Security
{
	public sealed class Authentication : Select<HttpRequest, AuthenticationInformation>
	{
		public static Authentication Default { get; } = new Authentication();

		Authentication() : base(AuthenticationAddress.Default.Then()
		                                             .Select(ClientStore.Default)
		                                             .Configure(AuthenticationStateAssignment.Default)
		                                             .Select(Api<IAuthentication>.Default)
		                                             .Request(x => x.Current())
		                                             .Query()
		                                             .Only()) {}
	}
}