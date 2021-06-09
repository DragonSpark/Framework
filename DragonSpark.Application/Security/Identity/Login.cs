using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public readonly struct Login<T>
	{
		public Login(ExternalLoginInfo information, T user)
		{
			Information = information;
			User        = user;
		}

		public ExternalLoginInfo Information { get; }

		public T User { get; }

		public void Deconstruct(out ExternalLoginInfo information, out T user)
		{
			information = Information;
			user        = User;
		}
	}
}