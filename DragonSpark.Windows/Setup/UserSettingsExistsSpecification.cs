﻿using DragonSpark.Specifications;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Windows.Setup
{
	sealed class UserSettingsExistsSpecification : SuppliedDelegatedSpecification<IFileInfo>
	{
		public static UserSettingsExistsSpecification Default { get; } = new UserSettingsExistsSpecification();
		UserSettingsExistsSpecification() : base( FileSystemInfoExistsSpecification.Default, Defaults.UserSettingsPath ) {}
	}
}