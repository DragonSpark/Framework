using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Components.Forms.Validation;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Compose
{
	public sealed class ValidationContext
	{
		public static ValidationContext Default { get; } = new ValidationContext();

		ValidationContext() {}

		public ValidationDefinitionContext Using(ISelecting<FieldIdentifier, bool> operation)
			=> new ValidationDefinitionContext(operation);

		public IFieldValidator Using(params ValidationAttribute[] attributes)
			=> Using(attributes.Select(x => x.Adapt()).ToArray());

		public IFieldValidator Using(params IFieldValidator[] validators) => new CompositeFieldValidator(validators);
	}
}
