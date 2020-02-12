using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class RegisterDependencies : ICommand<Type>
	{
		readonly Func<Type, IServiceCollection> _apply;
		readonly Func<Type, bool>               _can;
		readonly IArray<Type, Type>             _candidates;

		public RegisterDependencies(IServiceCollection services) : this(services, services.AddScoped) {}

		public RegisterDependencies(IServiceCollection services, Func<Type, IServiceCollection> apply)
			: this(apply, new CanRegister(services).Get, DependencyCandidates.Default) {}

		public RegisterDependencies(Func<Type, IServiceCollection> apply, Func<Type, bool> can,
		                            IArray<Type, Type> candidates)
		{
			_apply      = apply;
			_can        = can;
			_candidates = candidates;
		}

		public void Execute(Type parameter)
		{
			var set = new HashSet<Type>();
			foreach (var candidate in _candidates.Get(parameter).Open())
			{
				if (set.Add(candidate) && _can(candidate))
				{
					_apply(candidate);
				}
			}
		}
	}
}