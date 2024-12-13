using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class IsValid : Condition<EditContext>
{
	public static IsValid Default { get; } = new();

	IsValid() : base(x => !x.GetValidationMessages().AsValueEnumerable().Any()) { }
}