using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Forms
{
	// TODO: Remove
	public sealed class ValidationProvider : ValidatorBase
	{
		[Parameter, UsedImplicitly]
		public IFieldValidator Validator { get; set; } = default!;

		public override string? Text { get; set; }

		protected override bool Validate(IRadzenFormComponent component)
		{
			var (result, message) = Validator.Get(component.GetValue());
			Text = message;
			return result;
		}
	}
}