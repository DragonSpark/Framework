using DragonSpark.Commands;
using System;
using System.Windows.Input;

namespace DragonSpark.Application
{
	public interface IApplication<in T> : ICommand<T>, IApplication {}

	public interface IApplication : ICommand, IDisposable {}
}