using DragonSpark.Application;
using DragonSpark.Application.Components.Validation.Expressions;

namespace DragonSpark.Identity.Twitter;

sealed class HandleValidator : RegularExpressionValidator
{
	public static HandleValidator Default { get; } = new();

	HandleValidator() : base(HandlePattern.Default.Bounded().Get(new(1, 15))) {}
}