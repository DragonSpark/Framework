using DragonSpark.Specifications;

namespace DragonSpark.Windows.FileSystem
{
	public abstract class FileExtensionSpecificationBase : SpecificationBase<IFileSystemInfo>
	{
		readonly string extension;
		protected FileExtensionSpecificationBase( string extension )
		{
			this.extension = extension;
		}

		public override bool IsSatisfiedBy( IFileSystemInfo parameter ) => parameter.Extension == extension;
	}
}