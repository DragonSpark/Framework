using DragonSpark.Windows.Entity;

namespace DevelopersWin.VoteReporter.Entity
{
	public class Configuration : DbMigrationsConfiguration<VotingContext>
	{
		public Configuration()
		{}

		public Configuration( IActivationSource source ) : base( source )
		{}
	}
}