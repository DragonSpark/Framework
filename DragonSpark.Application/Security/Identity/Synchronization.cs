using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public readonly struct Synchronization<T> where T : IdentityUser
	{
		public Synchronization(ExternalLoginInfo login, T user)
		{
			Login = login;
			User  = user;
		}

		public ExternalLoginInfo Login { get; }
		public T User { get; }

		public void Deconstruct(out ExternalLoginInfo login, out T user)
		{
			login = Login;
			user  = User;
		}
	}
}