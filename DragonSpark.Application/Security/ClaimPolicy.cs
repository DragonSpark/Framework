using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security
{
	public class ClaimPolicy : ICommand<AuthorizationOptions>
	{
		readonly string _name, _type;

		public ClaimPolicy(string name, string type)
		{
			_name = name;
			_type = type;
		}

		public void Execute(AuthorizationOptions parameter)
		{
			parameter.AddPolicy(_name, policy => policy.RequireClaim(_type));
		}
	}
}