using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Setup;
using System;
using System.Linq;

namespace DevelopersWin.VoteReporter.Configuration.Development
{
	public class Module : Module<Setup>
	{
		public Module( IModuleMonitor moduleMonitor, SetupContext context ) : base( moduleMonitor, context )
		{}
	}

	class VoteCountLocator : IVoteCountLocator
	{
		public int Locate( Vote vote )
		{
			var minimum = vote.Records.OrderByDescending( record => record.Created ).FirstOrDefault().Transform( x => x.Count );
			var result = new Random().Next( minimum + 5, minimum + 150 );
			return result;
		}
	}
}
