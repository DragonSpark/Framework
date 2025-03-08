using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public abstract class ValidationComponent : ComponentBase, IDisposable
{
	readonly static FieldIdentifier DefaultIdentifier = new();
	readonly        Switch          _update           = new();
	ValidationMessageStore          _messages         = null!;
	EditContext?                    _context;
	bool                            _enabled = true;

	[Parameter]
	public bool Enabled
	{
		get => _enabled;
		set
		{
			if (_enabled != value)
			{
				_enabled = value;
				_update.Up();
			}
		}
	}

	[Parameter]
	public FieldIdentifier Identifier { get; set; } = DefaultIdentifier;

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
		Update();
	}

	void FieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (e.FieldIdentifier.Equals(Identifier))
		{
			Update();
		}
	}

	protected abstract bool Validate();

	public override Task SetParametersAsync(ParameterView parameters)
	{
		if (!Identifier.Equals(DefaultIdentifier) && parameters.DidParameterChange(nameof(Identifier), Identifier))
		{
			_messages.Clear(Identifier);
		}

		return base.SetParametersAsync(parameters);
	}

	protected override void OnParametersSet()
	{
		if (_update.Down())
		{
			Update();
		}
		base.OnParametersSet();
	}

	void Update()
	{
		_messages.Clear(Identifier);
		if (Enabled && !Validate())
		{
			_messages.Add(Identifier, ErrorMessage);
		}

		_context.Verify().NotifyValidationStateChanged();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		EditContext = null;
	}
}