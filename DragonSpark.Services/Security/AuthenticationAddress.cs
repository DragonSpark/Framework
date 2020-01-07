using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;
using DragonSpark.Services.Communication;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Services.Security
{
	sealed class AuthenticationAddress : ReferenceValueStore<HttpRequest, Uri>
	{
		public static AuthenticationAddress Default { get; } = new AuthenticationAddress();

		AuthenticationAddress() : base(Start.An.Instance<CurrentRequestUri>()
		                                    .Select(Authority.Default)
		                                    .Select(Uris.Default)
		                                    .Unless(A.Of<Uris>()
		                                             .Assigned()
		                                             .Then()
		                                             .Bind(A.Of<AuthenticationBaseAddress>().Get)
		                                           )) {}
	}
}