using System.IO;

namespace DragonSpark.Runtime
{
	public interface ISerializer
	{
		T Load<T>( Stream data );

		string Save<T>( T item );
	}
}