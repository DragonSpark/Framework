using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions;

public class RegularExpressionValidator : MetadataValueValidator<object>
{
	public RegularExpressionValidator(string expression) : this(new RegularExpressionAttribute(expression)) {}

	public RegularExpressionValidator(RegularExpressionAttribute metadata) : base(metadata) {}
}

public class RegularExpressionValidator<T> : MetadataValueValidator<T>
{
    public RegularExpressionValidator(string expression) : this(new RegularExpressionAttribute(expression)) {}

    public RegularExpressionValidator(RegularExpressionAttribute metadata) : base(metadata) {}
}