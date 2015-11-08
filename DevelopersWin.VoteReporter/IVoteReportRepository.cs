using DragonSpark.Extensions;
using System.IO;
using System.Linq;
using System.Xaml;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteReportRepository
	{
		// VoteReport GetLatest();

		void Save( VoteReport report );
	}

	class VoteReportRepository : IVoteReportRepository
	{
		readonly IStorage storage;
		readonly DirectoryInfo directory;

		public VoteReportRepository( IStorage storage, DirectoryInfo directory )
		{
			this.storage = storage;
			this.directory = directory;
		}

		/*public VoteReport GetLatest()
		{
			var result = directory.GetFiles( "*.xaml" ).OrderByDescending( info => info.CreationTimeUtc ).FirstOrDefault().Transform( x => (VoteReport)XamlServices.Load( x.FullName ) );
			return result;
		}*/

		public void Save( VoteReport report )
		{
			storage.Save( report );
		}
	}
}