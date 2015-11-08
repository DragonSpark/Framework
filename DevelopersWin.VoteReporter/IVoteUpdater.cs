using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Windows.Entity;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteUpdater
	{
		void Update( VoteRecording set, Vote vote );
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

		public void Update( VoteRecording set, Vote vote )
		{
			var record = context.Create<VoteRecord>( x => x.Set = set );
			record.Count = locator.Locate( vote );
			vote.Records.Add( record );
		}
	}
}