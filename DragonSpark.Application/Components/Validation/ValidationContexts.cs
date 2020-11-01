using DragonSpark.Compose;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class ValidationContexts : IValidationContexts
	{
		public static ValidationContexts Default { get; } = new ValidationContexts();

		ValidationContexts() : this(ValidatorKey.Default, VisitedKey.Default) {}

		readonly IValidatorKey<ObjectGraphValidator> _key;
		readonly IValidatorKey<HashSet<object>>      _visited;

		public ValidationContexts(IValidatorKey<ObjectGraphValidator> key, IValidatorKey<HashSet<object>> visited)
		{
			_key     = key;
			_visited = visited;
		}

		public ValidationContext Get(NewValidationContext parameter)
		{
			var (validator, instance, visited) = parameter;
			var result = new ValidationContext(instance);
			_key.Assign(result, validator);
			_visited.Assign(result, visited);
			return result;
		}

		public HashSet<object> Get(ValidationContext parameter) => _visited.Get(parameter) ?? new HashSet<object>();
	}
}