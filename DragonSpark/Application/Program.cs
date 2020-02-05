using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application
{
	public class Program : IProgram
	{
		readonly Func<IHostBuilder, IHostBuilder> _select;

		public Program(ISelect<IHostBuilder, IHostBuilder> select) : this(select.Get) {}

		public Program(Func<IHostBuilder, IHostBuilder> select) => _select = select;

		public Task Get(Array<string> arguments) => Get(Host.CreateDefaultBuilder(arguments));

		public Task Get(IHostBuilder parameter) => _select(parameter).Build().RunAsync();
	}
}