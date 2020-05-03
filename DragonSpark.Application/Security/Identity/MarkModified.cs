using DragonSpark.Compose;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class MarkModified<T> : IMarkModified<T> where T : IdentityUser
	{
		readonly DbContext _context;
		readonly ITime     _time;

		public MarkModified(DbContext context) : this(context, Time.Default) {}

		public MarkModified(DbContext context, ITime time)
		{
			_context = context;
			_time    = time;
		}

		public ValueTask<int> Get(T parameter)
		{
			_context.Entry(parameter).Entity.Modified = _time.Get();
			var result = _context.SaveChangesAsync().ToOperation();
			return result;
		}
	}
}