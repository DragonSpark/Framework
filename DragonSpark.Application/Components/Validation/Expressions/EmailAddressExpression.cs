namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class EmailAddressExpression : Expression
{
	public static EmailAddressExpression Default { get; } = new EmailAddressExpression();

	EmailAddressExpression() : base(@"^([a-zA-Z0-9_\-\.+]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,9})$") {}
}