using DragonSpark.Runtime.Execution;
using System;

namespace DragonSpark.Application.AspNet.Entities;

sealed class AmbientLock : Logical<IDisposable?>
{
	public static AmbientLock Default { get; } = new();

	AmbientLock() {}
}