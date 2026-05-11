using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class MigrationRun : Text.Text
{
	public static MigrationRun Default { get; } = new();

	MigrationRun() : base(A.Type<MigrationRun>().FullName.Verify()) {}
}