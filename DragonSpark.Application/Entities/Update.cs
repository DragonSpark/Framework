using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	sealed class Update<T> : IUpdate<T> where T : class
	{
		readonly DbContext _context;

		public Update(DbContext context) => _context = context;

		public void Execute(T parameter)
		{
			_context.Set<T>().Update(parameter);
		}
	}
}