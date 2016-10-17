using DragonSpark.Specifications;
using System.IO;

namespace DragonSpark.Windows.FileSystem
{
	public abstract class FileExtensionSpecificationBase : SpecificationBase<FileSystemInfo>
	{
		readonly string extension;
		protected FileExtensionSpecificationBase( string extension )
		{
			this.extension = extension;
		}

		public override bool IsSatisfiedBy( FileSystemInfo parameter ) => parameter.Extension == extension;
	}
}