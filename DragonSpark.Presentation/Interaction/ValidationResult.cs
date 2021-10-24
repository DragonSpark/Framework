namespace DragonSpark.Presentation.Interaction
{
	public class ValidationResult : IInteractionResult
	{
		public ValidationResult(string message) => Message = message;

		public string Message { get; }
	}
}
