namespace DragonSpark.Application.Security.Identity.Model;

public sealed class ReturnUrlValue : Value
{
	public static ReturnUrlValue Default { get; } = new();

	ReturnUrlValue() : base(ReturnUrl.Default) {}
}