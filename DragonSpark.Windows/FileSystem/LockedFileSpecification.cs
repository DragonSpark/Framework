using System.IO;
using DragonSpark.Specifications;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class LockedFileSpecification : SpecificationBase<FileInfo>
	{
		public static LockedFileSpecification Default { get; } = new LockedFileSpecification();
		LockedFileSpecification() {}

		public override bool IsSatisfiedBy( FileInfo parameter )
		{
			FileStream stream = null;

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