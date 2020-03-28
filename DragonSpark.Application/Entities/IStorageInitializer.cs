using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public interface IStorageInitializer<T> : IAlteration<T> where T : DbContext {}
}