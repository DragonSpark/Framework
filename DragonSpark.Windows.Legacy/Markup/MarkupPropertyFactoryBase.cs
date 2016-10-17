using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Windows.Legacy.Markup
{
	public abstract class MarkupPropertyFactoryBase : ParameterizedSourceBase<IServiceProvider, IMarkupProperty>, IMarkupPropertyFactory {}
}