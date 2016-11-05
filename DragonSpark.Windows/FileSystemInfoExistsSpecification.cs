using DragonSpark.Specifications;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Windows
{
	public sealed class FileSystemInfoExistsSpecification : SpecificationBase<IFileSystemInfo>
	{
		public static FileSystemInfoExistsSpecification Default { get; } = new FileSystemInfoExistsSpecification();
		FileSystemInfoExistsSpecification() {}

		public override bool IsSatisfiedBy( IFileSystemInfo parameter ) => parameter.Refreshed().Exists;
	}
}