using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.Setup
{
	public class SetupContext : IDisposable
	{
		readonly object arguments;
		
		public SetupContext( object arguments )
		{
			this.arguments = arguments ?? Enumerable.Empty<object>();
			Register( arguments );
		}

		public TItem Register<TItem>( TItem item )
		{
			items.Add( item );
			return item;
		}

		public virtual TItem Item<TItem>()
		{
			var result = Items.OfType<TItem>().SingleOrDefault();
			return result;
		}

		public object GetArguments()
		{
			var result = GetArguments<object>();
			return result;
		}

		public T GetArguments<T>()
		{
			arguments.GetType().Extend().GuardAsAssignable<T>( "arguments" );
			
			var result = (T)arguments;
			return result;
		}

		public IReadOnlyCollection<object> Items => new ReadOnlyCollection<object>( items );
		readonly IList<object> items = new Collection<object>();

		public ILogger Logger => Item<ILogger>();
		public void Dispose()
		{
			items.Clear();
		}
	}
}