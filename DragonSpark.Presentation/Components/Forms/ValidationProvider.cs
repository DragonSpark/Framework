using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class ValidationProvider : ValidatorBase
	{
		[Parameter, UsedImplicitly]
		public IFieldValidator Validator { get; set; } = default!;

		public override string? Text { get; set; }

		protected override bool Validate(IRadzenFormComponent component)
		{
			var result = Validator.Get(component.GetValue());
			Text = result.Message;
			return result.Valid;
		}
	}
}