using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DragonSpark.Setup
{
	public interface ISetupParameter<out TArguments> : ISetupParameter
	{
		new TArguments Arguments { get; }
	}

	public interface ISetupParameter : IDisposable
	{
		object Arguments { get; }

		IReadOnlyCollection<object> Items { get; }

		void Monitor( Task task );

		void Register( object item );

		void RegisterForDispose( IDisposable disposable );
	}

	public class SetupParameter : SetupParameter<object>
	{
		public SetupParameter() : this( null )
		{}

		public SetupParameter( object arguments ) : base( arguments )
		{}
	}

	public class SetupParameter<TArgument> : ISetupParameter<TArgument>
	{
		readonly ICollection<Task> tasks = new List<Task>();
		readonly ICollection<IDisposable> disposables = new List<IDisposable>();

		public SetupParameter( TArgument arguments )
		{
			Arguments = arguments;
			Items = new ReadOnlyCollection<object>( items );
			Register( arguments );
		}

		public TArgument Arguments { get; }

		public void Register( object item )
		{
			items.Add( item );
		}

		public void RegisterForDispose( IDisposable item )
		{
			disposables.Add( item );
		}

		object ISetupParameter.Arguments => Arguments;

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