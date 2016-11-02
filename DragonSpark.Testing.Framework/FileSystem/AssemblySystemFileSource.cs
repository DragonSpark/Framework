using System.IO;
using System.Reflection;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class AssemblySystemFileSource : ParameterizedSourceBase<Assembly, FileInfo>
	{
		public static IParameterizedSource<Assembly, FileInfo> Default { get; } = new AssemblySystemFileSource().Apply( Specification.Implementation );
		AssemblySystemFileSource() {}

		// ReSharper disable once AssignNullToNotNullAttribute
		public override FileInfo Get( Assembly parameter ) => new FileInfo( parameter.Location );

		public sealed class Specification : SpecificationBase<Assembly>
		{
			public static Specification Implementation { get; } = new Specification();
			Specification() {}

			public override bool IsSatisfiedBy( Assembly parameter ) => parameter.Location != null;
		}
	}
}