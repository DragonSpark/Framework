namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class FieldValidationMessages
	{
		public FieldValidationMessages(string invalid, string loading, string error)
		{
			Invalid = invalid;
			Loading = loading;
			Error   = error;
		}

		public string Invalid { get; }

		public string Loading { get; }

		public string Error { get; }
	}
}