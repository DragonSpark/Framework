namespace DragonSpark.Application.Security.Identity.Profile
{
	public interface IUserMapping : IAssignment<IdentityUser>
	{
		bool Required { get; }

		string Key { get; }
	}
}