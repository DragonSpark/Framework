using DragonSpark.Specifications;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	public sealed class FileSystemInfoExistsSpecification : SpecificationBase<FileSystemInfo>
	{
		public static FileSystemInfoExistsSpecification Default { get; } = new FileSystemInfoExistsSpecification();
		FileSystemInfoExistsSpecification() {}

		public override bool IsSatisfiedBy( FileSystemInfo parameter ) => parameter.Refreshed().Exists;
	}

	public static class Extensions
	{
		// public static T Refreshed<T>( this Func<T> @this ) where T : FileSystemInfo => @this().Refreshed();

		public static T Refreshed<T>( this T @this ) where T : FileSystemInfo
		{
			@this.Refresh();
			return @this;
		}
	}
}