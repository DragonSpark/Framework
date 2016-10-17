using DragonSpark.Specifications;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	sealed class UserSettingsExistsSpecification : SuppliedDelegatedSpecification<FileInfoBase>
	{
		public static UserSettingsExistsSpecification Default { get; } = new UserSettingsExistsSpecification();
		UserSettingsExistsSpecification() : base( FileSystemInfoExistsSpecification.Default, Defaults.UserSettingsPath ) {}
	}
}