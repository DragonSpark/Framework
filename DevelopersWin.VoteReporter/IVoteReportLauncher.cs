using DragonSpark.Windows;
using System;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteReportLauncher
	{
		void Launch( Uri location );
	}

	class VoteReportLauncher : IVoteReportLauncher
	{
		public void Launch( Uri location )
		{
			Process.Create( location.LocalPath );
		}
	}
}