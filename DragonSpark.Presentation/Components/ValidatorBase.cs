using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	/// <summary>
	/// ATTRIBUTION: https://www.nuget.org/packages/Radzen.Blazor/
	/// </summary>
	public abstract class ValidatorBase : RadzenComponent, IRadzenFormValidator, IDisposable
	{
		protected ValidationMessageStore Messages { get; private set; }

		[CascadingParameter]
		public IRadzenForm Form { get; set; }

		[Parameter]
		public string Component { get; set; }

		[Parameter]
		public abstract string Text { get; set; }

		[Parameter]
		public bool Popup { get; set; }

		public bool IsValid { get; protected set; } = true;

		[CascadingParameter]
		public EditContext EditContext { get; set; }

		public override Task SetParametersAsync(ParameterView parameters)
		{
			var result = base.SetParametersAsync(parameters);

			if (EditContext != null && Messages == null)
			{
				Messages                             =  new ValidationMessageStore(EditContext);
				EditContext.OnFieldChanged           += ValidateField;
				EditContext.OnValidationRequested    += ValidateModel;
				EditContext.OnValidationStateChanged += ValidationStateChanged;
			}

			return result;
		}

		void ValidateField(object sender, FieldChangedEventArgs args)
		{
			var component       = Form.FindComponent(Component);
			var fieldIdentifier = component?.FieldIdentifier ?? args.FieldIdentifier;
			var fieldName       = fieldIdentifier.FieldName;
			var str             = component != null ? fieldIdentifier.FieldName : null;

			if (fieldName == str)
			{
				ValidateModel(sender, ValidationRequestedEventArgs.Empty);
			}
		}

		void ValidateModel(object sender, ValidationRequestedEventArgs args)
		{
			var component = Form.FindComponent(Component);
			if (component != null)
			{
				IsValid = Validate(component);
				var     messages         = Messages;
				var     fieldIdentifier1 = component.FieldIdentifier;
				ref var local1           = ref fieldIdentifier1;
				messages.Clear(in local1);
				if (!IsValid)
				{
					messages.Add(in local1, Text);
				}

				EditContext?.NotifyValidationStateChanged();
			}
			else
			{
				throw new InvalidOperationException($"Cannot find component with Name {Component}");
			}
		}

		protected override string GetComponentCssClass()
			=> $"ui-message ui-messages-error {(Popup ? "ui-message-popup" : string.Empty)}";

		protected abstract bool Validate(IRadzenFormComponent component);

		void ValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
		{
			StateHasChanged();
		}

		public void Dispose()
		{
			if (EditContext != null)
			{
				EditContext.OnFieldChanged           -= ValidateField;
				EditContext.OnValidationRequested    -= ValidateModel;
				EditContext.OnValidationStateChanged -= ValidationStateChanged;
			}
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Visible && !IsValid)
			{
				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "style", Style);
				builder.AddAttribute(2, "class", GetCssClass());
				builder.AddMultipleAttributes(3, Attributes);
				builder.AddContent(4, Text);
				builder.CloseElement();
			}
		}
	}
}