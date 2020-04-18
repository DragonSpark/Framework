using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	/// <summary>
	/// Temp component to enable asynchronous click event:
	/// https://forum.radzen.com/t/synchronous-call-used-for-radzenbutton-click/2844
	/// </summary>
	public class ButtonComponent : RadzenComponent
	{
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			if (!Visible)
				return;
			__builder.AddContent(0, "    ");
			__builder.OpenElement(1, "button");
			__builder.AddAttribute(2, "style", Style);
			__builder.AddAttribute(3, "disabled", Disabled);
			__builder.AddAttribute(4, "type", Enum.GetName(typeof(ButtonType), ButtonType).ToLower());
			__builder.AddMultipleAttributes(5,
			                                RuntimeHelpers
				                                .TypeCheck<IEnumerable<KeyValuePair<string, object>>>(Attributes));
			__builder.AddAttribute(6, "class", GetCssClass());
			__builder.AddAttribute(7, "onclick", EventCallback.Factory.Create(this, OnClick));
			__builder.AddMarkupContent(8, "\r\n");
			if (!string.IsNullOrEmpty(Icon))
			{
				__builder.AddContent(9, "            ");
				__builder.OpenElement(10, "i");
				__builder.AddAttribute(11, "class", "ui-button-icon-left pi");
				__builder.AddContent(12, (MarkupString)Icon);
				__builder.CloseElement();
				__builder.AddMarkupContent(13, "\r\n");
			}

			if (!string.IsNullOrEmpty(Text))
			{
				__builder.AddContent(14, "            ");
				__builder.OpenElement(15, "span");
				__builder.AddAttribute(16, "class", "ui-button-text");
				__builder.AddContent(17, Text);
				__builder.CloseElement();
				__builder.AddMarkupContent(18, "\r\n");
			}

			__builder.AddContent(19, "    ");
			__builder.CloseElement();
			__builder.AddMarkupContent(20, "\r\n");
		}

		[Parameter]
		public string Text { get; set; } = string.Empty;

		[Parameter]
		public string Icon { get; set; }

		[Parameter]
		public ButtonStyle ButtonStyle { get; set; }

		[Parameter]
		public ButtonType ButtonType { get; set; }

		[Parameter]
		public ButtonSize Size { get; set; }

		[Parameter]
		public bool Disabled { get; set; }

		[Parameter]
		public EventCallback<MouseEventArgs> Click { get; set; }

		Task OnClick(MouseEventArgs args) => Disabled ? Task.CompletedTask : Click.InvokeAsync(args);

		[Parameter]
		public string CssClass { get; set; }


		string GetButtonSize() => Size != ButtonSize.Medium ? "sm" : "md";

		protected override string GetComponentCssClass()
		{
			if (string.IsNullOrWhiteSpace(CssClass))
			{
				var iconOnly = !string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(Icon)
					               ? string.Empty
					               : " ui-button-icon-only";
				var disabled = Disabled ? " ui-state-disabled" : string.Empty;
				var name     = Enum.GetName(typeof(ButtonStyle), ButtonStyle).ToLower();
				var result =
					$"ui-button ui-button-{GetButtonSize()} ui-widget ui-state-default ui-corner-all btn-{name}{disabled}{iconOnly}";
				return result;
			}
			return CssClass;
		}
	}
}