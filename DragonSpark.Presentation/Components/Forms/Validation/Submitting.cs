using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class Submitting : ReferenceValueStore<EditContext, DragonSpark.Model.Results.Switch>
{
	public static Submitting Default { get; } = new();

	Submitting() : base(_ => new()) {}
}