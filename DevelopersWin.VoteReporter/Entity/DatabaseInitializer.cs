using System.Data.Entity;
using DragonSpark.Setup;

namespace DevelopersWin.VoteReporter.Entity
{
	[RegisterAs( typeof(IDatabaseInitializer<VotingContext>) )]
	public class DatabaseInitializer : DragonSpark.Windows.Entity.MigrateDatabaseToLatestVersion<VotingContext, Configuration>
	{}
}