using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type factoryType = null, string name = null ) : base( new ActivatedValueProvider.Converter( p => factoryType ?? FactoryReflectionSupport.Instance.GetFactoryType( p.PropertyType ), name ), new Creator() )
		{}

		public class Creator : ActivatedValueProvider.Creator
		{
			readonly Func<Type, object, object> factory;

			public Creator() : this( Factory.From ) {}

			public Creator( [Required]Func<Type, object, object> factory )
			{
				this.factory = factory;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => 
				factory( parameter.Item1.Type, FactoryReflectionSupport.GetResultType( parameter.Item1.Type ) ?? parameter.Item2.Metadata.PropertyType );
		}
	}
}