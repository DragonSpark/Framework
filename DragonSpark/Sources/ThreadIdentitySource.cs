using System;
using System.Collections.Concurrent;

namespace DragonSpark.Sources
{
	/// <summary>
	/// A little more performance-friendly than ThreadLocal...
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ThreadIdentitySource<T> : ConcurrentDictionary<int, T>, ISource<T>, IAssignable<T>
	{
		public T Get()
		{
			T current;
			var result = TryGetValue( Environment.CurrentManagedThreadId, out current ) ? current : default(T);
			return result;
		}

		object ISource.Get() => Get();

		public void Assign( T item ) => this[Environment.CurrentManagedThreadId] = item;
	}
}