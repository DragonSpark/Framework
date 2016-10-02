using System.IO;

namespace DragonSpark.Runtime
{
	public sealed class Serializer : ISerializer
	{
		// [Export]
		public static ISerializer Default { get; } = new Serializer();
		Serializer() {}

		public T Load<T>( Stream data ) => (T)DataContractSerializers.Default.Get( typeof(T) ).ReadObject( data );

		public string Save<T>( T item )
		{
			var stream = new MemoryStream();
			var type = typeof(T) == typeof(object) ? item.GetType() : typeof(T);
			DataContractSerializers.Default.Get( type ).WriteObject( stream, item );
			stream.Seek( 0, SeekOrigin.Begin );
			var result = new StreamReader( stream ).ReadToEnd();
			return result;
		}
	}
}