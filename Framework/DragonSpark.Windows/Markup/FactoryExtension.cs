using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public class FactoryExtension : LocateExtension
	{
		public FactoryExtension()
		{}

		public FactoryExtension( Type type ) : this( type, null ) 
		{}

		public FactoryExtension( Type type, object parameter ) : this( type, null, parameter )
		{}

		public FactoryExtension( Type type, string buildName, object parameter ) : base( type, buildName )
		{
			Parameter = parameter;
		}

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var result = DetermineFactory( serviceProvider ).Transform( x => Create( x, serviceProvider ) );
			return result;
		}

		protected virtual object Create( IFactory factory, IServiceProvider serviceProvider )
		{
			var type = serviceProvider.Get<DeferredContext>().PropertyType;
			var result = factory.Create( type, DetermineParameter( serviceProvider ) );
			return result;
		}

		protected virtual IFactory DetermineFactory( IServiceProvider serviceProvider )
		{
			var result = Instance ?? base.GetValue( serviceProvider ) as IFactory;
			return result;
		}

		protected virtual object DetermineParameter( IServiceProvider serviceProvider )
		{
			return Parameter;
		}

		public IFactory Instance { get; set; }

		public object Parameter { get; set; }
	}
}