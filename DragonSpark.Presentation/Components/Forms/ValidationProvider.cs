using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class ValidationProvider : ValidatorBase
	{
		[Parameter, UsedImplicitly]
		public IFieldValidator? Validator { get; set; }

		public override string Text { get; set; } = default!;

		protected override bool Validate(IRadzenFormComponent component)
		{
			var result = Validator.Verify().Get(component.GetValue());
			Text = result.Message.Verify();
			return result.Valid;
		}
	}
}