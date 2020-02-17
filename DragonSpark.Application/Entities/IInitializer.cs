using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities {
	public interface IInitializer : ICommand<ModelBuilder> {}
}