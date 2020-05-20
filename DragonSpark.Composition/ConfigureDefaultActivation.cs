using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using LightInject;
using System;

namespace DragonSpark.Composition
{
	sealed class ConfigureDefaultActivation : ICommand<IServiceContainer>
	{
		[UsedImplicitly]
		public static ConfigureDefaultActivation Default { get; } = new ConfigureDefaultActivation();

		ConfigureDefaultActivation() : this(IsFallbackCandidate.Default.Get, Start.A.Selection<ServiceRequest>()
		                                                                          .By.Calling(x => x.ServiceType)
		                                                                          .Then()
		                                                                          .Activate()) {}

		readonly Func<Type, string, bool>     _condition;
		readonly Func<ServiceRequest, object> _select;

		public ConfigureDefaultActivation(Func<Type, string, bool> condition, Func<ServiceRequest, object> select)
		{
			_condition = condition;
			_select    = select;
		}

		public void Execute(IServiceContainer parameter)
		{
			parameter.RegisterInstance(parameter)
			         .RegisterFallback(_condition, _select);
		}
	}
}