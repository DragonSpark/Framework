using DragonSpark.Model.Operations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application
{
	public interface IHostInitializer : IOperation<IHost> {}
}