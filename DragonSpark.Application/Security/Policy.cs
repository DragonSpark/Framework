using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security
{
	public class Policy : ICommand<AuthorizationOptions>
	{
		readonly string        _name;
		readonly string        _type;
		readonly Array<string> _values;

		public Policy(string name, string type, params string[] values)
		{
			_name   = name;
			_type   = type;
			_values = values;
		}

		public void Execute(AuthorizationOptions parameter)
		{
			parameter.AddPolicy(_name, policy => policy.RequireClaim(_type, _values));
		}
	}
}
