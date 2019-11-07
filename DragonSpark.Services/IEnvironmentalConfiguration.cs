using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace DragonSpark.Services
{
	public interface
		IEnvironmentalConfiguration : ICommand<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> {}
}