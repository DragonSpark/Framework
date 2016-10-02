using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Windows.Markup
{
	public abstract class MarkupPropertyFactoryBase : ParameterizedSourceBase<IServiceProvider, IMarkupProperty>, IMarkupPropertyFactory {}
}