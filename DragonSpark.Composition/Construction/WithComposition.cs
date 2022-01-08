using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DragonSpark.Composition.Construction;

sealed class WithComposition : IAlteration<IHostBuilder>
{
	public static WithComposition Default { get; } = new();

	WithComposition() : this(ConstructionInfoProvider.Default) {}

	readonly FieldInfo _provider;

	public WithComposition(FieldInfo provider) => _provider = provider;

	public IHostBuilder Get(IHostBuilder parameter)
	{
		var options = ContainerOptions.Default.Clone().WithMicrosoftSettings().WithAspNetCoreSettings();
		var root    = new ServiceContainer(options);
		root.ConstructorSelector = new ConstructorSelector(new CanSelectDependency(root, options).Get);

		var current = _provider.GetValue(root).Verify().To<Lazy<IConstructionInfoProvider>>();

		_provider.SetValue(root, new Lazy<IConstructionInfoProvider>(new Construction(current.Value)));

		var @default = new LightInjectServiceProviderFactory(root);
		var factory  = new Factory(@default);
		var result   = parameter.UseServiceProviderFactory(factory);
		return result;
	}
}