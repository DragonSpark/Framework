using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public sealed class DefaultStorageInitializer : Alteration<ModelBuilder>, IStorageInitializer
	{
		public static DefaultStorageInitializer Default { get; } = new DefaultStorageInitializer();

		DefaultStorageInitializer() : base(new StorageInitializer()) {}
	}
}