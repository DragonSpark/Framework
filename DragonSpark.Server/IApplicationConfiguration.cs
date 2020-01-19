using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Server
{
	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}
}