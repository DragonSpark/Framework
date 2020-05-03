using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class MetadataFieldValidator : IFieldValidator
	{
		readonly ValidationAttribute _metadata;
		readonly string?              _name;

		public MetadataFieldValidator(ValidationAttribute metadata, string? name = null)
		{
			_metadata = metadata;
			_name     = name;
		}

		public ValidationResult Get(object parameter)
		{
			var valid = _metadata.IsValid(parameter);
			var result =
				new ValidationResult(valid,
				                     _name != null ? _metadata.FormatErrorMessage(_name) : _metadata.ErrorMessage);
			return result;
		}
	}
}