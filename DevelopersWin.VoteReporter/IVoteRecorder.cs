using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Windows.Entity;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteRecorder
	{
		Recording Record();
	}

	class VoteRecorder : IVoteRecorder
	{
		readonly VotingContext context;
		readonly RecordingFactory factory;

		public VoteRecorder( VotingContext context, RecordingFactory factory )
		{
			this.context = context;
			this.factory = factory;
		}

		public Recording Record()
		{
			var result = factory.Create();
			context.Save();
			return result;
		}
	}
}