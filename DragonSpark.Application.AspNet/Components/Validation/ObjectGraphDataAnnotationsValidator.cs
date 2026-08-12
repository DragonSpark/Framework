using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Components.Validation.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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
					_messages                   =  new(field, new(field));
					_validator                  =  new(Condition.Then());
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
		if (field.FieldName.IsAssigned())
		{
			var value = _delegates.Get(field);
			if (value is not null)
			{
				var context = _contexts.Get(new NewValidationContext(new(field.Model, field.FieldName), _validator));
				var results = new List<ValidationResult>();

				Validator.TryValidateProperty(value, context, results);
				_messages.Execute((field, results));
				_messages.Execute(_contexts.Get(context));
				edit.NotifyValidationStateChanged();
			}
		}
	}

	public void Dispose()
	{
		EditContext = null;
	}
}
