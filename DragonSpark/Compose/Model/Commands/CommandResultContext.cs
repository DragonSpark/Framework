using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Compose.Model.Commands
{
	public class CommandResultContext<T> : ResultContext<ICommand<T>>
	{
		public CommandResultContext(IResult<ICommand<T>> instance) : base(instance) {}

		public CommandContext<T> Assume() => new DelegatedInstanceCommand<T>(Get()).Then();
	}
}