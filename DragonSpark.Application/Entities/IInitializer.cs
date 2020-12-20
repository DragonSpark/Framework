using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public interface IInitializer<T> : IAlteration<T> where T : DbContext {}

	public interface IInitializer : IAlteration<ModelBuilder> {}
}