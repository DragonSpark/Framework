using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class ObjectGraphValidator
	{
		readonly Func<object?, bool> _condition;
		readonly IValidationContexts _contexts;

		public ObjectGraphValidator() : this(Is.Assigned<object?>()) {}

		public ObjectGraphValidator(Func<object?, bool> condition) : this(condition, ValidationContexts.Default) {}

		public ObjectGraphValidator(Func<object?, bool> condition, IValidationContexts contexts)
		{
			_condition = condition;
			_contexts  = contexts;
		}

		public ModelValidationContext Validate(object? value)
		{
			var result = new ModelValidationContext();
			Validate(value, result);
			return result;
		}

		public void Validate(object? value, ModelValidationContext context)
		{
			if (_condition(value))
			{

				Apply(value.Verify(), context);
			}
		}

		void Apply(object value, ModelValidationContext context)
		{
			if (context.Get(value))
			{
				if (value is IEnumerable<object> enumerable)
				{
					foreach (var item in enumerable.AsValueEnumerable())
					{
						Validate(item, context);
					}
				}

				Visit(value, context);
			}
		}

		void Visit(object value, ModelValidationContext context)
		{
			var path = context.Get();
			foreach (var item in Results(value, context).AsValueEnumerable())
			{
				var names = item!.MemberNames.AsValueEnumerable();
				if (names.Any())
				{
					foreach (var name in names)
					{
						context.Add(new ValidationResultMessage(path, new FieldIdentifier(value, name!),
						                                        item.ErrorMessage));
					}
				}
				else
				{
					context.Add(new ValidationResultMessage(path, value, item.ErrorMessage));
				}
			}
		}

		IEnumerable<ValidationResult> Results(object value, ModelValidationContext model)
		{
			var result  = new List<ValidationResult>();
			var context = _contexts.Get(new NewValidationContext(value, this, model));
			Validator.TryValidateObject(value, context, result, true);
			return result;
		}
	}
}