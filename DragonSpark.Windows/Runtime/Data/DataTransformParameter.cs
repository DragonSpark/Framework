using System.Xml.XPath;

namespace DragonSpark.Windows.Runtime.Data
{
	public struct DataTransformParameter
	{
		public DataTransformParameter( IXPathNavigable styleSheet, IXPathNavigable source )
		{
			StyleSheet = styleSheet;
			Source = source;
		}

		public IXPathNavigable Source { get; }

		public IXPathNavigable StyleSheet { get; }
	}
}