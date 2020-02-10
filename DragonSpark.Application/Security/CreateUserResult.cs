using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security
{
	public readonly struct CreateUserResult<T>
	{
		public CreateUserResult(T user, IdentityResult result)
		{
			User   = user;
			Result = result;
		}

		public T User { get; }

		public IdentityResult Result { get; }

		public void Deconstruct(out T user, out IdentityResult result)
		{
			user   = User;
			result = Result;
		}
	}
}