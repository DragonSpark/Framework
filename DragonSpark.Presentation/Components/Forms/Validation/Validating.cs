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
	readonly Switch           _requested = new(), _active = new();
	readonly IOperationsStore _store;

	ValidationMessageStore _messages = default!;
	IOperations            _list     = default!;
	EditContext?           _context;

	public Validating() : this(OperationsStore.Default) {}

	public Validating(IOperationsStore store) => _store  = store;

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
					_requested.Up();
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
		if (Enabled && !_active && _requested.Up())
		{
			StateHasChanged();
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (_requested.Down())
		{
			using (_active.Assigned(true))
			{
				var task = Update();
				_list.Execute(task);
				await task.Await();
			}
		}
	}

	void FieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (e.FieldIdentifier.Equals(Identifier))
		{
			Request();
		}
	}

	async Task Update()
	{
		_messages.Clear(Identifier);
		var context = _context;
		if (context is not null && IsValid())
		{
			await Validate.Invoke(new(new(context, Identifier), _messages, Message));

			context.NotifyValidationStateChanged();

			var callback = IsEmpty() ? Valid : Invalid;
			await callback.Invoke().Await();
		}
	}

	public virtual void Dispose()
	{
		Context = null;
		GC.SuppressFinalize(this);
	}
}