using System;
using DragonSpark.Commands;

namespace DragonSpark.Application.Setup
{
	public interface ISetup : ICommand<object>, IDisposable, IPriorityAware {}
}