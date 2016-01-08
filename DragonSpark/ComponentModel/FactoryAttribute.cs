using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System;
using Microsoft.Practices.ServiceLocation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type factoryType, string name = null ) : base( () => new ActivatedValueProvider( new ParameterFactory( factoryType, name ).Create, new Factory().Create ) )
		{}

		public class Factory : Factory<object>
		{
			readonly Func<FactoryBuiltObjectFactory> factory;

			public Factory() : this( Activator.Current ) {}

			public Factory( [Required]IActivator locator ) : this( locator.Activate<FactoryBuiltObjectFactory> ) { }

			protected Factory( [Required]Func<FactoryBuiltObjectFactory> factory )
			{
				this.factory = factory;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory().Create( new ObjectFactoryParameter( parameter.Item1.Type, FactoryReflectionSupport.GetResultType( parameter.Item1.Type ) ?? parameter.Item2.Metadata.PropertyType ) );
		}
	}
}