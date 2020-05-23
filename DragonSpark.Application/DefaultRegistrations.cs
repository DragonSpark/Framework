using DragonSpark.Application.Entities;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application
{
	sealed class DefaultRegistrations : ICommand<IServiceCollection>
	{
		public static DefaultRegistrations Default { get; } = new DefaultRegistrations();

		DefaultRegistrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.For<IStorageState>()
			         .Map<StorageState>()
			         .Scoped()
			         //
			         // TODO
			         .For<IToken>()
			         .Map<Token>()
			         .Scoped();
		}
	}
}