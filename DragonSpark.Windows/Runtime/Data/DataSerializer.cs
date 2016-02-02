using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using System;
using System.IO;
using System.Xaml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace DragonSpark.Windows.Runtime.Data
{
	public class Serializer : ISerializer
	{
		public static Serializer Instance { get; } = new Serializer();

		public object Load( Stream data ) => XamlServices.Load( data );

		public string Save( object item ) => XamlServices.Save( item );
	}

	public abstract class DataTransformerBase<T> : FactoryBase<DataTransformParameter, T> {}

	public abstract class DataTransformer<T> : DataTransformerBase<T>
	{
		readonly Func<DataTransformParameter, MemoryStream> factory;
		readonly Func<MemoryStream, T> transformer;

		protected DataTransformer( Func<MemoryStream, T> transformer ) : this( DataStreamFactory.Instance.Create, transformer ) {}

		protected DataTransformer( [Required]Func<DataTransformParameter, MemoryStream> factory, [Required]Func<MemoryStream, T> transformer )
		{
			this.factory = factory;
			this.transformer = transformer;
		}

		protected override T CreateItem( DataTransformParameter parameter )
		{
			var stream = factory( parameter );
			var result = transformer( stream );
			return result;
		}
	}

	public class DataTransformer : DataTransformer<string>
	{
		public static DataTransformer Instance { get; } = new DataTransformer();

		public DataTransformer() : base( stream => new StreamReader( stream ).ReadToEnd() )
		{}
	}

	public class DataSerializer : DataSerializer<object>
	{
		public new static DataSerializer Instance { get; } = new DataSerializer();
	}

	public class DataSerializer<T> : DataTransformer<T>
	{
		public static DataSerializer<T> Instance { get; } = new DataSerializer<T>();

		public DataSerializer() : this( Serializer.Instance ) {}

		public DataSerializer( ISerializer serializer ) : base( serializer.Load<T> ) {}
	}

	public class DataStreamFactory : DataTransformerBase<MemoryStream>
	{
		public static DataStreamFactory Instance { get; } = new DataStreamFactory();

		protected override MemoryStream CreateItem( DataTransformParameter parameter )
		{
			var transform = new XslCompiledTransform();
			transform.Load( parameter.Stylesheet );

			var stream = new MemoryStream();
			transform.Transform( parameter.Source, null, stream );
			stream.Seek( 0, SeekOrigin.Begin );
			return stream;
		}
	}

	public class DataTransformParameter
	{
		public DataTransformParameter( IXPathNavigable stylesheet, IXPathNavigable source )
		{
			Stylesheet = stylesheet;
			Source = source;
		}

		public IXPathNavigable Source { get; }

		public IXPathNavigable Stylesheet { get; }
	}
}