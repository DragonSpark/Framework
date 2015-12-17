using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using System;
using PostSharp.Patterns.Contracts;

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

		public FactoryValueProvider( Type activatedType, string name ) : this( Activation.Activator.Current, activatedType, name )
		{}

		public FactoryValueProvider( IActivator activator, Type activatedType, string name ) : this( activator, activator.Activate<FactoryBuiltObjectFactory>(), activatedType, name )
		{
		}

		public FactoryValueProvider( [Required]IActivator activator, [Required]IFactory<ObjectFactoryParameter, object> factory, [Required]Type activatedType, string name ) : base( activator, activatedType, name )
		{
			this.factory = factory;
		}

		protected override object Activate( Parameter parameter )
		{
			var result = factory.Create( new ObjectFactoryParameter( parameter.ActivatedType, FactoryReflectionSupport.Instance.GetResultType( parameter.ActivatedType ) ?? parameter.Metadata.PropertyType ) );
			return result;
		}
	}
}