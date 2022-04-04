using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class BearerConfiguration : ICommand<JwtBearerOptions>
{
	readonly Func<BearerSettings>    _settings;
	readonly ISelect<string, byte[]> _data;

	public BearerConfiguration(Func<BearerSettings> settings) : this(settings, EncodedTextAsData.Default) {}

	public BearerConfiguration(Func<BearerSettings> settings, ISelect<string, byte[]> data)
	{
		_settings = settings;
		_data     = data;
	}

	public void Execute(JwtBearerOptions parameter)
	{
		var settings = _settings();
		var key      = _data.Get(settings.Key);
		parameter.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer           = true,
			ValidateAudience         = true,
			ValidateLifetime         = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer              = settings.Issuer,
			ValidAudience            = settings.Audience,
			IssuerSigningKey         = new SymmetricSecurityKey(key)
		};
	}
}