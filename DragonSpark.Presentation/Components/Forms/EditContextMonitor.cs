using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public class EditContextMonitor : ComponentBase
	{
		[Parameter]
		public EventCallback<EditContext> Changed { get; set; }

		EditContext _context = default!;

		[CascadingParameter]
		EditContext EditContext
		{
			get => _context;
			set
			{
				if (_context != null)
				{
					_context.OnFieldChanged           -= FieldChanged;
					_context.OnValidationStateChanged -= ValidationStateChanged;
				}

				if ((_context = value) != null)
				{
					_context                          =  value;
					_context.OnFieldChanged           += FieldChanged;
					_context.OnValidationStateChanged += ValidationStateChanged;
				}
			}
		}

		void ValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
		{
			Changed.InvokeAsync(EditContext);
		}

		void FieldChanged(object? sender, FieldChangedEventArgs args)
		{
			Changed.InvokeAsync(EditContext);
		}
	}
}