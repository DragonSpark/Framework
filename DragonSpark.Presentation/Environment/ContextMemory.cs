using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Presentation.Connections;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ContextMemory : MemoryVariable<HttpContext>
{
	public ContextMemory(ContextCache memory, IConnectionIdentifier identifier)
		: base(memory, identifier.Then().Select(x => x.ToString()), ContextStoreConfiguration.Default) {}
}