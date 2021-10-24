using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class StandardAdjustedInput : Alteration<string>
{
	public static StandardAdjustedInput Default { get; } = new();

	StandardAdjustedInput() : base(x => x.Trim().ToLower()) {}
}