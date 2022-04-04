using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public sealed class EnsureCreated : IInitialize
{
	[UsedImplicitly]
	public static EnsureCreated Default { get; } = new ();

	EnsureCreated() {}

	public async ValueTask Get(DbContext parameter)
	{
		await parameter.Database.EnsureCreatedAsync().ConfigureAwait(false);
	}
}