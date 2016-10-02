using System.IO;
using System.Xml.Xsl;

namespace DragonSpark.Windows.Runtime.Data
{
	public class DataStreamFactory : DataTransformerBase<MemoryStream>
	{
		public static DataStreamFactory Default { get; } = new DataStreamFactory();

		public override MemoryStream Get( DataTransformParameter parameter )
		{
			var transform = new XslCompiledTransform();
			transform.Load( parameter.StyleSheet );

			var stream = new MemoryStream();
			transform.Transform( parameter.Source, null, stream );
			stream.Seek( 0, SeekOrigin.Begin );
			return stream;
		}
	}
}