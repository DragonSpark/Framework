using DevelopersWin.VoteReporter.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteProvider
	{
		IEnumerable<Vote> Retrieve( Recording @set );
	}

		class VoteProvider : IVoteProvider
	{
		readonly VotingContext context;

		public VoteProvider( VotingContext context )
		{
			this.context = context;
		}

		public IEnumerable<Vote> Retrieve( Recording @set )
		{
			var result = context.Votes.Where( vote => vote.Records.All( record => record.Recording.Id != @set.Id ) );
			return result;
		}
	}

}