using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class ProcessDependents : ICommand<ProcessDependentsInput>
{
	public static ProcessDependents Default { get; } = new();

	ProcessDependents() {}

	public void Execute(ProcessDependentsInput parameter)
	{
		var (result, graph, references) = parameter;
		var entityTypes = references.Where(x => x.Value.Count == 0).Select(x => x.Key).Except(result);
		var queue       = new Queue<IEntityType>(entityTypes);
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			result.Add(current);

			foreach (var dependent in graph.Get(current).OrderBy(x => references[x].Count))
			{
				var types = references[dependent];
				types.ExceptWith(result);
				switch (types.Count)
				{
					case 0:
						queue.Enqueue(dependent);
						break;
				}
			}
		}
	}
}