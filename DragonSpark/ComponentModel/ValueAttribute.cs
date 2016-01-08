using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public class ValueAttribute : ActivateAttribute
	{
		public ValueAttribute( Type activatedType, string name = null ) : base( () => new ActivatedValueProvider( new ParameterFactory( activatedType, name ).Create, new Factory().Create ) )
		{}

		public class Factory : Factory<object>
		{
			readonly Func<Tuple<ActivateParameter, DefaultValueParameter>, IValue> factory;

			public Factory() : this( new Factory<IValue>().Create ) { }

			protected Factory( [Required]Func<Tuple<ActivateParameter, DefaultValueParameter>, IValue> factory )
			{
				this.factory = factory;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter ).Item;
		}
	}
}