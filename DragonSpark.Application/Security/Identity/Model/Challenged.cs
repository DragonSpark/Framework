using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class Challenged
	{
		public Challenged(ExternalLoginInfo login, string origin)
		{
			Login  = login;
			Origin = origin;
		}

		public ExternalLoginInfo Login { get; }

		public string Origin { get; }

		public void Deconstruct(out ExternalLoginInfo login, out string origin)
		{
			login  = Login;
			origin = Origin;
		}
	}
}