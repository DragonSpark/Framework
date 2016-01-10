using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Patterns.Contracts;
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

	public interface IApplicationSetupParameter : ISetupParameter
	{
		IServiceLocator Locator { get; }
	}

	public interface ISetupParameter : IDisposable
	{
		IMessageLogger Logger { get; }

		object Arguments { get; }

		IReadOnlyCollection<object> Items { get; }

		void Monitor( Task task );

		void Register( object item );

		void RegisterForDispose( IDisposable disposable );
	}

	public class SetupParameter : SetupParameter<object>
	{
		public SetupParameter() : this( MessageLogger.Instance, null ) {}

		public SetupParameter( IMessageLogger logger, object arguments ) : base( logger, arguments ) {}
	}

	public class SetupParameter<TLogger, TArgument> : SetupParameter<TArgument> where TLogger : class, IMessageLogger, new()
	{
		readonly protected static Func<TLogger> DefaultLogger = MessageLoggerFactory<TLogger>.Instance.CreateUsing;

		public SetupParameter( TArgument arguments ) : this( DefaultLogger(), arguments ) {
		}
		public SetupParameter( TLogger logger, TArgument arguments ) : base( logger, arguments ) {}
	}

	public class MessageLoggerFactory<TLogger> : ActivateFactory<TLogger> where TLogger : class, IMessageLogger
	{
		public new static MessageLoggerFactory<TLogger> Instance { get; } = new MessageLoggerFactory<TLogger>();

		protected override TLogger Activate( ActivateParameter parameter ) => base.Activate( parameter ).Information( Resources.LoggerCreatedSuccessfully );
	}
	
	public class ApplicationSetupParameter<TLogger, TArgument> : SetupParameter<TLogger, TArgument>, IApplicationSetupParameter where TLogger : class, IMessageLogger, new()
	{
		public ApplicationSetupParameter( TArgument arguments ) : this( DefaultLogger(), arguments ) {}

		public ApplicationSetupParameter( TLogger logger, TArgument arguments ) : this( new ServiceLocatorFactory( logger ).CreateUsing(), logger, arguments )
		{}

		public ApplicationSetupParameter( IServiceLocator locator, TLogger logger, TArgument arguments ) : base( logger, arguments )
		{
			Locator = locator;
		}

		public IServiceLocator Locator { get; }
	}

	public class SetupParameter<TArgument> : ISetupParameter<TArgument>
	{
		readonly IList<object> items = new Collection<object>();
		readonly ICollection<Task> tasks = new List<Task>();
		readonly ICollection<IDisposable> disposables = new List<IDisposable>();

		public SetupParameter( [Required]IMessageLogger logger, TArgument arguments )
		{
			Logger = logger;
			Arguments = arguments;
			Items = new ReadOnlyCollection<object>( items );
			Register( arguments );
		}

		public IMessageLogger Logger { get; }

		public TArgument Arguments { get; }

		public void Register( object item ) => items.Add( item );

		public void RegisterForDispose( IDisposable item ) => disposables.Add( item );

		object ISetupParameter.Arguments => Arguments;

		public void Monitor( Task task ) => tasks.Add( task );

		public IReadOnlyCollection<object> Items { get; }
		
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