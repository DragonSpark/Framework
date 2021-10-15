using DragonSpark.Compose;
using DragonSpark.Text;
using System;

namespace DragonSpark.Runtime.Environment;

sealed class LocateComponentMessage<T> : Message<Type>
{
	public static LocateComponentMessage<T> Default { get; } = new LocateComponentMessage<T>();

	LocateComponentMessage() :
		base(x => $"A request was made to locate a type of {A.Type<T>()}.  Its implementation type of {x} was located, but it could not be activated.") {}
}