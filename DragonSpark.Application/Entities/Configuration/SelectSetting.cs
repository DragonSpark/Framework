using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Configuration;

sealed class SelectSetting : StartWhere<string, Setting>
{
	public static SelectSetting Default { get; } = new();

	SelectSetting() : base((p, x) => x.Id == p) {}
}