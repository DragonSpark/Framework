using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	sealed class Clear : IClear
	{
		readonly DbContext _context;

		public Clear(DbContext context) => _context = context;

		public void Execute(None parameter)
		{
			_context.ChangeTracker.Clear();
		}
	}
}