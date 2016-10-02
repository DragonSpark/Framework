using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DragonSpark.Windows.Runtime
{
	public static class ThreadLocalStorage
	{
		public static T Get<T>( string key )
		{
			var slot = Thread.GetNamedDataSlot( key );
			return (T)Thread.GetData( slot );
		}

		/// <summary>
		/// store an object in the current thread storage area with the given key
		/// </summary>
		public static void Set( string key, object value )
		{
			var slot = Thread.GetNamedDataSlot( key );

			Thread.SetData( slot, value );
		}

		public static IDisposable Push<T>( T instance )
		{
			var stack = GetStack<T>();
			stack.Push( instance );
			return new DisposableAction( () => stack.Pop() );
		}

		public static T Peek<T>()
		{
			var stack = GetStack<T>();
			return stack.Any() ? (T)stack.Peek() : default(T);
		}

		static Dictionary<Type, Stack<object>> GetItems()
		{
			var key = typeof(ThreadLocalStorage).AssemblyQualifiedName;
			var result = Get<Dictionary<Type, Stack<object>>>( key );
			if ( result == null )
			{
				Set( key, result = new Dictionary<Type, Stack<object>>() );
			}
			return result;
		}

		static Stack<object> GetStack<T>()
		{
			var items = GetItems();
			Stack<object> stack;

			if ( !items.TryGetValue( typeof(T), out stack ) )
			{
				items.Add( typeof(T), stack = new Stack<object>() );
			}

			return stack;
		}
	}
}