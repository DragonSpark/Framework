using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Windows.Entity;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteUpdater
	{
		void Update( Recording set, Vote vote );
	}

	class VoteUpdater : IVoteUpdater
	{
		readonly IVoteCountLocator locator;
		readonly VotingContext context;

		public VoteUpdater( IVoteCountLocator locator, VotingContext context )
		{
			this.locator = locator;
			this.context = context;
		}

		public void Update( Recording set, Vote vote )
		{
			var record = context.Create<Record>( x => x.Recording = set );
			record.Count = locator.Locate( vote );
			vote.Records.Add( record );
		}
	}
}