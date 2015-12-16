using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type factoryType ) : base( typeof(FactoryValueProvider), factoryType )
		{}
	}

	public class FactoryValueProvider : ActivatedValueProvider
	{
		readonly IFactory<ObjectFactoryParameter, object> factory;

		public FactoryValueProvider( Type activatedType, string name ) : base( activatedType, name )
		{}

		public FactoryValueProvider( IActivator activator, Type activatedType, string name ) : this( activator, activator.Activate<FactoryBuiltObjectFactory>(), activatedType, name )
		{
		}

		public FactoryValueProvider( IActivator activator, IFactory<ObjectFactoryParameter, object> factory, Type activatedType, string name ) : base( activator, activatedType, name )
		{
			this.factory = factory;
		}

		protected override object Activate( DefaultValueParameter parameter, Type qualified )
		{
			var result = factory.Create( new ObjectFactoryParameter( qualified, FactoryReflectionSupport.Instance.GetResultType( qualified ) ?? parameter.Metadata.PropertyType ) );
			return result;
		}
	}
}