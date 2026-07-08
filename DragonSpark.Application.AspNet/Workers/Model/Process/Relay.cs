using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Runtime.Activation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

sealed class Relay<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<ExternalProcess> _status;
	readonly Func<Type, T>               _new;

	public Relay(IStopAware<ExternalProcess> status) : this(status, NewInstance<T>.Default.Get) {}

	public Relay(IStopAware<ExternalProcess> status, Func<Type, T> @new)
	{
		_status = status;
		_new    = @new;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;
		var @new = _new(subject.GetType());
		@new.Id = subject.Id;
		await _status.Off(new(@new, stop));
		subject.Update(@new.Updates.Single());
	}
}