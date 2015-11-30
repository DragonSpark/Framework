using System;
using System.Windows.Markup;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Windows.Markup
{
	public interface IMarkupTargetValueSetterBuilder : IFactory<IProvideValueTarget, IMarkupTargetValueSetter>
	{
		bool Handles( IProvideValueTarget service );

		Type GetPropertyType( IProvideValueTarget parameter );
	}
}