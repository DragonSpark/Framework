using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Connections.Circuits;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<CreateCircuitRecord>()
				 .And<RegisterCircuit>()
				 .Scoped()
				 //
				 .Then.Start<CircuitHandler>()
				 .Forward<RecordAwareCircuitHandler>()
				 .Scoped()
				 //
				 .Then.Start<IConnectionIdentifier>()
				 .Forward<ConnectionIdentifier>()
				 .Singleton()
			;
	}
}

/*sealed class HostFieldDefinition : FieldDefinition<Circuit>
{
	public static HostFieldDefinition Default { get; } = new();

	HostFieldDefinition() : base("_circuitHost") {}
}

sealed class HostField : FieldAccessor<Circuit, IAsyncDisposable>
{
	public static HostField Default { get; } = new();

	HostField() : base(HostFieldDefinition.Default) {}
}

sealed class CircuitIdentifier : Formatter<Circuit>
{
	public static CircuitIdentifier Default { get; } = new();

	CircuitIdentifier() : base(HostField.Default.Then()
										.Select(ClientAccessor.Default)
										.Select(ConnectionIdentifierAccessor.Default)
										.Get()) {}
}

sealed class ClientAccessor : PropertyAccessor<object>
{
	public static ClientAccessor Default { get; } = new();

	ClientAccessor() : base("Client") {}
}

sealed class ConnectionIdentifierAccessor : PropertyAccessor<string>
{
	public static ConnectionIdentifierAccessor Default { get; } = new();

	ConnectionIdentifierAccessor() : base("ConnectionId") {}
}*/