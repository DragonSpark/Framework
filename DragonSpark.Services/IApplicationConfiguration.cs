using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Services
{
	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}
}