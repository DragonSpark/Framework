using AspNet.Security.OAuth.Paypal;

namespace DragonSpark.Identity.PayPal
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base(PaypalAuthenticationDefaults.DisplayName) {}
	}
}