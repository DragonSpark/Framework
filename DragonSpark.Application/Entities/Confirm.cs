using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class Confirm : IResulting<int>
	{
		readonly DbContext _storage;

		public Confirm(DbContext storage) => _storage = storage;


		public ValueTask<int> Get() => _storage.SaveChangesAsync().ToOperation();
	}
}