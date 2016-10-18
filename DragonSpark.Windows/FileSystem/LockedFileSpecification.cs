using DragonSpark.Specifications;
using System.IO;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class LockedFileSpecification : SpecificationBase<IFileInfo>
	{
		public static LockedFileSpecification Default { get; } = new LockedFileSpecification();
		LockedFileSpecification() {}

		public override bool IsSatisfiedBy( IFileInfo parameter )
		{
			Stream stream = null;

			try
			{
				stream = parameter.Open( FileMode.Open, FileAccess.Read, FileShare.None );
			}
			catch ( IOException )
			{
				return true;
			}
			finally
			{
				stream?.Close();
			}

			return false;
		}
	}
}