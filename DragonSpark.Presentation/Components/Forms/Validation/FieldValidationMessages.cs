namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class FieldValidationMessages
	{
		public static implicit operator FieldValidationMessages(string instance) => new (instance);

		public FieldValidationMessages(string invalid, string loading = "Validating this field... please wait.",
		                               string error = "An exception occurred while validating this field.")
		{
			Invalid = invalid;
			Loading = loading;
			Error   = error;
		}

		public string Invalid { get; }

		public string Loading { get; }

		public string Error { get; }

		public void Deconstruct(out string invalid, out string loading, out string error)
		{
			invalid = Invalid;
			loading = Loading;
			error   = Error;
		}
	}
}