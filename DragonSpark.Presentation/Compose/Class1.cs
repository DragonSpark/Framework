using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Presentation.Compose
{
	class Class1 {}

	public sealed class Receiver : IResult<object>
	{
		readonly Action                 _receiver;
		readonly ITable<object, Action> _receivers;

		public Receiver(Action receiver) : this(receiver, Receivers.Default) {}

		public Receiver(Action receiver, ITable<object, Action> receivers)
		{
			_receiver  = receiver;
			_receivers = receivers;
		}

		public object Get()
		{
			var result = _receiver.Target.Verify();
			_receivers.Assign(result, _receiver);
			return result;
		}
	}

	public sealed class Receivers : ReferenceValueTable<object, Action>
	{
		public static Receivers Default { get; } = new Receivers();

		Receivers() {}
	}
}