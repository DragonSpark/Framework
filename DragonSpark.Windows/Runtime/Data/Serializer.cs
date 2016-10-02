using DragonSpark.Runtime;
using System.IO;
using System.Xaml;

namespace DragonSpark.Windows.Runtime.Data
{
	public sealed class Serializer : ISerializer
	{
		// [Export]
		public static ISerializer Default { get; } = new Serializer();
		Serializer() {}

		public T Load<T>( Stream data ) => (T)XamlServices.Load( data );

		public string Save<T>( T item ) => XamlServices.Save( item );
	}
}