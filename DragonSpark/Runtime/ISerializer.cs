using PostSharp.Patterns.Contracts;
using System.IO;

namespace DragonSpark.Runtime
{
	public interface ISerializer
	{
		object Load( Stream data );

		string Save( object item );
	}

	/*public class DataContractSerializerFactory<T> : FactoryBase<DataContractSerializer>
	{
		readonly Func<Type, Type[]> knownTypes;

		public DataContractSerializerFactory() : this( new KnownTypeFactory( ) )
		{}

		public DataContractSerializerFactory( Func<Type, Type[]> knownTypes )
		{
			this.knownTypes = knownTypes;
		}

		[Cache]
		protected override DataContractSerializer CreateItem()
		{
			var result = new DataContractSerializer( typeof(T), knownTypes( typeof(T) ) );
			return result;
		}
	}*/

	/*public class Serializer<T> : ISerializer
	{
		public static Serializer<T> Instance { get; } = new Serializer<T>();

		readonly DataContractSerializer serializer;

		public Serializer() : this( DataContractSerializerFactory<T>. )
		{}

		public Serializer( [Required]DataContractSerializer serializer )
		{
			this.serializer = serializer;
		}

		public object Load( Stream data ) => serializer.ReadObject( data );

		public string Save( object item ) => new MemoryStream()
												.With( stream => serializer.WriteObject( stream, item ) )
												.With( stream => new StreamReader( stream ).ReadToEnd() );
	}*/

	public static class SerializerExtensions
	{
		public static T Load<T>( [Required]this ISerializer @this, Stream data ) => (T)@this.Load( data );
	}
}