using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public abstract class ValidationComponent : ComponentBase, IDisposable
{
	ValidationMessageStore _messages = default!;
	EditContext?           _context;
	bool                   _enabled = true;

	[Parameter]
	public bool Enabled
	{
		get => _enabled;
		set
		{
			if (_enabled != value)
			{
				_enabled = value;
				Update();
			}
		}
	}

	[Parameter]
	public FieldIdentifier Identifier { get; set; }

	[Parameter]
	public string ErrorMessage { get; set; } = "This field does not contain a valid value.";

	[CascadingParameter]
	EditContext? EditContext
	{
		get => _context;
		set
		{
			if (_context != value)
			{
				if (_context != null)
				{
					_messages.Clear();
					_context.OnFieldChanged        -= FieldChanged;
					_context.OnValidationRequested -= ValidationRequested;
				}

				if ((_context = value) != null)
				{
					_messages = new ValidationMessageStore(_context);

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
			Update();
		}
	}

	void FieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (e.FieldIdentifier.Equals(Identifier))
		{
			Update();
		}
	}

	protected abstract bool Validate();

	void Update()
	{
		_messages.Clear(Identifier);
		if (Enabled)
		{
			if (!Validate())
			{
				_messages.Add(Identifier, ErrorMessage);
			}
		}
		_context.Verify().NotifyValidationStateChanged();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		EditContext = null;
	}
}