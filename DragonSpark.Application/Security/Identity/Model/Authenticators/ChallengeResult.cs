using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators
{
	public readonly struct ChallengeResult<T> where T : IdentityUser
	{
		public ChallengeResult(T user, ExternalLoginInfo information, IdentityResult result)
		{
			User        = user;
			Information = information;
			Result      = result;
		}

		public T User { get; }

		public ExternalLoginInfo Information { get; }

		public IdentityResult Result { get; }

		public void Deconstruct(out T user, out ExternalLoginInfo information, out IdentityResult result)
		{
			user        = User;
			information = Information;
			result      = Result;
		}
	}
}