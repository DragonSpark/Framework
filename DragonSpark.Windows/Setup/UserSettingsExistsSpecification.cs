using DragonSpark.Specifications;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	sealed class UserSettingsExistsSpecification : SuppliedDelegatedSpecification<FileInfo>
	{
		public static UserSettingsExistsSpecification Default { get; } = new UserSettingsExistsSpecification();
		UserSettingsExistsSpecification() : base( FileSystemInfoExistsSpecification.Default, Defaults.UserSettingsPath ) {}
	}
}