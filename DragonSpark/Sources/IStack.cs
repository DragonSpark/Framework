using System.Collections.Immutable;

namespace DragonSpark.Sources
{
	public interface IStack<T>
	{
		bool Contains( T item );

		ImmutableArray<T> All();

		T Peek();

		void Push( T item );

		T Pop();
	}
}