using System.Collections.Generic;

namespace DragonSpark.Application.Components.Validation
{
	public readonly struct NewValidationContext
	{
		public NewValidationContext(ObjectGraphValidator validator, object instance)
			: this(validator, instance, new HashSet<object>()) {}

		public NewValidationContext(ObjectGraphValidator validator, object instance, HashSet<object> visited)
		{
			Validator = validator;
			Instance  = instance;
			Visited   = visited;
		}

		public ObjectGraphValidator Validator { get; }

		public object Instance { get; }

		public HashSet<object> Visited { get; }

		public void Deconstruct(out ObjectGraphValidator validator, out object instance, out HashSet<object> visited)
		{
			validator = Validator;
			instance  = Instance;
			visited   = Visited;
		}
	}
}