using System.IO;
using System.Xaml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace DragonSpark.Windows.Runtime.Data
{
	public class DataTransformer : IDataTransformer
	{
		public static DataTransformer Instance { get; } = new DataTransformer();

		public object Transform( IXPathNavigable stylesheet, IXPathNavigable source )
		{
			var stream = DetermineStream( stylesheet, source );

			var result = XamlServices.Load( stream );
			return result;
		}

		static MemoryStream DetermineStream( IXPathNavigable stylesheet, IXPathNavigable source )
		{
			var transform = new XslCompiledTransform();
			transform.Load( stylesheet );

			var stream = new MemoryStream();
			transform.Transform( source, null, stream );
			stream.Seek( 0, SeekOrigin.Begin );
			return stream;
		}

		public string ToString( IXPathNavigable stylesheet, IXPathNavigable source )
		{
			var stream = DetermineStream( stylesheet, source );

			var result = new StreamReader( stream ).ReadToEnd();
			return result;
		}
	}
}