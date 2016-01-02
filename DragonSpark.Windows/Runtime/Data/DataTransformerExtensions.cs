using System.Xml.XPath;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Runtime.Data
{
	public static class DataTransformerExtensions
	{
		public static TResult Transform<TResult>( this IDataTransformer target, string stylesheetXml, string sourceXml ) where TResult : class
		{
			var stylesheet = DataBuilder.Create( stylesheetXml );
			var source = DataBuilder.Create( sourceXml );

			var result = target.Transform<TResult>( stylesheet, source );
			return result;
		}

		public static TResult Transform<TResult>( this IDataTransformer target, IXPathNavigable stylesheet, IXPathNavigable source ) where TResult : class
		{
			var result = target.Transform( stylesheet, source ).To<TResult>();
			return result;
		}
	}
}