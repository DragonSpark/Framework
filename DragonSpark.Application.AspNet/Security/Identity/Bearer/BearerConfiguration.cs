using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class BearerConfiguration : ICommand<JwtBearerOptions>
{
	readonly ApplicationTokenValidation _validation;

	public BearerConfiguration(ApplicationTokenValidation validation) => _validation = validation;

	public void Execute(JwtBearerOptions parameter)
	{
		parameter.TokenValidationParameters = _validation;
	}
}