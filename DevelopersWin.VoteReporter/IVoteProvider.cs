using DevelopersWin.VoteReporter.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteProvider
	{
		IEnumerable<Vote> Retrieve( VoteRecording @set );
	}

		class VoteProvider : IVoteProvider
	{
		readonly VotingContext context;

		public VoteProvider( VotingContext context )
		{
			this.context = context;
		}

		public IEnumerable<Vote> Retrieve( VoteRecording @set )
		{
			var result = context.Votes.Where( vote => vote.Records.All( record => record.Set.Id != @set.Id ) );
			return result;
		}
	}

}