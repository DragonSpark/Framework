using DevelopersWin.VoteReporter.Entity;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteCountLocator
	{
		int Locate( Vote vote );
	}
}