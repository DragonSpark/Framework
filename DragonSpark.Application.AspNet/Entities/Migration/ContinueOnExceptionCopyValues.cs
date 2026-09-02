using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ContinueOnExceptionCopyValues : Command<MapInput>
{
	public static ContinueOnExceptionCopyValues Default { get; } = new();

	ContinueOnExceptionCopyValues() : base(new CopyValues(ContinueOnExceptionAssignValue.Default)) {}
}