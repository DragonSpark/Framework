using DevelopersWin.VoteReporter.Properties;
using DragonSpark.Windows;

namespace DevelopersWin.VoteReporter
{
	public interface IVoteReportContentGenerator
	{
		string Generate( VoteReport report );
	}

	class VoteReportContentGenerator : IVoteReportContentGenerator
	{
		readonly IDataTransformer transformer;
		readonly ISerializer serializer;

		public VoteReportContentGenerator( IDataTransformer transformer, ISerializer serializer )
		{
			this.transformer = transformer;
			this.serializer = serializer;
		}

		public string Generate( VoteReport report )
		{
			var stylesheet = DataBuilder.Create( Resources.Report );
			var data = serializer.Serialize( report );
			var source = DataBuilder.Create( data );
			var result = transformer.ToString( stylesheet, source );
			return result;
		}
	}
}