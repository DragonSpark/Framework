using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Application
{
	public class ActivatedProgram<T> : IProgram where T : ISelect<IHost, Task>
	{
		protected ActivatedProgram() {}

		public Task Get(IHost parameter) => parameter.Services.GetRequiredService<T>().Get(parameter);
	}
}