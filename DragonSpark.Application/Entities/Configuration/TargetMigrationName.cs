namespace DragonSpark.Application.Entities.Configuration;

public sealed class TargetMigrationName : SettingVariable
{
	public TargetMigrationName(ISettingAccessor accessor) : base(nameof(TargetMigrationName), accessor) {}
}