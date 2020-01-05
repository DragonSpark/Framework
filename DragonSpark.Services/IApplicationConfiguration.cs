using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Services
{
	sealed class ConfigureFromEnvironment : ICommand<IApplicationBuilder>
	{
		public static ConfigureFromEnvironment Default { get; } = new ConfigureFromEnvironment();

		ConfigureFromEnvironment() : this(A.Type<IApplicationConfiguration>(),
		                                  Start.A.Selection.Of.System.Type.By.Self.Then()
		                                       .Activate<IApplicationConfiguration>()
		                                       .Get) {}

		readonly Type                                  _type;
		readonly Func<Type, IApplicationConfiguration> _select;

		public ConfigureFromEnvironment(Type type, Func<Type, IApplicationConfiguration> select)
		{
			_type   = type;
			_select = select;
		}

		public void Execute(IApplicationBuilder parameter)
		{
			var implementation = parameter.ApplicationServices.GetRequiredService<IComponentType>().Get(_type);

			_select(implementation)?.Execute(parameter);
		}
	}

	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}

	sealed class ServerHostBuilder<T> : ISelect<Array<string>, IHost> where T : class
	{
		public static ServerHostBuilder<T> Default { get; } = new ServerHostBuilder<T>();

		ServerHostBuilder() {}

		public IHost Get(Array<string> parameter) => Host.CreateDefaultBuilder(parameter)
		                                                 .ConfigureWebHostDefaults(builder => builder.UseStartup<T>())
		                                                 .Build();
	}
}