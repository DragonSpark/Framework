using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public interface IInitializer<in T> : IOperation<T> where T : DbContext {}
}