using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyPartQueryProvider : ParameterizedSourceBase<Assembly, string>
	{
		public static AssemblyPartQueryProvider Default { get; } = new AssemblyPartQueryProvider();
		AssemblyPartQueryProvider() {}

		public override string Get( Assembly parameter ) => parameter.From<AssemblyPartsAttribute, string>( attribute => attribute.Query ) ?? AssemblyPartsAttribute.Default;
	}
}