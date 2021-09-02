using DragonSpark.Application.Runtime;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class InMemoryDbContextFactory<T> : DbContextFactory<T> where T : DbContext
	{
		public InMemoryDbContextFactory() : this(IdentifyingText.Default.Get()) {}

		public InMemoryDbContextFactory(string name) : base(MemoryOptions<T>.Default.Get(name)) {}
	}
}