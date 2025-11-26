using DragonSpark.Application.Components.Validation.Expressions;

namespace DragonSpark.Application.Components.Validation;

public sealed class IsAssignedValidator<T> : IValidateValue<T?>
{
	public static IsAssignedValidator<T> Default { get; } = new();

	IsAssignedValidator() {}
	
	public bool Get(T? parameter) => parameter is not null;
}