using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class EnsureCreated : IInitialize
{
	[UsedImplicitly]
	public static EnsureCreated Default { get; } = new ();

	EnsureCreated() {}

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		await parameter.Subject.Database.EnsureCreatedAsync(parameter).Off();
	}
}