using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

	public class SetupParameter<TArgument> : ISetupParameter
	{
		readonly TArgument arguments;
		readonly ICollection<Task> tasks = new List<Task>();
		readonly ICollection<IDisposable> disposables = new List<IDisposable>();

		public SetupParameter() : this( default(TArgument) )
		{}

		public SetupParameter( TArgument arguments )
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