using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class SaveStateInitializer : IInitializer
{
	public static SaveStateInitializer Default { get; } = new();

	SaveStateInitializer() {}

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		await subject.SaveChangesAsync(stop).Off();
	}
}