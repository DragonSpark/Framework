using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public class RegistrationAwareProcessClientService : IHostedService
{
	readonly ProcessClientService            _previous;
	readonly IEnumerable<IEventRegistration> _registrations;
	readonly IEntries                        _entries;

	protected RegistrationAwareProcessClientService(ProcessClientService previous,
	                                                IEnumerable<IEventRegistration> registrations)
		: this(previous, registrations, Entries.Default) {}

	protected RegistrationAwareProcessClientService(ProcessClientService previous,
	                                                IEnumerable<IEventRegistration> registrations, IEntries entries)
	{
		_previous      = previous;
		_registrations = registrations;
		_entries       = entries;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		foreach (var registration in _registrations)
		{
			registration.Execute(_entries);
		}

		return _previous.StartAsync(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_entries.Execute();
		return _previous.StopAsync(cancellationToken);
	}
}