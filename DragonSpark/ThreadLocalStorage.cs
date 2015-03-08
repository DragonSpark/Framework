using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DragonSpark.Runtime;

namespace DragonSpark
{
	// Propz: http://www.codeplex.com/unity/Thread/View.aspx?ThreadId=24944
	public static class ThreadLocalStorage
	{
		/// <summary>
		/// retrieve an object associated w/ the current thread by key
		/// </summary>
		/// <typeparam name="T">the type of the object to retrieve</typeparam>
		public static T GetThreadObject<T>(string key)
		{
			LocalDataStoreSlot slot = Thread.GetNamedDataSlot(key);
			return slot != null ? (T)Thread.GetData(slot) : default(T);
		}

		/// <summary>
		/// store an object in the current thread storage area with the given key
		/// </summary>
		public static void SetThreadObject(string key, object value)
		{
			LocalDataStoreSlot slot = Thread.GetNamedDataSlot(key);
			if (slot == null)
			{
				slot = Thread.AllocateNamedDataSlot(key);
			}

			Thread.SetData(slot, value);
		}

		public static IDisposable Push<T>(T instance)
		{
			Stack<object> stack = GetStack<T>();
			stack.Push(instance);

			return new DisposableActionContext( () => stack.Pop() );
		}

		public static T Peek<T>()
		{
			Stack<object> stack = GetStack<T>();

			return stack.Count == 0 ? default(T) : (T)stack.Peek();
		}

		static Dictionary<Type, Stack<object>> ResolveItems()
		{
			string key = typeof(ThreadLocalStorage).AssemblyQualifiedName;
			Dictionary<Type, Stack<object>> result = GetThreadObject<Dictionary<Type, Stack<object>>>( key );
			if ( result == null )
			{
				SetThreadObject( key, result = new Dictionary<Type, Stack<object>>( ) );
			}
			return result;
		}

		static Stack<object> GetStack<T>()
		{
			Dictionary<Type, Stack<object>> items = ResolveItems();
			Stack<object> stack;

			if (!items.TryGetValue(typeof(T), out stack))
			{
				items.Add( typeof(T), stack = new Stack<object>() );
			}

			return stack;
		}
	}
}