using DragonSpark.Model;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Model;

public class Selections<T> : List<SelectionListing<T>>
{
	public Selections() {}

	public Selections(IEnumerable<SelectionListing<T>> collection) : base(collection) {}

	public IEnumerable<T> Selected { get; set; } = Empty.Enumerable<T>();
}

// TODO

public sealed class Signal : Variable<EventCallback?>
{
	public Task Fire()
	{
		var callback = Get();
		return callback is not null ? callback.Value.InvokeAsync() : Task.CompletedTask;
	}
}