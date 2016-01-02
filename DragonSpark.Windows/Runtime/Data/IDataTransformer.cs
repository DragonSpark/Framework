using System.Xml.XPath;

namespace DragonSpark.Windows.Runtime.Data
{
	public interface IDataTransformer
	{
		object Transform( IXPathNavigable stylesheet, IXPathNavigable source );

		string ToString( IXPathNavigable stylesheet, IXPathNavigable source );
	}
}