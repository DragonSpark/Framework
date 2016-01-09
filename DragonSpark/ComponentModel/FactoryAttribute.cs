using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type factoryType = null, string name = null ) : base( () => new ActivatedValueProvider( new ParameterFactory( p => factoryType ?? FactoryReflectionSupport.Instance.GetFactoryType( p.PropertyType ), name ).Create, new Factory().Create ) )
		{}

		public class Factory : Factory<object>
		{
			readonly Func<Type, object, object> factory;

			public Factory() : this( Activation.FactoryModel.Factory.From ) {}

			public Factory( [Required]Func<Type, object, object> factory )
			{
				this.factory = factory;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => 
				factory( parameter.Item1.Type, FactoryReflectionSupport.GetResultType( parameter.Item1.Type ) ?? parameter.Item2.Metadata.PropertyType );
		}
	}
}