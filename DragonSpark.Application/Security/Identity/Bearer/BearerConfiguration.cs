using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class BearerConfiguration : ICommand<JwtBearerOptions>
{
	readonly Func<TokenValidation>   _validation;

	public BearerConfiguration(Func<TokenValidation> validation) => _validation = validation;

	public void Execute(JwtBearerOptions parameter)
	{
		parameter.TokenValidationParameters = _validation();
	}
}