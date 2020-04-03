namespace DragonSpark.Application.Security.Identity.Profile
{
	public interface IUserMapping : IAccessor<IdentityUser>
	{
		bool Required { get; }

		string Key { get; }
	}
}