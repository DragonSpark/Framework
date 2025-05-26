using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.AspNet.Components.Validation;

/// <summary>
/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
/// </summary>
[MustDisposeResource(false)]
public sealed class ObjectGraphDataAnnotationsValidator : ComponentBase, IDisposable
{
	readonly IDelegates          _delegates;
	readonly IValidationContexts _contexts;
	Messages                     _messages = null!;
	ObjectGraphValidator         _validator = null!;

	[MustDisposeResource(false)]
	public ObjectGraphDataAnnotationsValidator() : this(new Delegates(), ValidationContexts.Default) {}

	[MustDisposeResource(false)]
	public ObjectGraphDataAnnotationsValidator(IDelegates delegates, IValidationContexts contexts)
	{
		_delegates = delegates;
		_contexts  = contexts;
	}

	[Parameter]
	public ICondition<object?> Condition { get; set; } = Is.Assigned<object?>().Out();

	[CascadingParameter]
	EditContext? EditContext
	{
		get;
		set
		{
			if (field != value)
			{
				if (field != null)
				{
					_messages.Execute();
					field.OnFieldChanged        -= FieldChanged;
					field.OnValidationRequested -= ValidationRequested;
				}

				if ((field = value) != null)
				{
					field.OnValidationRequested += ValidationRequested;
					field.OnFieldChanged        += FieldChanged;
					_messages                   =  new Messages(field, new ValidationMessageStore(field));
					_validator                  =  new ObjectGraphValidator(Condition.Then());
				}
			}
		}
	}

	void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
	{
		var edit    = EditContext.Verify();
		var context = _validator.Validate(edit.Model);

		_messages.Execute();
		_messages.Execute(context);

		edit.NotifyValidationStateChanged();
	}

	void FieldChanged(object? sender, FieldChangedEventArgs e)
	{
		var edit    = EditContext.Verify();
		var field   = e.FieldIdentifier;
		if (!string.IsNullOrEmpty(field.FieldName))
		{
			var value   = _delegates.Get(field);
			var context = _contexts.Get(new NewValidationContext(field, _validator));
			var results = new List<ValidationResult>();

			Validator.TryValidateProperty(value, context, results);

			_messages.Execute((field, results));
			_messages.Execute(_contexts.Get(context));

			edit.NotifyValidationStateChanged();
		}
	}

	public void Dispose()
	{
		// GC.SuppressFinalize(this);
		EditContext = null;
	}
}
