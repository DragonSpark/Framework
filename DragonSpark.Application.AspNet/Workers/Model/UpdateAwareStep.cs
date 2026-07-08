using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Workers.Model.States;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class UpdateAwareStep<T> : Appending<Stop<T>>, IStopAware<T> where T : ExternalProcess
{
	public UpdateAwareStep(IUpdate update, IEdit<ExternalProcess> edit, IStopAware<T> previous)
		: base(new Relay<T>(new AppendState(update, edit)), previous) {}
}