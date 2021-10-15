using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Model;

public class Value : IValue
{
	readonly string _name;

	protected Value(string name) => _name = name;

	public object Get(IValueProvider parameter) => new Dictionary<string, string?>
	{
		[_name] = parameter.Get(_name)
	};
}