using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type factoryType = null, string name = null ) : base( new ActivatedValueProvider.Converter( p => factoryType ?? MemberInfoFactoryTypeLocator.Instance.Create( p ), name ), Creator.Instance )
		{}

		public class Creator : ActivatedValueProvider.Creator
		{
			public new static Creator Instance { get; } = new Creator();

			readonly Func<Type, object> factory;

			public Creator() : this( Factory.From ) {}

			public Creator( [Required]Func<Type, object> factory )
			{
				this.factory = factory;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter.Item1.Type );
		}
	}
}