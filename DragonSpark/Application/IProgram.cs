using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Application;

public interface IProgram : ISelect<IHostBuilder, Task> {}