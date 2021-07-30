using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public interface IStorageInitializer<T> : IAlteration<T> where T : DbContext {}


	public interface IStorageInitializer : IAlteration<ModelBuilder> {}

	public class StorageInitializer : Alteration<ModelBuilder>, IStorageInitializer
	{
		public StorageInitializer(params IInitializer[] initializers) : base(initializers.Alter) {}
	}
}