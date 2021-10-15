using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text;

public class Message<T> : Select<T, string>, IMessage<T>
{
	public Message(Func<T, string> select) : base(select) {}
}