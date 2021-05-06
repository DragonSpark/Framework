namespace DragonSpark.Identity.PayPal
{
	public sealed class PayPalApplicationSettings
	{
		public string Key { get; set; }  = null!;

		public string Secret { get; set; }  = null!;

		public PayPalAuthenticationSettings Authentication { get; set; } = new PayPalAuthenticationSettings();

	}
}