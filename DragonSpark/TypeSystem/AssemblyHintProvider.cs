using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyHintProvider : ParameterizedSourceBase<Assembly, string>
	{
		public static AssemblyHintProvider Default { get; } = new AssemblyHintProvider();
		AssemblyHintProvider() {}

		public override string Get( Assembly parameter ) => parameter.From<AssemblyHintAttribute, string>( attribute => attribute.Hint ) ?? parameter.GetName().Name;
	}
}