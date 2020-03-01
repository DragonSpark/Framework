using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms
{
	public static class Validators
	{
		public static IFieldValidator Of(params IFieldValidator[] validators)
			=> new CompositeFieldValidator(validators);

		public static IFieldValidator Adapt(this ValidationAttribute @this, string name = null)
			=> new MetadataFieldValidator(@this, name);
	}
}