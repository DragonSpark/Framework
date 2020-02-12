using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application
{
	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}
}