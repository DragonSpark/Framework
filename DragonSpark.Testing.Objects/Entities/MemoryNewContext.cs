using DragonSpark.Application.Entities;
using DragonSpark.Application.Runtime;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class MemoryNewContext<T> : NewContext<T> where T : DbContext
{
	public MemoryNewContext() : this(IdentifyingText.Default.Get()) {}

	public MemoryNewContext(string name) : base(new InMemoryDbContextFactory<T>(name)) {}
}