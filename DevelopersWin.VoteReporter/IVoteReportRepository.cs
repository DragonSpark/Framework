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

		public VoteReportRepository( IStorage storage )
		{
			this.storage = storage;
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