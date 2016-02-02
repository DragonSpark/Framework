using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public class AmbientValueAttribute : ActivateAttributeBase
	{
		public AmbientValueAttribute( Type valueType = null, string name = null ) : base( new ActivatedValueProvider.Converter( valueType, name ), Creator.Instance ) {}

		public class Creator : ActivatedValueProvider.Creator
		{
			public new static Creator Instance { get; } = new Creator();

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => Ambient.GetCurrent( parameter.Item1.Type );
		}
	}

	public class ValueAttribute : ActivateAttributeBase
	{
		public ValueAttribute( [OfType( typeof(IValue) )]Type valueType, string name = null ) : base( new ActivatedValueProvider.Converter( valueType, name ), Creator.Instance ) {}

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