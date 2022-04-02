using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class AmbientOperations : DragonSpark.Model.Commands.Assume<Func<ValueTask>>, IAmbientOperations
{
	public static AmbientOperations Default { get; } = new();

	AmbientOperations() : base(CurrentAmbientOperations.Default) {}
}