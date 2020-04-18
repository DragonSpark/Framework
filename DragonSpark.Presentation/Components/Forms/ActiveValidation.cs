using DragonSpark.Model.Properties;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class ActiveValidation : ValidatorBase
	{
		readonly IProperty<object, bool> _active;

		public ActiveValidation() : this(IsActive.Default) {}

		public ActiveValidation(IProperty<object, bool> active) => _active = active;

		[Parameter]
		public IComponent Context { get; set; }

		protected override bool Validate(IRadzenFormComponent component) => !_active.Get((object)Context ?? Form);

		public override string Text { get; set; } = "The form is currently performing an operation.";
	}
}
