using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public class Initializers : IInitializer
{
	readonly IInitializer[] _initializers;

	protected Initializers(params IInitializer[] initializers) => _initializers = initializers;

	public async ValueTask Get(DbContext parameter)
	{
		foreach (var initializer in _initializers)
		{
			await initializer.Await(parameter);
		}
	}
}