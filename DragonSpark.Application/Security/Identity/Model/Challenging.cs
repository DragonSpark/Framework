namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class Challenging
	{
		public Challenging(string provider, string returnUrl)
		{
			Provider  = provider;
			ReturnUrl = returnUrl;
		}

		public string Provider { get; }

		public string ReturnUrl { get; }

		public void Deconstruct(out string provider, out string returnUrl)
		{
			provider  = Provider;
			returnUrl = ReturnUrl;
		}
	}
}