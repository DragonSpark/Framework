using System.Data.Entity;
using DragonSpark.Setup;
using DragonSpark.Setup.Registration;

namespace DevelopersWin.VoteReporter.Entity
{
	[Register( typeof(IDatabaseInitializer<VotingContext>) )]
	public class DatabaseInitializer : DragonSpark.Windows.Entity.MigrateDatabaseToLatestVersion<VotingContext, Configuration>
	{}
}