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

		EditContext _editContext;

		public FieldValidator() => _validate = Refresh;

		[Parameter, UsedImplicitly]
		public bool Popup { get; set; }

		[Parameter, UsedImplicitly]
		public List<FieldValidationDefinition> Definitions { get; set; }

		[Parameter, UsedImplicitly]
		public string Component { get; set; }

		public FieldIdentifier Identifier { get; private set; }

		[Inject]
		internal ILogger<FieldValidator> Logger { get; [UsedImplicitly] set; }

		[CascadingParameter, UsedImplicitly]
		OperationMonitor Monitor { get; set; }

		FieldValidationContext Validation { get; set; }

		[CascadingParameter, UsedImplicitly]
		public EditContext EditContext
		{
			get => _editContext;
			set
			{

				if (_editContext != value)
				{
					var register = _editContext != null;
					_editContext = value;
					if (register)
					{
						Register();
					}
				}
			}
		}

		public bool? Valid => Validation.Valid;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			Register();
		}

		void Register()
		{
			Identifier = new FieldIdentifier(EditContext.Model, Component);
			Validation = new FieldValidationContext(this, Definitions.Result(), EditContext);
			Monitor.Execute(this);
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
			=> $"ui-message ui-messages-{(Validation.Valid.HasValue ? "error" : "active")} {(Popup ? "ui-message-popup" : string.Empty)}";

		public async ValueTask<bool> Validate()
		{
			if (Valid.GetValueOrDefault(false))
			{
				return true;
			}

			await Validation.Get().ConfigureAwait(false);

			if (!Valid.HasValue)
			{
				throw new
					InvalidOperationException($"An attempt was made to validate a field '{Component}' but a validation result could not be attained.");
			}

			return Valid.Value;
		}

		async Task Refresh()
		{
			await Validation.Get();

			EditContext.NotifyValidationStateChanged();
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