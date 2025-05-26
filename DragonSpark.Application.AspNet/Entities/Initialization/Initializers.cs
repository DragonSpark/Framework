using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public class Initializers : IInitializer
{
	readonly IInitializer[] _initializers;

	protected Initializers(params IInitializer[] initializers) => _initializers = initializers;

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		foreach (var initializer in _initializers)
		{
			await initializer.Off(parameter);
		}
	}
}