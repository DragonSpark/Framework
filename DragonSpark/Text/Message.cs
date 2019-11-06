using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text
{
	public class Message<T> : Select<T, string>, IMessage<T>
	{
		public Message(Func<T, string> select) : base(select) {}
	}
}