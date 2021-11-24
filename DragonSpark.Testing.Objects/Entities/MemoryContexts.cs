using DragonSpark.Application.Entities;
using DragonSpark.Application.Runtime;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class MemoryContexts<T> : Contexts<T> where T : DbContext
{
	public MemoryContexts() : this(IdentifyingText.Default.Get()) {}

	public MemoryContexts(string name) : base(new InMemoryDbContextFactory<T>(name)) {}
}