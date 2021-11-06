using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Components.Validation;

public sealed class StandardAdjustedInput : Alteration<string>
{
	public static StandardAdjustedInput Default { get; } = new();

	StandardAdjustedInput() : base(x => x.Trim().ToLower()) {}
}