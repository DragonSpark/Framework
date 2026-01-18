using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() {}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;
		var values = from.CurrentValues.Properties.ToDictionary(x => x.Name, x => from.CurrentValues[x]);
		to.CurrentValues.SetValues(values);
	}
}