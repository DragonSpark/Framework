using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Collections.Generic;

namespace DragonSpark.Application.Connections;

sealed class SignToken : Formatter<HttpConnectionOptions>
{
	public SignToken(SecurityDescriptorFormatter formatter)
		: this(formatter, new Dictionary<string, object>
		{
			[SignedTokenClaimName.Default] = ExpectedTokenContent.Default.Get()
		}) {}

	public SignToken(SecurityDescriptorFormatter formatter, IDictionary<string, object> claims)
		: base(formatter.Then().Bind(claims).Any()) {}
}