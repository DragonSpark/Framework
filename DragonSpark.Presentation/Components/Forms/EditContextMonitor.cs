using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public class EditContextMonitor : ComponentBase
	{
		[Parameter]
		public EventCallback<EditContext> Changed { get; set; }

		EditContext _editContext = default!;

		[CascadingParameter]
		EditContext EditContext
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
					_editContext                =  value;
					_editContext.OnFieldChanged += FieldChanged;
				}

			}
		}

		void FieldChanged(object? sender, FieldChangedEventArgs args)
		{
			Changed.InvokeAsync(EditContext);
		}
	}
}