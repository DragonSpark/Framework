using System;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteReportGenerator
	{
		Uri Generate();
	}

	class VoteReportGenerator : IVoteReportGenerator
	{
		readonly IVoteReportContentGenerator generator;
		readonly IStorage storage;
		readonly VoteReportFactory factory;

		public VoteReportGenerator( IVoteReportContentGenerator generator, IStorage storage, VoteReportFactory factory )
		{
			this.generator = generator;
			this.storage = storage;
			this.factory = factory;
		}


		public Uri Generate()
		{
			var report = factory.Create();
			var content = generator.Generate( report );
			var result = storage.Save( content );
			return result;
		}
	}
}