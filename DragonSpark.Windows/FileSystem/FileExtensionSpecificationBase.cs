using DragonSpark.Specifications;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public abstract class FileExtensionSpecificationBase : SpecificationBase<FileSystemInfoBase>
	{
		readonly string extension;
		protected FileExtensionSpecificationBase( string extension )
		{
			this.extension = extension;
		}

		public override bool IsSatisfiedBy( FileSystemInfoBase parameter ) => parameter.Extension == extension;
	}
}