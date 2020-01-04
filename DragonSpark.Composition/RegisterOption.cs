using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DragonSpark.Composition
{
	sealed class RegisterOption<T> : IAlteration<IServiceCollection> where T : class, new()
	{
		public static RegisterOption<T> Default { get; } = new RegisterOption<T>();

		RegisterOption() : this(A.Type<T>().Name) {}

		readonly string _name;

		public RegisterOption(string name) => _name = name;

		public IServiceCollection Get(IServiceCollection parameter)
			=> parameter.Configure<T>(parameter.Configuration().GetSection(_name))
			            .AddSingleton(x => x.GetRequiredService<IOptions<T>>().Value)
			            .Return(parameter);
	}
}