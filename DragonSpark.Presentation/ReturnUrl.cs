namespace DragonSpark.Presentation
{
	public sealed class ReturnUrl : Text.Text
	{
		public static ReturnUrl Default { get; } = new ReturnUrl();

		ReturnUrl() : base("returnUrl") {}
	}
}