using DragonSpark.Text;
using System;

namespace DragonSpark.Model;

sealed class AssignedArgumentMessage : Message<Type>
{
	public static AssignedArgumentMessage Default { get; } = new AssignedArgumentMessage();

	AssignedArgumentMessage()
		: base(x => $"Expected an argument of type {x} to be assigned, but the provided instance was not.") {}
}