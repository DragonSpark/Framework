using System.Composition;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Testing.Objects.Composition
{
	[Export]
	public class ParameterServiceFactory : ParameterizedSourceBase<Parameter, IParameterService>
	{
		public override IParameterService Get( Parameter parameter ) => new ParameterService( parameter );
	}
}