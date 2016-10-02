using System.IO;
using DragonSpark.Specifications;

namespace DragonSpark.Windows
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