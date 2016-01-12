using System;
using PostSharp.Patterns.Contracts;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;

namespace DragonSpark.Runtime
{
	public interface ISerializer
	{
		object Load( Stream data );

		string Save( object item );
	}

	public class DataContractSerializerFactory<T> : FactoryBase<DataContractSerializer>
	{
		readonly Func<Type, Type[]> knownTypes;

		public DataContractSerializerFactory( KnownTypeFactory factory ) : this( factory.Create )
		{}

		public DataContractSerializerFactory( [Required]Func<Type, Type[]> knownTypes )
		{
			this.knownTypes = knownTypes;
		}

		[Freeze]
		protected override DataContractSerializer CreateItem() => typeof(T).With( type => new DataContractSerializer( type, knownTypes( type ) ) );
	}

	/*public class Serializer<T> : ISerializer
	{
		public static Serializer<T> Instance { get; } = new Serializer<T>();

		readonly DataContractSerializer serializer;

		public Serializer() : this( DataContractSerializerFactory<T> )
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