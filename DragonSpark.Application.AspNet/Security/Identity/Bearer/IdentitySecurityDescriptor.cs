using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class IdentitySecurityDescriptor : ISelect<ClaimsIdentity, SecurityTokenDescriptor>
{
	readonly BearerSettings     _settings;
	readonly SigningCredentials _credentials;
	readonly IResult<DateTime>  _expires;

	public IdentitySecurityDescriptor(BearerSettings settings, BearerSigningCredentials credentials)
		: this(settings, credentials.Get(), ExpiresTomorrow.Default) {}

	public IdentitySecurityDescriptor(BearerSettings settings, SigningCredentials credentials,
	                                  IResult<DateTime> expires)
	{
		_settings    = settings;
		_credentials = credentials;
		_expires     = expires;
	}

	public SecurityTokenDescriptor Get(ClaimsIdentity parameter) => new()
	{
		Subject            = parameter,
		Issuer             = _settings.Issuer,
		Audience           = _settings.Audience,
		Expires            = _expires.Get(),
		SigningCredentials = _credentials
	};
}