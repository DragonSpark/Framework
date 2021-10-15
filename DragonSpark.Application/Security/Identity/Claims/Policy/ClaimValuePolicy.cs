using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

public class ClaimValuePolicy : ICommand<AuthorizationOptions>
{
	readonly string _name, _type, _value;

	public ClaimValuePolicy(string name, string type, string value)
	{
		_name  = name;
		_type  = type;
		_value = value;
	}

	public void Execute(AuthorizationOptions parameter)
	{
		parameter.AddPolicy(_name, policy => policy.RequireClaim(_type, _value));
	}
}