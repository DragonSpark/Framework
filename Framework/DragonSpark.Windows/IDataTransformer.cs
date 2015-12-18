using System.Xml.XPath;

namespace DragonSpark.Windows
{
	public interface IDataTransformer
	{
		object Transform( IXPathNavigable stylesheet, IXPathNavigable source );

		string ToString( IXPathNavigable stylesheet, IXPathNavigable source );
	}
}