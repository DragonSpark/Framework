using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class ObjectGraphValidator
	{
		readonly Func<object?, bool>    _condition;
		readonly ValidationMessageStore _store;
		readonly IValidationContexts    _contexts;

		public ObjectGraphValidator(ValidationMessageStore store) : this(Is.Assigned<object?>(), store) {}

		public ObjectGraphValidator(Func<object?, bool> condition, ValidationMessageStore store)
			: this(condition, store, ValidationContexts.Default) {}

		public ObjectGraphValidator(Func<object?, bool> condition, ValidationMessageStore store,
		                            IValidationContexts contexts)
		{
			_condition = condition;
			_store     = store;
			_contexts  = contexts;
		}

		public void Validate(object? value)
		{
			Enter(value, new HashSet<object>());
		}

		public void Validate(object? value, ValidationContext context)
		{
			Enter(value, _contexts.Get(context));
		}

		void Enter(object? value, HashSet<object> visited)
		{
			if (_condition(value))
			{
				Apply(value.Verify(), visited);
			}
		}

		void Apply(object value, HashSet<object> visited)
		{
			if (visited.Add(value))
			{
				if (value is IEnumerable<object> enumerable)
				{
					foreach (var item in enumerable)
					{
						Enter(item, visited);
					}
				}

				Visit(value, visited);
			}
		}

		void Visit(object value, HashSet<object> visited)
		{
			var results = new List<ValidationResult>();
			Validate(value, visited, results);

			foreach (var item in results)
			{
				if (item.MemberNames.Any())
				{
					foreach (var name in item.MemberNames)
					{
						_store.Add(new FieldIdentifier(value, name), item.ErrorMessage);
					}
				}
				else
				{
					_store.Add(new FieldIdentifier(value, string.Empty), item.ErrorMessage);
				}
			}
		}

		void Validate(object value, HashSet<object> visited, ICollection<ValidationResult> results)
		{
			var context = _contexts.Get(new NewValidationContext(this, value, visited));
			Validator.TryValidateObject(value, context, results, true);
		}
	}
}