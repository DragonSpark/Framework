using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Model;

public sealed class Signal : Variable<EventCallback?>
{
	public Task Fire()
	{
		var callback = Get();
		return callback is not null ? callback.Value.InvokeAsync() : Task.CompletedTask;
	}
}