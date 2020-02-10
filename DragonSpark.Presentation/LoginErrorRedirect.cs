namespace DragonSpark.Presentation
{
	public sealed class LoginErrorRedirect : ErrorRedirect
	{
		public LoginErrorRedirect(string message, string origin) : base("./Login", message, origin) {}
	}
}