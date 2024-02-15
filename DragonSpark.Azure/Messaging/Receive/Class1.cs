using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;

namespace DragonSpark.Azure.Messaging.Receive;

class Class1;

public sealed class ContainsIntendedAudience : ContainsPropertyValue
{
	public ContainsIntendedAudience(string? audience) : base(IntendedAudience.Default, audience) {}
}

public class ContainsPropertyValue : ICondition<IReadOnlyDictionary<string, object>>
{
	readonly string           _property;
	readonly string?          _value;
	readonly StringComparison _comparison;

	protected ContainsPropertyValue(string property, string? value,
	                                StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		_property   = property;
		_value      = value;
		_comparison = comparison;
	}

	public bool Get(IReadOnlyDictionary<string, object> parameter)
		=> string.Equals(parameter.TryGetValue(_property, out var property) ? property as string : null, _value,
		                 _comparison);
}