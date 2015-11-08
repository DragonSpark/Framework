using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Windows.Entity;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteRecorder
	{
		VoteRecording Record();
	}

	class VoteRecorder : IVoteRecorder
	{
		readonly VotingContext context;
		readonly VoteRecordSetFactory factory;

		public VoteRecorder( VotingContext context, VoteRecordSetFactory factory )
		{
			this.context = context;
			this.factory = factory;
		}

		public VoteRecording Record()
		{
			var result = factory.Create();
			context.Save();
			return result;
		}
	}
}