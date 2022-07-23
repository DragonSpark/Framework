﻿using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class SecurityDescriptor : ISelect<IDictionary<string, object>, SecurityTokenDescriptor>
{
	readonly BearerSettings     _settings;
	readonly SigningCredentials _credentials;
	readonly IResult<DateTime>  _expires;

	public SecurityDescriptor(BearerSettings settings, BearerSigningCredentials credentials)
		: this(settings, credentials.Get(), ExpiresTomorrow.Default) {}

	public SecurityDescriptor(BearerSettings settings, SigningCredentials credentials, IResult<DateTime> expires)
	{
		_settings    = settings;
		_credentials = credentials;
		_expires     = expires;
	}

	public SecurityTokenDescriptor Get(IDictionary<string, object> parameter) => new()
	{
		Claims = parameter,
		Issuer             = _settings.Issuer,
		Audience           = _settings.Audience,
		Expires            = _expires.Get(),
		SigningCredentials = _credentials
	};
}