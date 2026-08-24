using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class BearerConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
	readonly ApplicationTokenValidation _validation;

	public BearerConfiguration(ApplicationTokenValidation validation) => _validation = validation;

	public void Configure(string? name, JwtBearerOptions options)
	{
		options.TokenValidationParameters = _validation;
	}

	public void Configure(JwtBearerOptions options) => Configure(Options.DefaultName, options);
}