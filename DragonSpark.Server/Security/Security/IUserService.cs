namespace DragonSpark.Security
{
	public interface IUserService
	{
		long GetNextMembershipNumber();

		UserProfile Create( string name );

		UserProfile Get( string name );

		UserProfile GetAnonymous();

		void Save( UserProfile user );
	}
}