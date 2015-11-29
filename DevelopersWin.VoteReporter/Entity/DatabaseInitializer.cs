using System.Data.Entity;
using DragonSpark.Setup;

namespace DevelopersWin.VoteReporter.Entity
{
	[Register( typeof(IDatabaseInitializer<VotingContext>) )]
	public class DatabaseInitializer : DragonSpark.Windows.Entity.MigrateDatabaseToLatestVersion<VotingContext, Configuration>
	{}
}