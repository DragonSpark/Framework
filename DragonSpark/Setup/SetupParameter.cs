using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Setup
{
	/*public interface ISetupParameter<out TArguments> : ISetupParameter
	{
		new TArguments Arguments { get; }
	}

	public interface ISetupParameter : IDisposable
	{
		/*IMessageLogger Logger { get; }#1#

		object Arguments { get; }

		IReadOnlyCollection<object> Items { get; }

		void Monitor( Task task );

		void Register( object item );

		// void RegisterForDispose( IDisposable disposable );
	}*/

	public interface ITaskMonitor : IDisposable
	{
		void Monitor( Task task );
	}

	// [Persistent]
	public class TaskMonitor : ITaskMonitor
	{
		readonly ICollection<Task> tasks = new List<Task>();

		public void Monitor( Task task ) => tasks.Add( task );

		public void Dispose()
		{
			Task.WhenAll( tasks ).Wait();
			tasks.Clear();
		}
	}

	/*public class SetupParameter : SetupParameter<object>
	{
		public SetupParameter() : this( MessageLogger.Instance, null ) {}

		public SetupParameter( IMessageLogger logger, object arguments ) : base( logger, arguments ) {}
	}*/

	public class MessageLoggerFactory<TLogger> : ActivateFactory<TLogger> where TLogger : class, IMessageLogger
	{
		public new static MessageLoggerFactory<TLogger> Instance { get; } = new MessageLoggerFactory<TLogger>();

		protected override TLogger Activate( ActivateParameter parameter ) => base.Activate( parameter ).Information( Resources.LoggerCreatedSuccessfully );
	}

	/*public class SetupParameter<TArgument> : ISetupParameter<TArgument>
	{
		readonly IList<object> items = new Collection<object>();
		readonly ICollection<Task> tasks = new List<Task>();
		// readonly ICollection<IDisposable> disposables = new List<IDisposable>();

		public SetupParameter( TArgument arguments )
		{
			// Logger = logger;
			Arguments = arguments;
			Items = new ReadOnlyCollection<object>( items );
			// Register( arguments );
		}

		// public IMessageLogger Logger { get; }

		public TArgument Arguments { get; }

		// public void Register( object item ) => items.Add( item );

		// public void RegisterForDispose( IDisposable item ) => disposables.Add( item );

		object ISetupParameter.Arguments => Arguments;

		public void Monitor( Task task ) => tasks.Add( task );

		public IReadOnlyCollection<object> Items { get; }
		
		public void Dispose()
		{
			Task.WhenAll( tasks ).Wait();
			tasks.Clear();
			items.Clear();
			/*disposables.Each( disposable => disposable.Dispose() );
			disposables.Clear();#1#
		}
	}*/
}