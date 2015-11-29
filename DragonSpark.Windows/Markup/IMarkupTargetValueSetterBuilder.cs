using System;
using System.Windows.Markup;
using DragonSpark.Activation;

namespace DragonSpark.Windows.Markup
{
	public interface IMarkupTargetValueSetterBuilder : IFactory<IProvideValueTarget, IMarkupTargetValueSetter>
	{
		bool Handles( IProvideValueTarget service );

		Type GetPropertyType( IProvideValueTarget parameter );
	}
}