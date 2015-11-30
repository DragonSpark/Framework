using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.FactoryModel
{
	public class FactoryBuiltObjectFactory : ActivateFactory<ObjectFactoryParameter, object>
	{
		public FactoryBuiltObjectFactory( IActivator activator ) : base( activator )
		{}

		protected override object Activate( Type qualified, ObjectFactoryParameter parameter )
		{
			var item = base.Activate( qualified, parameter );
			var result = item.AsTo<IFactory, object>( factory => factory.Create() ) ?? item.AsTo<IFactoryWithParameter, object>( factory => factory.Create( parameter.Context ) );
			return result;
		}

		/*object DetermineResultFromContext( IFactoryWithParameter factory, ObjectFactoryParameter parameter )
		{
			var result = activated.Transform( factory.Create );
			return result;
		}*/
		
		/*protected override ObjectFactoryParameter QualifyParameter( object parameter )
		{
			return base.QualifyParameter( parameter ) ?? parameter.AsTo<Type, ObjectFactoryParameter>( type => type );
		}*/
	}
}