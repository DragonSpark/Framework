using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class MetadataFieldValidator : IFieldValidator
	{
		readonly ValidationAttribute _metadata;
		readonly ValidationResult    _invalid;

		public MetadataFieldValidator(ValidationAttribute metadata, string? name = null)
			: this(metadata,
			       new ValidationResult(name == null ? metadata.ErrorMessage : metadata.FormatErrorMessage(name))) {}

		public MetadataFieldValidator(ValidationAttribute metadata, ValidationResult invalid)
		{
			_metadata = metadata;
			_invalid  = invalid;
		}

		public ValidationResult Get(object parameter)
			=> _metadata.IsValid(parameter) ? ValidationResult.Success : _invalid;
	}
}