using AspNet.Security.OAuth.Paypal;

namespace DragonSpark.Identity.PayPal
{
	public sealed class PayPalAuthenticationSettings
	{
		public string[]? Scopes { get; set; }

		public string AuthorizationEndpoint { get; set; } = PaypalAuthenticationDefaults.AuthorizationEndpoint;
		public string TokenEndpoint { get; set; } = PaypalAuthenticationDefaults.TokenEndpoint;
		public string UserInformationEndpoint { get; set; } = PaypalAuthenticationDefaults.UserInformationEndpoint;
	}
}