using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public class FieldModelMonitor : ComponentBase
	{
		[Parameter]
		public object Model { get; set; } = default!;

		/// <summary>Gets or sets a callback that updates the bound value.</summary>
		[Parameter]
		public EventCallback Changed { get; set; }

		EditContext? _editContext;

		[CascadingParameter]
		EditContext? EditContext
		{
			get => _editContext;
			set
			{
				if (_editContext != null)
				{
					_editContext.OnFieldChanged -= FieldChanged;
				}

				if ((_editContext = value) != null)
				{
					_editContext.OnFieldChanged += FieldChanged;
				}

			}
		}

		void FieldChanged(object? sender, FieldChangedEventArgs args)
		{
			if (args.FieldIdentifier.Model == Model)
			{
				Changed.InvokeAsync(Model);
			}
		}
	}
}