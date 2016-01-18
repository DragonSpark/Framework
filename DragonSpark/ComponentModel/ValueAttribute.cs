using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public class ValueAttribute : ActivateAttributeBase
	{
		public ValueAttribute( Type activatedType, string name = null ) : base( new ActivatedValueProvider.Converter( activatedType, name ), Creator.Instance ) {}

		public class Creator : ActivatedValueProvider.Creator
		{
			public new static Creator Instance { get; } = new Creator();

			readonly Func<Tuple<ActivateParameter, DefaultValueParameter>, IValue> factory;

			public Creator() : this( new ActivatedValueProvider.Creator<IValue>().Create ) { }

			protected Creator( [Required]Func<Tuple<ActivateParameter, DefaultValueParameter>, IValue> factory )
			{
				this.factory = factory;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter ).Item;
		}
	}
}