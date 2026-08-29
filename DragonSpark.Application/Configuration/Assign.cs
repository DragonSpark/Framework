using Microsoft.Extensions.Configuration;
using System;

namespace DragonSpark.Application.Configuration;

public class Assign : IAssign
{
	readonly string                  _name;
	readonly Action<string, string?> _assign;

	protected Assign(string name) : this(name, Environment.SetEnvironmentVariable) {}

	protected Assign(string name, Action<string, string?> assign)
	{
		_name   = name;
		_assign = assign;
	}

	public void Execute(IConfiguration parameter)
	{
		_assign(_name, parameter[_name]);
	}
}