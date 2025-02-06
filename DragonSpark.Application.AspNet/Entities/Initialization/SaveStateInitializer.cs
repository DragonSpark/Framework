using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class SaveStateInitializer : IInitializer
{
	public static SaveStateInitializer Default { get; } = new();

	SaveStateInitializer() {}

	public async ValueTask Get(DbContext parameter)
	{
		await parameter.SaveChangesAsync().Await();
	}
}