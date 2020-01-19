using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DragonSpark.Composition
{
	sealed class RegisterModularity : IServiceConfiguration
	{
		[UsedImplicitly]
		public static RegisterModularity Default { get; } = new RegisterModularity();

		RegisterModularity() : this(TypeSelection<PublicAssemblyTypes>.Default.Get) {}

		readonly Func<Array<Type>, IComponentTypes> _locator;
		readonly Func<string, Array<Assembly>>      _select;
		readonly Func<Array<Assembly>, Array<Type>> _types;

		public RegisterModularity(Func<Array<Assembly>, Array<Type>> types)
			: this(ComponentTypeLocators.Default.Get, types, EnvironmentAwareAssemblies.Default.Get) {}

		public RegisterModularity(Func<Array<Type>, IComponentTypes> locator, Func<Array<Assembly>, Array<Type>> types,
		                          Func<string, Array<Assembly>> select)
		{
			_locator = locator;
			_types   = types;
			_select  = select;
		}

		public void Execute(IServiceCollection parameter)
		{
			var name       = parameter.GetRequiredInstance<IHostEnvironment>().EnvironmentName;
			var assemblies = _select(name);
			var types      = _types(assemblies);
			var locator    = _locator(types);

			parameter.AddSingleton<IArray<Assembly>>(new ArrayInstance<Assembly>(assemblies))
			         .AddSingleton<IArray<Type>>(new ArrayInstance<Type>(types))
			         .AddSingleton(locator)
			         .AddSingleton<IComponentType>(new ComponentType(locator));
		}
	}
}