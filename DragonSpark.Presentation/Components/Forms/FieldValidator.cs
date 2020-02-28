using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	/// <summary>
	/// ATTRIBUTION: https://www.nuget.org/packages/Radzen.Blazor/
	/// </summary>
	public class FieldValidator : RadzenComponent, ICommand
	{
		readonly Func<Task> _validate;

		public FieldValidator() => _validate = Validate;

		[Parameter, UsedImplicitly]
		public bool Popup { get; set; }

		[Parameter, UsedImplicitly]
		public List<FieldValidationDefinition> Definitions { get; set; }

		[Parameter, UsedImplicitly]
		public string Component { get; set; }

		public FieldIdentifier Identifier { get; private set; }

		[Inject]
		public ILogger<FieldValidator> Logger { get; [UsedImplicitly]set; }

		[CascadingParameter, UsedImplicitly]
		IRadzenForm Form { get; set; }

		[CascadingParameter, UsedImplicitly]
		OperationMonitor Monitor { get; set; }

		FieldValidationContext Validation { get; set; }

		public bool? Valid => !Validation.Active ? Validation.Text == null : (bool?)null;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			Identifier = Form.FindComponent(Component).FieldIdentifier;
			Monitor.Execute(this);

			Validation = new FieldValidationContext(this, Definitions.Result(), Monitor.EditContext);
		}

		public void Reset()
		{
			Validation.Execute();
		}

		public void Start()
		{
			InvokeAsync(_validate);
		}

		public void Execute(None _)
		{
			StateHasChanged();
		}

		protected override string GetComponentCssClass()
			=> $"ui-message ui-messages-{(Validation.Active ? "active" : "error")} {(Popup ? "ui-message-popup" : string.Empty)}";

		async Task Validate()
		{
			await Validation.Get();

			Monitor.Refresh();
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Visible)
			{
				var text = Validation.Text;
				if (text != null)
				{
					builder.OpenElement(0, "div");
					builder.AddAttribute(1, "style", Style);
					builder.AddAttribute(2, "class", GetCssClass());
					builder.AddMultipleAttributes(3, Attributes);
					builder.AddContent(4, text);
					builder.CloseElement();
				}
			}
		}
	}
}