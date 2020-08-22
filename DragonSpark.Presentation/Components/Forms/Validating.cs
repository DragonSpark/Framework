using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public class Validating : ComponentBase, IDisposable
	{
		ValidationMessageStore _messages = default!;
		EditContext?           _context;

		[Parameter]
		public FieldIdentifier Identifier { get; set; }

		[Parameter]
		public string Message { get; set; } = "This field does not contain a valid value.";

		[Parameter]
		public EventCallback<ValidationContext> Validate { get; set; }

		[CascadingParameter]
		EditContext? Context
		{
			get => _context;
			set
			{
				if (_context != value)
				{
					if (_context != null)
					{
						_messages.Clear();
						_context.OnFieldChanged -= ContextOnOnFieldChanged;
					}

					if ((_context = value) != null)
					{
						_messages = new ValidationMessageStore(_context);

						_context.OnFieldChanged += ContextOnOnFieldChanged;
					}
				}
			}
		}

		void ContextOnOnFieldChanged(object? sender, FieldChangedEventArgs e)
		{
			if (e.FieldIdentifier.Equals(Identifier))
			{
				InvokeAsync(Update);
			}
		}

		async Task Update()
		{
			_messages.Clear(Identifier);

			var context    = new FieldContext(Context.Verify(), Identifier);
			var validation = new ValidationContext(context, _messages, Message);

			await Validate.InvokeAsync(validation);

			_context.Verify().NotifyValidationStateChanged();
		}

		public virtual void Dispose()
		{
			Context = null;
		}
	}
}