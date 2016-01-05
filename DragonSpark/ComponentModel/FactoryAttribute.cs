using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type factoryType, string name = null ) : base( () => new FactoryValueProvider( factoryType, name ) )
		{}
	}

	public class FactoryValueProvider : ActivatedValueProvider
	{
		readonly IFactory<ObjectFactoryParameter, object> factory;

		public FactoryValueProvider( Type activatedType, string name ) : this( Activation.Activator.Current, activatedType, name )
		{}

		public FactoryValueProvider( IActivator activator, Type activatedType, string name ) : this( activator, activator.Activate<FactoryBuiltObjectFactory>(), activatedType, name )
		{}

		public FactoryValueProvider( [Required]IActivator activator, [Required]IFactory<ObjectFactoryParameter, object> factory, [Required]Type activatedType, string name ) : base( activator, activatedType, name )
		{
			this.factory = factory;
		}

		protected override object Activate( Parameter parameter )
		{
			var result = factory.Create( new ObjectFactoryParameter( parameter.ActivatedType, FactoryReflectionSupport.GetResultType( parameter.ActivatedType ) ?? parameter.Metadata.PropertyType ) );
			return result;
		}
	}
}