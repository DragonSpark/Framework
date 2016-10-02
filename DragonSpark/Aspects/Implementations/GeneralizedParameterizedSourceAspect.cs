using DragonSpark.Sources.Parameterized;
using PostSharp.Aspects.Advices;

namespace DragonSpark.Aspects.Implementations
{
	[IntroduceInterface( typeof(IParameterizedSource<object, object>) )]
	public sealed class GeneralizedParameterizedSourceAspect : GeneralizedAspectBase, IParameterizedSource<object, object>
	{
		public object Get( object parameter ) => null;
	}
}