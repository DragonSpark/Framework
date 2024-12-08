using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class BearerConfiguration : ICommand<JwtBearerOptions>
{
	readonly TokenValidation _validation;

	public BearerConfiguration(TokenValidation validation) => _validation = validation;

	public void Execute(JwtBearerOptions parameter)
	{
		parameter.TokenValidationParameters = _validation;
	}
}