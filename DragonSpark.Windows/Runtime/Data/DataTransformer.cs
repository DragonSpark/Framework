using System;
using System.IO;

namespace DragonSpark.Windows.Runtime.Data
{
	public sealed class DataTransformer : DataTransformer<string>
	{
		public static DataTransformer Default { get; } = new DataTransformer();
		DataTransformer() : base( stream => new StreamReader( stream ).ReadToEnd() ) {}
	}

	public abstract class DataTransformer<T> : DataTransformerBase<T>
	{
		readonly Func<DataTransformParameter, MemoryStream> factory;
		readonly Func<MemoryStream, T> transformer;

		protected DataTransformer( Func<MemoryStream, T> transformer ) : this( DataStreamFactory.Default.Get, transformer ) {}

		protected DataTransformer( Func<DataTransformParameter, MemoryStream> factory, Func<MemoryStream, T> transformer )
		{
			this.factory = factory;
			this.transformer = transformer;
		}

		public override T Get( DataTransformParameter parameter )
		{
			var stream = factory( parameter );
			var result = transformer( stream );
			return result;
		}
	}
}