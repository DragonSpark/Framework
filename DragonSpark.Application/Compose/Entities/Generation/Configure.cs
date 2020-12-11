using AutoBogus;
using AutoBogus.Conventions;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	sealed class Configure<T> : ICommand<IAutoGenerateConfigBuilder> where T : class
	{
		public static Configure<T> Default { get; } = new Configure<T>();

		Configure() : this(ModelBinder<T>.Default) {}

		readonly IAutoBinder _binder;

		public Configure(IAutoBinder binder) => _binder = binder;

		public void Execute(IAutoGenerateConfigBuilder parameter)
		{
			parameter.WithConventions().WithBinder(_binder);
		}
	}
}