using DragonSpark.Model.Sequences;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class CompositeFieldValidator : IFieldValidator
	{
		readonly ValidationResult       _success;
		readonly Array<IFieldValidator> _validators;

		public CompositeFieldValidator(params IFieldValidator[] validators)
			: this(ValidationResult.Success, validators) {}

		public CompositeFieldValidator(ValidationResult success, params IFieldValidator[] validators)
		{
			_success    = success;
			_validators = validators;
		}

		public ValidationResult Get(object parameter)
		{
			var length = _validators.Length;
			for (var i = 0; i < length; i++)
			{
				var call = _validators[i].Get(parameter);
				if (!call.Valid)
				{
					return call;
				}
			}

			return _success;
		}
	}
}