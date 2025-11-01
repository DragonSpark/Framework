using DragonSpark.Application.Components.Validation.Expressions;
using System;

namespace DragonSpark.Application.Components.Validation;

public sealed class DifferentValueValidator : IValidateValue<string>
{
	readonly string           _other;
	readonly StringComparison _comparison;

	public DifferentValueValidator(string other,
	                               StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		_other      = other;
		_comparison = comparison;
	}

	public bool Get(string parameter) => !_other.Equals(parameter, _comparison);
}

// TODO

public sealed class IsAssignedValidator<T> : IValidateValue<T?>
{
	public static IsAssignedValidator<T> Default { get; } = new();

	IsAssignedValidator() {}
	
	public bool Get(T? parameter) => parameter is not null;
}