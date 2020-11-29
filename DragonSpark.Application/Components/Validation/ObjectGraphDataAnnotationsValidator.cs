using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation
{
	/// <summary>
	/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
	/// </summary>
	public class ObjectGraphDataAnnotationsValidator : ComponentBase, IDisposable
	{
		readonly IDelegates          _delegates;
		readonly IValidationContexts _contexts;
		Messages                     _messages = default!;

		public ObjectGraphDataAnnotationsValidator() : this(new StoredDelegates(), ValidationContexts.Default) {}

		public ObjectGraphDataAnnotationsValidator(IDelegates delegates, IValidationContexts contexts)
		{
			_delegates = delegates;
			_contexts  = contexts;
		}

		[Parameter]
		public ICondition<object?> Condition { get; set; } = Is.Assigned<object?>().Out();

		ObjectGraphValidator Validator { get; set; } = null!;

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
						_messages.Execute();
						_context.OnFieldChanged        -= FieldChanged;
						_context.OnValidationRequested -= ValidationRequested;
					}

					if ((_context = value) != null)
					{
						_context.OnValidationRequested += ValidationRequested;
						_context.OnFieldChanged        += FieldChanged;
						_messages                      =  new Messages(_context, new ValidationMessageStore(_context));
						Validator                      =  new ObjectGraphValidator(Condition.Then());
					}
				}
			}
		}

		EditContext? _context;

		void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
		{
			var edit    = EditContext.Verify();
			var context = Validator.Validate(edit.Model);

			_messages.Execute();
			_messages.Execute(context);

			edit.NotifyValidationStateChanged();
		}

		void FieldChanged(object? sender, FieldChangedEventArgs e)
		{
			var edit    = EditContext.Verify();
			var field   = e.FieldIdentifier;
			var value   = _delegates.Get(field);
			var context = _contexts.Get(new NewValidationContext(field, Validator));
			var results = new List<ValidationResult>();

			System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(value, context, results);

			_messages.Execute((field, results));
			_messages.Execute(_contexts.Get(context));

			edit.NotifyValidationStateChanged();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			EditContext = null;
		}
	}
}