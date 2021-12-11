using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Resize;

public interface IResizeMonitor : IAsyncDisposable
{
	ValueTask<bool> Add(ElementReference element);

	ValueTask Remove(ElementReference element);
}