using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public class Validating : ComponentBase, IDisposable
{
	readonly Switch           _requested = new();
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

	[Parameter]
	public EventCallback Valid { get; set; }

	[Parameter]
	public EventCallback Invalid { get; set; }

	[Parameter]
	public bool Enabled
	{
		get => _enabled;
		set
		{
			if (_enabled != value)
			{
				_enabled = value;
				if (value)
				{
					Request();
				}
			}
		}
	}

	bool _enabled = true;

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
		Request();
	}

	bool IsEmpty() => !_messages[Identifier].AsValueEnumerable().Any();

	bool IsValid() => !_context.Verify().GetValidationMessages(Identifier).AsValueEnumerable().Any();

	void Request()
	{
		if (Enabled)
		{
			_requested.Up();
			_list.Execute(StartUpdate());
		}
	}

	void FieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (Enabled && e.FieldIdentifier.Equals(Identifier))
		{
			Task.Run(FieldChanged(e).Self);
		}
	}

	async Task FieldChanged(FieldChangedEventArgs _)
	{
		await Task.Delay(100);
		if (!_requested.Down())
		{
			await StartUpdate().ConfigureAwait(false);
		}
	}

	Task StartUpdate() => InvokeAsync(_update);

	async Task Update()
	{
		_messages.Clear(Identifier);
		if (IsValid())
		{
			await Validate.InvokeAsync(new(new(Context.Verify(), Identifier), _messages, Message));
			_context.Verify().NotifyValidationStateChanged();

			var callback = IsEmpty() ? Valid : Invalid;
			await callback.InvokeAsync().ConfigureAwait(false);
		}
	}

	public virtual void Dispose()
	{
		Context = null;
		GC.SuppressFinalize(this);
	}
}