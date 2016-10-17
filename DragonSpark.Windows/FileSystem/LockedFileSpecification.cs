using DragonSpark.Specifications;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class LockedFileSpecification : SpecificationBase<FileInfoBase>
	{
		public static LockedFileSpecification Default { get; } = new LockedFileSpecification();
		LockedFileSpecification() {}

		public override bool IsSatisfiedBy( FileInfoBase parameter )
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