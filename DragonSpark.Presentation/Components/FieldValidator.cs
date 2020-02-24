using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public interface IFieldValidation : IOperationResult<FieldIdentifier, bool> {}

	public class FieldValidationDefinition : ComponentBase
	{
		public FieldValidationDefinition(IOperationResult<FieldIdentifier, bool> operation, string errorText,
		                                 string loadingText)
		{
			Operation   = operation;
			ErrorText   = errorText;
			LoadingText = loadingText;
		}

		public string ErrorText { get; }

		public string LoadingText { get; }

		public IOperationResult<FieldIdentifier, bool> Operation { get; }
	}

	public readonly struct ValidationResult
	{
		public static ValidationResult Success { get; } = new ValidationResult(true);

		public ValidationResult(string message) : this(string.IsNullOrEmpty(message), message) {}

		public ValidationResult(bool valid, string message = null)
		{
			Valid   = valid;
			Message = message;
		}

		public bool Valid { get; }

		public string Message { get; }
	}

	public sealed class FieldValidationContext : IOperationResult<ValidationResult>
	{
		readonly FieldValidator                   _owner;
		readonly Array<FieldValidationDefinition> _definitions;

		public FieldValidationContext(FieldValidator owner, Array<FieldValidationDefinition> definitions)
		{
			_owner       = owner;
			_definitions = definitions;
		}

		public string Status { get; private set; }

		public async ValueTask<ValidationResult> Get()
		{
			foreach (var definition in _definitions.Open())
			{
				Status = definition.LoadingText;
				if (!await definition.Operation.Get(_owner.Identifier))
				{
					return new ValidationResult(false, definition.ErrorText);
				}
			}

			return ValidationResult.Success;
		}
	}

	/// <summary>
	/// ATTRIBUTION: https://www.nuget.org/packages/Radzen.Blazor/
	/// </summary>
	public class FieldValidator : RadzenComponent
	{
		readonly Func<Task> _validate;

		public FieldValidator() => _validate = Validate;

		[Parameter, UsedImplicitly]
		public bool Popup { get; set; }

		[Parameter, UsedImplicitly]
		public string Text { get; set; }

		[Parameter, UsedImplicitly]
		public List<FieldValidationDefinition> Definitions { get; set; }

		[Parameter]
		public string Component { get; set; }

		public FieldIdentifier Identifier { get; private set; }

		[CascadingParameter, UsedImplicitly]
		IRadzenForm Form { get; set; }

		[CascadingParameter, UsedImplicitly]
		OperationMonitor Monitor { get; set; }

		ValidationMessageStore Messages { get; set; }

		FieldValidationContext Validation { get; set; }

		OperationView<ValidationResult> Current { get; set; }

		public bool? Valid { get; private set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

			Identifier = Form.FindComponent(Component).FieldIdentifier;
			Monitor.Execute(this);

			Validation = new FieldValidationContext(this, Definitions.Result());
			Messages   = new ValidationMessageStore(Monitor.EditContext);
		}

		public void Reset()
		{
			Messages.Clear(Identifier);
			Current = null; // TODO: Cancel?
			Valid   = null;
		}

		public void Start()
		{
			InvokeAsync(_validate);
		}

		public void Refresh()
		{
			StateHasChanged();
		}

		protected override string GetComponentCssClass()
			=> $"ui-message ui-messages-{(Current?.IsActive ?? false ? "active" : "error")} {(Popup ? "ui-message-popup" : string.Empty)}";

		async Task Validate()
		{
			Current = Validation.AsView();

			StateHasChanged();

			var result = await Current.Get();
			Valid = result.Valid;
			Text = result.Message;
			if (!result.Valid)
			{
				Messages.Add(Identifier, Text);
			}

			Current = null;
			Monitor.Refresh();
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Visible && (Current != null || !Valid.GetValueOrDefault(true)))
			{
				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "style", Style);
				builder.AddAttribute(2, "class", GetCssClass());
				builder.AddMultipleAttributes(3, Attributes);
				builder.AddContent(4, Valid.HasValue ? Text : "Loading...");
				builder.CloseElement();
			}
		}
	}
}