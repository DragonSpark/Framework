using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class FactoryBuiltObjectFactory : ActivateFactory<ObjectFactoryParameter, object>
	{
		public FactoryBuiltObjectFactory() : this( Activation.Activator.Current )
		{}

		public FactoryBuiltObjectFactory( IActivator activator ) :	base( activator )
		{}

		protected override object Activate( ObjectFactoryParameter parameter )
		{
			var item = base.Activate( parameter );
			var result = item.AsTo<IFactory, object>( factory => factory.Create() ) ?? item.AsTo<IFactoryWithParameter, object>( factory => factory.Create( parameter.Context ) );
			return result;
		}
	}
}