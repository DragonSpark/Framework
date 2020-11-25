using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public class Validating : ComponentBase, IDisposable
	{
		readonly IOperationsStore _store;
		readonly Func<Task>       _update;

		ValidationMessageStore _messages = default!;
		IOperations            _list     = default!;
		EditContext?           _context;

		public Validating() : this(OperationsStore.Default) {}

		public Validating(IOperationsStore store)
		{
			_store  = store;
			_update = Update;
		}

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
						_list.Execute();
						_messages.Clear();
						_context.OnFieldChanged        -= FieldChanged;
						_context.OnValidationRequested -= ValidationRequested;
					}

					if ((_context = value) != null)
					{
						_messages = new ValidationMessageStore(_context);
						_list     = _store.Get(_context);

						_context.OnFieldChanged        += FieldChanged;
						_context.OnValidationRequested += ValidationRequested;
					}
				}
			}
		}

		void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
		{
			if (!_messages[Identifier].AsValueEnumerable().Any())
			{
				_list.Execute(InvokeAsync(_update));
			}
		}

		void FieldChanged(object? sender, FieldChangedEventArgs e)
		{
			if (e.FieldIdentifier.Equals(Identifier))
			{
				InvokeAsync(_update);
			}
		}

		async Task Update()
		{
			_messages.Clear(Identifier);

			var edit = _context.Verify();
			if (!edit.GetValidationMessages(Identifier).AsValueEnumerable().Any())
			{
				var context = new ValidationContext(new FieldContext(Context.Verify(), Identifier), _messages, Message);

				await Validate.InvokeAsync(context);

				edit.NotifyValidationStateChanged();
			}
		}

		public virtual void Dispose()
		{
			Context = null;
			GC.SuppressFinalize(this);
		}
	}
}