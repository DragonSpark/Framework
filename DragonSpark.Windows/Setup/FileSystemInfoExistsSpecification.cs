using DragonSpark.Specifications;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	public sealed class FileSystemInfoExistsSpecification : SpecificationBase<FileSystemInfoBase>
	{
		public static FileSystemInfoExistsSpecification Default { get; } = new FileSystemInfoExistsSpecification();
		FileSystemInfoExistsSpecification() {}

		public override bool IsSatisfiedBy( FileSystemInfoBase parameter ) => parameter.Refreshed().Exists;
	}
}