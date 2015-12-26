using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Setup
{
	public class SetupContext : IDisposable
	{
		readonly object arguments;
		readonly ICollection<Task> tasks = new List<Task>();

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
			arguments.GetType().Adapt().GuardAsAssignable<T>( "arguments" );
			
			var result = (T)arguments;
			return result;
		}

		public T Monitor<T>( T task ) where T : Task
		{
			tasks.Add( task );
			return task;
		}

		public IReadOnlyCollection<object> Items => new ReadOnlyCollection<object>( items );
		readonly IList<object> items = new Collection<object>();
		

		// public IMessageLogger MessageLogger => Item<IMessageLogger>();
		public void Dispose()
		{
			Task.WhenAll( tasks ).Wait();
			tasks.Clear();
			items.Clear();
		}
	}
}