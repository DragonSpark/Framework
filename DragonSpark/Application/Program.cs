using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application {
	public class Program : Select<IHost, Task>, IProgram
	{
		public Program(ISelect<IHost, Task> select) : base(select) {}

		public Program(Func<IHost, Task> select) : base(select) {}
	}
}