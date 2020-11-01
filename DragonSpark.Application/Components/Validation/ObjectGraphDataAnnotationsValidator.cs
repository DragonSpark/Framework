using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DragonSpark.Application.Components.Validation
{
	/// <summary>
	/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
	/// </summary>
	public class ObjectGraphDataAnnotationsValidator : ComponentBase, IDisposable
	{
		readonly IValidationContexts _contexts;
		ValidationMessageStore       _store = default!;

		public ObjectGraphDataAnnotationsValidator() : this(ValidationContexts.Default) {}

		public ObjectGraphDataAnnotationsValidator(IValidationContexts contexts) => _contexts = contexts;

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
						_store.Clear();
						_context.OnFieldChanged        -= FieldChanged;
						_context.OnValidationRequested -= ValidationRequested;
					}

					if ((_context = value) != null)
					{
						_store                         =  new ValidationMessageStore(_context);
						_context.OnFieldChanged        += FieldChanged;
						_context.OnValidationRequested += ValidationRequested;
						Validator                      =  new ObjectGraphValidator(Condition.Then(), _store);
					}
				}
			}
		}

		EditContext? _context;

		void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
		{
			_store.Clear();
			var context = EditContext.Verify();
			Validator.Validate(context.Model);
			context.NotifyValidationStateChanged();
		}

		void FieldChanged(object? sender, FieldChangedEventArgs e)
		{
			ValidateField(EditContext.Verify(), _store, e.FieldIdentifier);
		}

		void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier field)
		{
			var metadata = field.Model.GetType().GetProperty(field.FieldName);
			if (metadata != null)
			{
				var results  = new List<ValidationResult>();
				var property = metadata.GetValue(field.Model);
				var context  = _contexts.Get(new NewValidationContext(Validator, field.Model));
				context.MemberName = metadata.Name;

				System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(property, context, results);

				messages.Clear(field);
				messages.Add(field, results.Select(result => result.ErrorMessage));

				editContext.NotifyValidationStateChanged();
			}
		}

		public void Dispose()
		{
			EditContext = null;
		}
	}
}