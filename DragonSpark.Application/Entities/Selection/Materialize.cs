using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class Materialize<T> : IResulting<Array<T>>
	{
		readonly IQueryable<T> _source;

		public Materialize(IQueryable<T> source) => _source = source;

		public async ValueTask<Array<T>> Get() => await _source.ToArrayAsync().ConfigureAwait(false);
	}
}
