using DragonSpark.Application.Runtime;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class NewMemoryOptions<T> : SelectedResult<string, DbContextOptions<T>> where T : DbContext
{
	public static NewMemoryOptions<T> Default { get; } = new NewMemoryOptions<T>();

	NewMemoryOptions() : base(IdentifyingText.Default, MemoryOptions<T>.Default) {}
}