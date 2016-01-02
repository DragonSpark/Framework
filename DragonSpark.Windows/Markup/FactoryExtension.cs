using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using System;
using DragonSpark.Activation.FactoryModel;
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

		protected override IFactory<ObjectFactoryParameter, object> DetermineFactory( IServiceProvider serviceProvider )
		{
			return Activator.Current.Activate<FactoryBuiltObjectFactory>();
		}

		protected override ObjectFactoryParameter DetermineParameter( IServiceProvider serviceProvider )
		{
			var type = serviceProvider.Get<DeferredContext>().PropertyType;
			var context = new ObjectFactoryParameter( Type, Parameter ?? type );
			return context;
		}
	}

	public abstract class FactoryExtension<TParameter, TResult> : LocateExtension
	{
		protected FactoryExtension()
		{}

		protected FactoryExtension( Type type ) : this( type, null ) 
		{}

		protected FactoryExtension( Type type, object parameter ) : this( type, null, parameter )
		{}

		protected FactoryExtension( Type type, string buildName, object parameter ) : base( type, buildName )
		{
			Parameter = parameter;
		}

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var parameter = DetermineParameter( serviceProvider );
			var result = DetermineFactory( serviceProvider ).Create( parameter );
			return result;
		}

		protected abstract IFactory<TParameter, TResult> DetermineFactory( IServiceProvider serviceProvider );

		protected abstract TParameter DetermineParameter( IServiceProvider serviceProvider );

		public object Parameter { get; set; }
	}
}