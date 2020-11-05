using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	sealed class StateConnectionLock : LockInstance<DbContext>
	{
		public StateConnectionLock(DbContext context) : base(context) {}
	}
}