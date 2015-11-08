using System.Globalization;
using DevelopersWin.VoteReporter.Entity;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteCountLocator
	{
		int Locate( Vote vote );
	}

	class VoteCountLocator : IVoteCountLocator
	{
		readonly DocumentProvider provider;

		public VoteCountLocator( DocumentProvider provider )
		{
			this.provider = provider;
		}

		public int Locate( Vote vote )
		{
			var document = provider.Load( vote.Location );
			var node = document.DocumentNode.SelectSingleNode( "//div[@class='uvIdeaVoteCount']/strong" );
			var data = node.InnerText;
			var result = int.Parse( data, NumberStyles.AllowThousands );
			return result;
		}
	}
}