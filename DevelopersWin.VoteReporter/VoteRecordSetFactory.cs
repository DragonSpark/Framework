using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Windows.Entity;
using System;
using System.Linq;

namespace DevelopersWin.VoteReporter
{
	public class VoteRecordSetFactory : Factory<Recording>
	{
		readonly VotingContext context;
		readonly IVoteProvider provider;
		readonly IVoteUpdater updater;

		public VoteRecordSetFactory( VotingContext context, IVoteProvider provider, IVoteUpdater updater )
		{
			this.context = context;
			this.provider = provider;
			this.updater = updater;
		}

		protected override Recording CreateFrom( Type resultType, object parameter )
		{
			var set = context.Create<Recording>();
			var array = provider.Retrieve( set ).ToArray();
			set.Records = array.Select( vote => vote.With( x => updater.Update( set, vote ) ).Records.OrderByDescending( record => record.Created ).First() ).ToList();
			return set;
		}
	}
}