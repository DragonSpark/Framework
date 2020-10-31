using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DragonSpark.Application.Components
{
	/// <summary>
	/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
	/// </summary>
	public class ObjectGraphDataAnnotationsValidator : ComponentBase
	{
		readonly static object ValidatorKey = new object(), ObjectsKey = new object();

		ValidationMessageStore _store = default!;

		[CascadingParameter]
		EditContext EditContext { get; set; } = default!;

		protected override void OnInitialized()
		{
			_store = new ValidationMessageStore(EditContext);

			EditContext.OnValidationRequested += (sender, eventArgs) =>
			                                     {
				                                     _store.Clear();
				                                     ValidateObject(EditContext.Model, new HashSet<object>());
				                                     EditContext.NotifyValidationStateChanged();
			                                     };

			EditContext.OnFieldChanged += (sender, args) => ValidateField(EditContext, _store, args.FieldIdentifier);
		}

		// ReSharper disable once MethodTooLong
		// ReSharper disable once CognitiveComplexity
		void ValidateObject(object? value, HashSet<object> visited)
		{
			if (value is { } && visited.Add(value))
			{
				if (value is IEnumerable<object> enumerable)
				{
					foreach (var item in enumerable)
					{
						ValidateObject(item, visited);
					}
				}

				var results = new List<ValidationResult>();
				ValidateObject(value, visited, results);

				foreach (var item in results)
				{
					if (!item.MemberNames.Any())
					{
						_store.Add(new FieldIdentifier(value, string.Empty), item.ErrorMessage);
						continue;
					}

					foreach (var name in item.MemberNames)
					{
						var identifier = new FieldIdentifier(value, name);
						_store.Add(identifier, item.ErrorMessage);
					}
				}
			}
		}

		ValidationContext New(object value) => New(value, new HashSet<object>());

		ValidationContext New(object value, HashSet<object> visited)
		{
			var result = new ValidationContext(value);
			result.Items.Add(ValidatorKey, this);
			result.Items.Add(ObjectsKey, visited);
			return result;
		}

		void ValidateObject(object value, HashSet<object> visited, List<ValidationResult> validationResults)
		{
			var context = New(value, visited);
			Validator.TryValidateObject(value, context, validationResults, true);
		}

		internal static void TryValidateRecursive(object value,
		                                          ValidationContext context)
		{
			if (context.Items.TryGetValue(ValidatorKey, out var result) &&
			    result is ObjectGraphDataAnnotationsValidator validator)
			{
				var visited = (HashSet<object>)context.Items[ObjectsKey];
				validator.ValidateObject(value, visited);
			}
		}

		void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier field)
		{
			var metadata = field.Model.GetType().GetProperty(field.FieldName);
			if (metadata != null)
			{
				var property = metadata.GetValue(field.Model);
				var context  = New(field.Model);
				context.MemberName = metadata.Name;
				var results = new List<ValidationResult>();

				Validator.TryValidateProperty(property, context, results);
				messages.Clear(field);
				messages.Add(field, results.Select(result => result.ErrorMessage));

				editContext.NotifyValidationStateChanged();
			}
		}
	}
}