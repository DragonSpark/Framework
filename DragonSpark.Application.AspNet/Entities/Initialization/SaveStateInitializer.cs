using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public sealed class SaveStateInitializer : IInitializer
{
	public static SaveStateInitializer Default { get; } = new();

	SaveStateInitializer() {}

	public async ValueTask Get(DbContext parameter)
	{
		await parameter.SaveChangesAsync().Await();
	}
}