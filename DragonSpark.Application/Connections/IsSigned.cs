using DragonSpark.Application.Communication;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DragonSpark.Application.Connections;

public sealed class IsSigned : ICondition<HttpContext>
{
	readonly ParseToken _parse;
	readonly IHeader    _header;
	readonly Claim      _expected;

	public IsSigned(ParseToken parse)
		: this(parse, SignedTokenHeader.Default, new(SignedTokenClaimName.Default, ExpectedTokenContent.Default)) {}

	public IsSigned(ParseToken parse, IHeader header, Claim expected)
	{
		_parse    = parse;
		_header   = header;
		_expected = expected;
	}

	public bool Get(HttpContext parameter)
	{
		var headers   = parameter.Request.Headers;
		var token     = _header.Get(headers).Verify();
		var principal = _parse.Get(token);
		var result    = principal.HasClaim(_expected.Type, _expected.Value);
		return result;
	}
}