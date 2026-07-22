using System.Threading.Tasks;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Dispatch : IDispatch
{
	readonly IWriteMessage _write;
	readonly string        _name;

	public Dispatch(IWriteMessage write, string name)
	{
		_write = write;
		_name  = name;
	}

	public ValueTask Get(Stop<MessageProperties> parameter)
	{
		var (((message, identifier), visibility, life), stop) = parameter;
		return _write.Get(new(new(identifier, message, _name, visibility, life), stop));
	}
}