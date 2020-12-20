namespace DragonSpark.Application.Components.Validation.Expressions
{
	public sealed class EmailAddressValidator : RegularExpressionValidator
	{
		public static EmailAddressValidator Default { get; } = new EmailAddressValidator();

		EmailAddressValidator() : base(EmailAddressExpression.Default) {}
	}
}