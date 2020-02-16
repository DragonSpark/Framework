using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security
{
	public class ClaimValuesPolicy : ICommand<AuthorizationOptions>
	{
		readonly string        _name;
		readonly string        _type;
		readonly Array<string> _values;

		public ClaimValuesPolicy(string name, string type, params string[] values)
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

	public class ClaimPolicy : ICommand<AuthorizationOptions>
	{
		readonly string _name;
		readonly string _type;

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