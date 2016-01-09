using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Markup
{
	public class FactoryExtension : FactoryExtension<ObjectFactoryParameter, object>
	{
		public FactoryExtension()
		{}

		public FactoryExtension( Type type ) : base( type )
		{}

		public FactoryExtension( Type type, object parameter ) : base( type, parameter )
		{}

		public FactoryExtension( Type type, string buildName, object parameter ) : base( type, buildName, parameter )
		{}

		protected override IFactory<ObjectFactoryParameter, object> DetermineFactory( IServiceProvider serviceProvider ) => Activator.GetCurrent().Activate<FactoryBuiltObjectFactory>();

		protected override ObjectFactoryParameter DetermineParameter( IServiceProvider serviceProvider ) => new ObjectFactoryParameter( Type, Parameter ?? serviceProvider.Get<DeferredContext>().PropertyType );
	}

	public abstract class FactoryExtension<TParameter, TResult> : LocateExtension
	{
		protected FactoryExtension()
		{}

		protected FactoryExtension( Type type, object parameter = null ) : this( type, null, parameter )
		{}

		protected FactoryExtension( Type type, string buildName, object parameter ) : base( type, buildName )
		{
			Parameter = parameter;
		}

		protected override object GetValue( IServiceProvider serviceProvider ) => DetermineFactory( serviceProvider ).Create( DetermineParameter( serviceProvider ) );

		protected abstract IFactory<TParameter, TResult> DetermineFactory( IServiceProvider serviceProvider );

		protected abstract TParameter DetermineParameter( IServiceProvider serviceProvider );

		public object Parameter { get; set; }
	}
}