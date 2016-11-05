using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public struct StackAssignment<T> : IDisposable
	{
		readonly IStackSource<T> source;

		public StackAssignment( IStackSource<T> source, T item )
		{
			this.source = source;
			source.Get().Push( item );
		}

		public void Dispose() => source.Get().Pop();
	}
}