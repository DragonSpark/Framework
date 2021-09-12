using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Components
{
	class Class1 {}

	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<ConnectionIdentifier>().And<ConnectionStartTime>().Scoped();
		}
	}

	public sealed class ConnectionIdentifier : Instance<Guid>
	{
		public ConnectionIdentifier() : base(Guid.NewGuid()) {}
	}

	public sealed class ConnectionStartTime : Instance<DateTimeOffset>
	{
		[UsedImplicitly]
		public ConnectionStartTime() : this(Time.Default) {}

		public ConnectionStartTime(ITime time) : base(time.Get()) {}
	}
}
