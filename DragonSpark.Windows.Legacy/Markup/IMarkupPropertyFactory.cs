using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Windows.Legacy.Markup
{
	public interface IMarkupPropertyFactory : IParameterizedSource<IServiceProvider, IMarkupProperty> {}
}