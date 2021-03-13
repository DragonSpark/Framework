using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public class FieldValidation<T> : ComponentBase, IDisposable
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
		public IValidateValue<T> Validator { get; set; } = default!;

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

		void Update()
		{
			_messages.Clear(Identifier);
			if (Enabled)
			{
				if (!Validator.Get(Identifier.GetValue<T>()))
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
}