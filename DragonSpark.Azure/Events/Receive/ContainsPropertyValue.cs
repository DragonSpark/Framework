using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Azure.Events.Receive;

public class ContainsPropertyValue : ICondition<ProcessEventArgs>
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

	public bool Get(ProcessEventArgs parameter)
		=> string.Equals(parameter.Data.Properties.TryGetValue(_property, out var property) ? property as string : null,
		                 _value, _comparison);
}