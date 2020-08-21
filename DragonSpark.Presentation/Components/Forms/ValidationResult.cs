using JetBrains.Annotations;

namespace DragonSpark.Presentation.Components.Forms
{
	public readonly struct ValidationResult
	{
		public static ValidationResult Success { get; } = new ValidationResult(true);

		public ValidationResult(string message) : this(string.IsNullOrEmpty(message), message) {}

		public ValidationResult(bool valid, string? message = null)
		{
			Valid   = valid;
			Message = message;
		}

		public bool Valid { get; }

		public string? Message { get; }

		public void Deconstruct(out bool valid, [CanBeNull] out string? message)
		{
			valid   = Valid;
			message = Message;
		}
	}
}