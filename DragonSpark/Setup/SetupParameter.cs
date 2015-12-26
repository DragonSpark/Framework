using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Setup
{
	public interface ISetupParameter : IDisposable
	{
		object GetArguments();

		IReadOnlyCollection<object> Items { get; }

		void Monitor( Task task );

		void Register( object item );

		void RegisterForDispose( IDisposable disposable );
	}

	public static class SetupParameterExtensions
	{
		public static TItem Register<TItem>( this ISetupParameter @this, TItem item )
		{
			@this.Register( item );
			return item;
		}

		public static TItem RegisterForDispose<TItem>( this ISetupParameter @this, TItem item ) where TItem : IDisposable
		{
			@this.RegisterForDispose( item );
			return item;
		}

		public static TItem Item<TItem>( this ISetupParameter @this )
		{
			var result = @this.Items.OfType<TItem>().SingleOrDefault();
			return result;
		}

		public static T GetArguments<T>( this ISetupParameter @this )
		{
			var arguments = @this.GetArguments();
			arguments.GetType().Adapt().GuardAsAssignable<T>( nameof(arguments) );

			var result = (T)arguments;
			return result;
		}

		public static T Monitor<T>( this ISetupParameter @this, T task ) where T : Task
		{
			@this.Monitor( task );
			return task;
		}
	}

	public class SetupParameter : ISetupParameter
	{
		readonly object arguments;
		readonly ICollection<Task> tasks = new List<Task>();
		readonly ICollection<IDisposable> disposables = new List<IDisposable>();

		public SetupParameter() : this( Enumerable.Empty<object>() )
		{}

		public SetupParameter( object arguments )
		{
			this.arguments = arguments;
			Items = new ReadOnlyCollection<object>( items );
			Register( arguments );
		}

		public void Register( object item )
		{
			items.Add( item );
		}

		public void RegisterForDispose( IDisposable item )
		{
			disposables.Add( item );
		}

		public object GetArguments()
		{
			return arguments;
		}

		public void Monitor( Task task )
		{
			tasks.Add( task );
		}

		public IReadOnlyCollection<object> Items { get; }
		readonly IList<object> items = new Collection<object>();
		
		public void Dispose()
		{
			Task.WhenAll( tasks ).Wait();
			tasks.Clear();
			items.Clear();
			disposables.Each( disposable => disposable.Dispose() );
			disposables.Clear();
		}
	}
}