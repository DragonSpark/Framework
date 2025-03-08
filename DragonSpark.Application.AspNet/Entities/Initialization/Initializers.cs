using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public class Initializers : IInitializer
{
	readonly IInitializer[] _initializers;

	protected Initializers(params IInitializer[] initializers) => _initializers = initializers;

	public async ValueTask Get(DbContext parameter)
	{
		foreach (var initializer in _initializers)
		{
			await initializer.Off(parameter);
		}
	}
}